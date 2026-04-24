using Microsoft.AspNetCore.Mvc;
using System.Text;
using AVADH_PRIME_Consume.Models;
using Newtonsoft.Json;

namespace AVADH_PRIME_Consume.Controllers
{
    public class FeesReceiptsController : Controller
    {
        private readonly HttpClient _httpClient;

        public FeesReceiptsController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("api");
        }

        // 🔹 GET: List
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("FeesReceipts");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "API Error";
                return View(new List<FeesReceiptsModel>());
            }

            var data = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<FeesReceiptsModel>>(data)
                       ?? new List<FeesReceiptsModel>();

            return View(list);
        }

        // 🔹 GET: Add
        public IActionResult Add()
        {
            return View(new FeesReceiptsModel
            {
                Payment_Date = DateTime.Now,
                CreatedAt = DateTime.Now,
                IsActive = true
            });
        }

        // 🔹 GET: Edit
        public async Task<IActionResult> Edit(int Receipt_Id)
        {
            var response = await _httpClient.GetAsync($"FeesReceipts/{Receipt_Id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var data = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<FeesReceiptsModel>(data);

            return View("Add", model);
        }

        // 🔹 POST: Add / Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(FeesReceiptsModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = model.Receipt_Id == 0
                ? await _httpClient.PostAsync("FeesReceipts", content)
                : await _httpClient.PutAsync($"FeesReceipts/{model.Receipt_Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "API Error: " + error);
                return View(model);
            }

            TempData["Success"] = model.Receipt_Id == 0
                ? "Receipt added successfully!"
                : "Receipt updated successfully!";

            return RedirectToAction("Index");
        }

        // 🔹 POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Receipt_Id)
        {
            var response = await _httpClient.DeleteAsync($"FeesReceipts/{Receipt_Id}");

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Delete failed!";

            return RedirectToAction("Index");
        }
    }
}