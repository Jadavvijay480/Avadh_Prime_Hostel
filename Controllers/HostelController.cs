using AVADH_PRIME_Consume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AVADH_PRIME_Consume.Controllers
{
   
        public class HostelController : Controller
        {
            private readonly HttpClient _httpClient;

            public HostelController(IHttpClientFactory factory)
            {
                _httpClient = factory.CreateClient("api");
            }

            // 🔹 GET: Hostel list
            public async Task<IActionResult> Index()
            {
                var response = await _httpClient.GetAsync("Hostel");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "API Error";
                    return View(new List<HostelModel>());
                }

                var data = await response.Content.ReadAsStringAsync();
                var hostels = JsonConvert.DeserializeObject<List<HostelModel>>(data)
                              ?? new List<HostelModel>();

                return View(hostels);
            }

            // 🔹 GET: Add
            public IActionResult Add()
            {
                return View(new HostelModel
                {
                    CreatedAt = DateTime.Today,
                    IsActive = true
                });
            }

            // 🔹 GET: Edit
            public async Task<IActionResult> Edit(int Hostel_Id)
            {
                var response = await _httpClient.GetAsync($"Hostel/{Hostel_Id}");

                if (!response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                var data = await response.Content.ReadAsStringAsync();
                var hostel = JsonConvert.DeserializeObject<HostelModel>(data);

                return View("Add", hostel);
            }

            // 🔹 POST: Add / Update
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Add(HostelModel hostel)
            {
                if (!ModelState.IsValid)
                    return View(hostel);

                var json = JsonConvert.SerializeObject(hostel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = hostel.Hostel_Id == 0
                    ? await _httpClient.PostAsync("Hostel", content)
                    : await _httpClient.PutAsync($"Hostel/{hostel.Hostel_Id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", "API Error: " + error);
                    return View(hostel);
                }

                TempData["Success"] = hostel.Hostel_Id == 0
                    ? "Hostel added successfully!"
                    : "Hostel updated successfully!";

                return RedirectToAction("Index");
            }

            // 🔹 POST: Delete
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Delete(int Hostel_Id)
            {
                var response = await _httpClient.DeleteAsync($"Hostel/{Hostel_Id}");

                if (!response.IsSuccessStatusCode)
                    TempData["Error"] = "Delete failed!";

                return RedirectToAction("Index");
            }
        }
    }

