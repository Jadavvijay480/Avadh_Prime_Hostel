using AVADH_PRIME_Consume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AVADH_PRIME_Consume.Controllers
{
    public class VisitorsController : Controller
    {
        private readonly HttpClient _httpClient;

        public VisitorsController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("api");
        }

        // 🔹 GET: List
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("Visitors");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "API Error";
                return View(new List<VisitorsModel>());
            }

            var data = await response.Content.ReadAsStringAsync();
            var visitors = JsonConvert.DeserializeObject<List<VisitorsModel>>(data)
                           ?? new List<VisitorsModel>();

            return View(visitors);
        }

        // 🔹 GET: Add
        public IActionResult Add()
        {
            return View(new VisitorsModel
            {
                Visit_Date = DateTime.Today,
                Check_In_Time = DateTime.Now,
                Is_Approved = false,
                Is_Deleted = false,
                Created_At = DateTime.Now
            });
        }

        // 🔹 GET: Edit
        public async Task<IActionResult> Edit(int Visitor_Id)
        {
            var response = await _httpClient.GetAsync($"Visitors/{Visitor_Id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var data = await response.Content.ReadAsStringAsync();
            var visitor = JsonConvert.DeserializeObject<VisitorsModel>(data);

            if (visitor == null)
                return RedirectToAction("Index");

            return View("Add", visitor);
        }

        // 🔹 POST: Add / Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(VisitorsModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            if (model.Visitor_Id == 0)
            {
                response = await _httpClient.PostAsync("Visitors", content);
            }
            else
            {
                response = await _httpClient.PutAsync($"Visitors/{model.Visitor_Id}", content);
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "API Error: " + error);
                return View(model);
            }

            TempData["Success"] = model.Visitor_Id == 0
                ? "Visitor added successfully!"
                : "Visitor updated successfully!";

            return RedirectToAction("Index");
        }

        // 🔹 GET: Details
        public async Task<IActionResult> Details(int Visitor_Id)
        {
            var response = await _httpClient.GetAsync($"Visitors/{Visitor_Id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var data = await response.Content.ReadAsStringAsync();
            var visitor = JsonConvert.DeserializeObject<VisitorsModel>(data);

            if (visitor == null)
                return NotFound();

            return View(visitor);
        }

        // 🔹 POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Visitor_Id)
        {
            var response = await _httpClient.DeleteAsync($"Visitors/{Visitor_Id}");

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Delete failed!";

            return RedirectToAction("Index");
        }
    }
}