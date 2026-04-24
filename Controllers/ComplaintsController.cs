using Microsoft.AspNetCore.Mvc;
using System.Text;
using AVADH_PRIME_Consume.Models;
using Newtonsoft.Json;


namespace AVADH_PRIME_Consume.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ComplaintsController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("api");
        }

        // 🔹 GET: Complaint list
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("StudentComplaints");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "API Error";
                return View(new List<ComplaintsModel>());
            }

            var data = await response.Content.ReadAsStringAsync();
            var complaints = JsonConvert.DeserializeObject<List<ComplaintsModel>>(data)
                             ?? new List<ComplaintsModel>();

            return View(complaints);
        }

        // 🔹 GET: Add
        public IActionResult Add()
        {
            return View(new ComplaintsModel
            {
                Complaint_Date = DateTime.Today,
                Status = "Pending",
                Priority = "Medium",
                CreatedAt = DateTime.Now
            });
        }

        // 🔹 GET: Edit
        public async Task<IActionResult> Edit(int Complaint_Id)
        {
            var response = await _httpClient.GetAsync($"StudentComplaints/{Complaint_Id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var data = await response.Content.ReadAsStringAsync();
            var complaint = JsonConvert.DeserializeObject<ComplaintsModel>(data);

            return View("Add", complaint);
        }

        // 🔹 POST: Add / Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ComplaintsModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 🔹 FILE UPLOAD
            if (model.AttachmentFile != null)
            {
                model.Attachment_Path = await SaveAttachment(model.AttachmentFile);
            }
            else if (model.Complaint_Id != 0)
            {
                // KEEP OLD FILE ON UPDATE
                var existing = await _httpClient.GetAsync($"StudentComplaints/{model.Complaint_Id}");
                var data = await existing.Content.ReadAsStringAsync();
                var old = JsonConvert.DeserializeObject<ComplaintsModel>(data);

                model.Attachment_Path = old?.Attachment_Path;
            }

            // 🔹 AUDIT
            if (model.Complaint_Id == 0)
                model.CreatedAt = DateTime.Now;
            else
                model.UpdatedAt = DateTime.Now;

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = model.Complaint_Id == 0
                ? await _httpClient.PostAsync("StudentComplaints", content)
                : await _httpClient.PutAsync($"StudentComplaints/{model.Complaint_Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "API Error: " + error);
                return View(model);
            }

            TempData["Success"] = model.Complaint_Id == 0
                ? "Complaint submitted successfully!"
                : "Complaint updated successfully!";

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Add(ComplaintsModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    // ✅ FIX: Handle audit fields properly
        //    if (model.Complaint_Id == 0)
        //    {
        //        model.CreatedAt = DateTime.Now;
        //    }
        //    else
        //    {
        //        model.UpdatedAt = DateTime.Now;
        //    }

        //    var json = JsonConvert.SerializeObject(model);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = model.Complaint_Id == 0
        //        ? await _httpClient.PostAsync("StudentComplaints", content)
        //        : await _httpClient.PutAsync($"StudentComplaints/{model.Complaint_Id}", content);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        var error = await response.Content.ReadAsStringAsync();
        //        ModelState.AddModelError("", "API Error: " + error);
        //        return View(model);
        //    }

        //    TempData["Success"] = model.Complaint_Id == 0
        //        ? "Complaint submitted successfully!"
        //        : "Complaint updated successfully!";

        //    return RedirectToAction("Index");
        //}

        // 🔹 POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Complaint_Id)
        {
            var response = await _httpClient.DeleteAsync($"StudentComplaints/{Complaint_Id}");

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Delete failed!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int Complaint_Id)
        {
            var response = await _httpClient.GetAsync($"StudentComplaints/{Complaint_Id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var data = await response.Content.ReadAsStringAsync();
            var complaint = JsonConvert.DeserializeObject<ComplaintsModel>(data);

            if (complaint == null)
                return NotFound();

            return View(complaint);
        }

        // 🔹 SAVE ATTACHMENT
        private async Task<string> SaveAttachment(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/complaints");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/complaints/" + fileName;
        }
    }
}
