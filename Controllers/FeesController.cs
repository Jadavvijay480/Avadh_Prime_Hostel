using Microsoft.AspNetCore.Mvc;
using AVADH_PRIME_Consume.Models;
using Newtonsoft.Json;
using System.Text;

namespace AVADH_PRIME_Consume.Controllers
{
    public class FeesController : Controller
    {
        private readonly HttpClient _httpClient;

        public FeesController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("api");
        }

        // 🔹 GET: Fees list
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("StudentFees");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "API Error";
                return View(new List<FeesModel>());
            }

            var data = await response.Content.ReadAsStringAsync();
            var fees = JsonConvert.DeserializeObject<List<FeesModel>>(data)
                       ?? new List<FeesModel>();

            return View(fees);
        }

        // 🔹 GET: Add
        public IActionResult Add()
        {
            return View(new FeesModel
            {
                Fee_Date = DateTime.Today,
                Due_Date = DateTime.Today.AddDays(30),
                Paid_Amount = 0,
                Late_Fine = 0,
                IsActive = true,
                CreatedAt = DateTime.Now
            });
        }

        // 🔹 GET: Edit
        public async Task<IActionResult> Edit(int Fees_Id)
        {
            var response = await _httpClient.GetAsync($"StudentFees/{Fees_Id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var data = await response.Content.ReadAsStringAsync();
            var fee = JsonConvert.DeserializeObject<FeesModel>(data);

            return View("Add", fee);
        }

        // 🔹 POST: Add / Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(FeesModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = model.Fees_Id == 0
                ? await _httpClient.PostAsync("StudentFees", content)
                : await _httpClient.PutAsync($"StudentFees/{model.Fees_Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "API Error: " + error);
                return View(model);
            }

            TempData["Success"] = model.Fees_Id == 0
                ? "Fees record added successfully!"
                : "Fees record updated successfully!";

            return RedirectToAction("Index");
        }

        // 🔹 POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Fees_Id)
        {
            var response = await _httpClient.DeleteAsync($"StudentFees/{Fees_Id}");

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Delete failed!";

            return RedirectToAction("Index");
        }

        // 🔹 GET: Fees Details
        public async Task<IActionResult> Details(int Fees_Id)
        {
            var response = await _httpClient.GetAsync($"StudentFees/{Fees_Id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var data = await response.Content.ReadAsStringAsync();
            var fees = JsonConvert.DeserializeObject<FeesModel>(data);

            if (fees == null)
                return NotFound();

            return View(fees);
        }
    }
}
