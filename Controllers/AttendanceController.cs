using Microsoft.AspNetCore.Mvc;
using System.Text;
using AVADH_PRIME_Consume.Models;
using Newtonsoft.Json;


namespace AVADH_PRIME_Consume.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly HttpClient _httpClient;

        public AttendanceController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("api");
        }

        // 🔹 GET: Attendance list
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("StudentAttendance");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "API Error";
                return View(new List<AttendanceModel>());
            }

            var data = await response.Content.ReadAsStringAsync();
            var attendance = JsonConvert.DeserializeObject<List<AttendanceModel>>(data)
                             ?? new List<AttendanceModel>();

            return View(attendance);
        }

        // 🔹 GET: Add
        public IActionResult Add()
        {
            return View(new AttendanceModel
            {
                Attendance_Date = DateTime.Today,
                Status = "Present",
                CreatedAt = DateTime.Now
            });
        }

        // 🔹 GET: Edit
        public async Task<IActionResult> Edit(int Attendance_Id)
        {
            var response = await _httpClient.GetAsync($"StudentAttendance/{Attendance_Id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var data = await response.Content.ReadAsStringAsync();
            var attendance = JsonConvert.DeserializeObject<AttendanceModel>(data);

            return View("Add", attendance);
        }

        // 🔹 POST: Add / Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AttendanceModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = model.Attendance_Id == 0
                ? await _httpClient.PostAsync("StudentAttendance", content)
                : await _httpClient.PutAsync($"StudentAttendance/{model.Attendance_Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "API Error: " + error);
                return View(model);
            }

            TempData["Success"] = model.Attendance_Id == 0
                ? "Attendance marked successfully!"
                : "Attendance updated successfully!";

            return RedirectToAction("Index");
        }

        // 🔹 POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Attendance_Id)
        {
            var response = await _httpClient.DeleteAsync($"StudentAttendance/{Attendance_Id}");

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Delete failed!";

            return RedirectToAction("Index");
        }
    }
}
