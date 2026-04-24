using Microsoft.AspNetCore.Mvc;
using System.Text;
using AVADH_PRIME_Consume.Models;
using Newtonsoft.Json;

namespace AVADH_PRIME_Consume.Controllers
{
    public class RoomsAllocationController : Controller
    {
        private readonly HttpClient _httpClient;

        public RoomsAllocationController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("api");
        }

        // 🔹 GET: List
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("RoomsAllocation");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "API Error";
                return View(new List<RoomsAllocationModel>());
            }

            var data = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<RoomsAllocationModel>>(data)
                       ?? new List<RoomsAllocationModel>();

            return View(list);
        }

        // 🔹 GET: Add
        public IActionResult Add()
        {
            return View(new RoomsAllocationModel
            {
                Allocation_Date = DateTime.Now,
                CreatedAt = DateTime.Now,
                Status = "Allocated"
            });
        }

        // 🔹 GET: Edit
        public async Task<IActionResult> Edit(int Allocation_Id)
        {
            var response = await _httpClient.GetAsync($"RoomsAllocation/{Allocation_Id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var data = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<RoomsAllocationModel>(data);

            return View("Add", model);
        }

        // 🔹 POST: Add / Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoomsAllocationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = model.Allocation_Id == 0
                ? await _httpClient.PostAsync("RoomsAllocation", content)
                : await _httpClient.PutAsync($"RoomsAllocation/{model.Allocation_Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "API Error: " + error);
                return View(model);
            }

            TempData["Success"] = model.Allocation_Id == 0
                ? "Room allocated successfully!"
                : "Room allocation updated successfully!";

            return RedirectToAction("Index");
        }

        // 🔹 POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Allocation_Id)
        {
            var response = await _httpClient.DeleteAsync($"RoomsAllocation/{Allocation_Id}");

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Delete failed!";

            return RedirectToAction("Index");
        }
    }
}