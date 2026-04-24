using AVADH_PRIME_Consume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AVADH_PRIME_Consume.Controllers
{
    public class WardenController : Controller
    {
        private readonly HttpClient _httpClient;

        public WardenController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("api");
        }

        // 🔹 GET: Warden List
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("Warden");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "API Error while fetching wardens";
                    return View(new List<WardenModel>());
                }

                var data = await response.Content.ReadAsStringAsync();
                var wardens = JsonConvert.DeserializeObject<List<WardenModel>>(data)
                              ?? new List<WardenModel>();

                return View(wardens);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<WardenModel>());
            }
        }

        // 🔹 GET: Add
        public IActionResult Add()
        {
            return View(new WardenModel
            {
                DateOfBirth = DateTime.Today.AddYears(-25),
                JoiningDate = DateTime.Today,
                IsActive = true,
                Status = "Active",
                CreatedAt = DateTime.Now
            });
        }

        // 🔹 GET: Edit
        public async Task<IActionResult> Edit(int Warden_Id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Warden/{Warden_Id}");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Failed to load warden";
                    return RedirectToAction("Index");
                }

                var data = await response.Content.ReadAsStringAsync();
                var warden = JsonConvert.DeserializeObject<WardenModel>(data);

                if (warden == null)
                    return RedirectToAction("Index");

                return View("Add", warden);
            }
            catch
            {
                TempData["Error"] = "Something went wrong!";
                return RedirectToAction("Index");
            }
        }

        // 🔹 POST: Add / Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(WardenModel warden)
        {
            if (!ModelState.IsValid)
                return View(warden);

            // ✅ IMAGE UPLOAD
            if (warden.ImageFile != null)
            {
                warden.Warden_Image = await SaveImage(warden.ImageFile);
            }

            // ✅ DOCUMENT UPLOAD
            if (warden.IDProofFile != null)
            {
                warden.ID_Proof_Path = await SaveDocument(warden.IDProofFile);
            }

            // ✅ KEEP OLD FILES (EDIT CASE)
            if (warden.Warden_Id != 0)
            {
                var existing = await _httpClient.GetAsync($"Warden/{warden.Warden_Id}");
                var data = await existing.Content.ReadAsStringAsync();
                var oldWarden = JsonConvert.DeserializeObject<WardenModel>(data);

                if (warden.ImageFile == null)
                    warden.Warden_Image = oldWarden?.Warden_Image;

                if (warden.IDProofFile == null)
                    warden.ID_Proof_Path = oldWarden?.ID_Proof_Path;
            }

            // ✅ DATE HANDLING
            if (warden.Warden_Id == 0)
                warden.CreatedAt = DateTime.Now;
            else
                warden.UpdatedAt = DateTime.Now;

            var json = JsonConvert.SerializeObject(warden);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            if (warden.Warden_Id == 0)
                response = await _httpClient.PostAsync("Warden", content);
            else
                response = await _httpClient.PutAsync($"Warden/{warden.Warden_Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "API Error: " + error);
                return View(warden);
            }

            TempData["Success"] = warden.Warden_Id == 0
                ? "Warden added successfully!"
                : "Warden updated successfully!";

            return RedirectToAction("Index");
        }

        // 🔹 DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Warden_Id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Warden/{Warden_Id}");

                if (!response.IsSuccessStatusCode)
                    TempData["Error"] = "Delete failed!";
                else
                    TempData["Success"] = "Warden deleted successfully!";

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = "Something went wrong!";
                return RedirectToAction("Index");
            }
        }

        // 🔹 DETAILS
        public async Task<IActionResult> Details(int Warden_Id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Warden/{Warden_Id}");

                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var data = await response.Content.ReadAsStringAsync();
                var warden = JsonConvert.DeserializeObject<WardenModel>(data);

                if (warden == null)
                    return NotFound();

                return View(warden);
            }
            catch
            {
                return NotFound();
            }
        }

        // 🔹 SAVE IMAGE
        private async Task<string> SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/wardens");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/wardens/" + fileName;
        }

        // 🔹 SAVE DOCUMENT
        private async Task<string> SaveDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/documents");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/documents/" + fileName;
        }
    }
}



//using AVADH_PRIME_Consume.Models;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System.Text;

//namespace AVADH_PRIME_Consume.Controllers
//{
//    public class WardenController : Controller
//    {
//        private readonly HttpClient _httpClient;

//        public WardenController(IHttpClientFactory factory)
//        {
//            _httpClient = factory.CreateClient("api");
//        }

//        // 🔹 GET: Warden list
//        public async Task<IActionResult> Index()
//        {
//            var response = await _httpClient.GetAsync("Warden");

//            if (!response.IsSuccessStatusCode)
//            {
//                ViewBag.Error = "API Error";
//                return View(new List<WardenModel>());
//            }

//            var data = await response.Content.ReadAsStringAsync();
//            var wardens = JsonConvert.DeserializeObject<List<WardenModel>>(data)
//                          ?? new List<WardenModel>();

//            return View(wardens);
//        }

//        // 🔹 GET: Add
//        public IActionResult Add()
//        {
//            return View(new WardenModel
//            {
//                DateOfBirth = DateTime.Today.AddYears(-25),
//                JoiningDate = DateTime.Today,
//                IsActive = true,
//                Status = "Active",
//                CreatedAt = DateTime.Now
//            });
//        }

//        // 🔹 GET: Edit
//        public async Task<IActionResult> Edit(int Warden_Id)
//        {
//            var response = await _httpClient.GetAsync($"Warden/{Warden_Id}");

//            if (!response.IsSuccessStatusCode)
//                return RedirectToAction("Index");

//            var data = await response.Content.ReadAsStringAsync();
//            var warden = JsonConvert.DeserializeObject<WardenModel>(data);

//            return View("Add", warden);
//        }

//        // 🔹 POST: Add / Update
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Add(WardenModel warden)
//        {
//            if (!ModelState.IsValid)
//                return View(warden);

//            // ✅ KEEP OLD IMAGE if not uploading new
//            if (warden.ImageFile != null)
//            {
//                warden.Warden_Image = await SaveImage(warden.ImageFile);
//            }

//            // ❗ IMPORTANT FIX
//            else if (warden.Warden_Id != 0)
//            {
//                var existing = await _httpClient.GetAsync($"Warden/{warden.Warden_Id}");
//                var data = await existing.Content.ReadAsStringAsync();
//                var oldWarden = JsonConvert.DeserializeObject<WardenModel>(data);
//                warden.Warden_Image = oldWarden?.Warden_Image;
//            }

//            if (warden.Warden_Id == 0)
//                warden.CreatedAt = DateTime.Now;
//            else
//                warden.UpdatedAt = DateTime.Now;

//            var json = JsonConvert.SerializeObject(warden);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");

//            HttpResponseMessage response;

//            if (warden.Warden_Id == 0)
//                response = await _httpClient.PostAsync("Warden", content);
//            else
//                response = await _httpClient.PutAsync($"Warden/{warden.Warden_Id}", content);

//            if (!response.IsSuccessStatusCode)
//            {
//                var error = await response.Content.ReadAsStringAsync();
//                ModelState.AddModelError("", "API Error: " + error);
//                return View(warden);
//            }

//            TempData["Success"] = warden.Warden_Id == 0
//                ? "Warden added successfully!"
//                : "Warden updated successfully!";

//            return RedirectToAction("Index");
//        }

//        // 🔹 POST: Delete
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Delete(int Warden_Id)
//        {
//            var response = await _httpClient.DeleteAsync($"Warden/{Warden_Id}");

//            if (!response.IsSuccessStatusCode)
//                TempData["Error"] = "Delete failed!";

//            return RedirectToAction("Index");
//        }

//        // 🔹 GET: Details
//        public async Task<IActionResult> Details(int Warden_Id)
//        {
//            var response = await _httpClient.GetAsync($"Warden/{Warden_Id}");

//            if (!response.IsSuccessStatusCode)
//                return NotFound();

//            var data = await response.Content.ReadAsStringAsync();
//            var warden = JsonConvert.DeserializeObject<WardenModel>(data);

//            if (warden == null)
//                return NotFound();

//            return View(warden);
//        }


//        // 🔹 IMAGE SAVE
//        private async Task<string> SaveImage(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//                return null;

//            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/wardens");

//            if (!Directory.Exists(folder))
//                Directory.CreateDirectory(folder);

//            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

//            string filePath = Path.Combine(folder, fileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            // ✅ MUST return FULL path

//            return "/uploads/wardens/" + fileName;
//        }

//        // 🔹 SAVE DOCUMENT
//        private async Task<string> SaveDocument(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//                return null;

//            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/documents");

//            if (!Directory.Exists(folder))
//                Directory.CreateDirectory(folder);

//            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
//            string filePath = Path.Combine(folder, fileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            return "/uploads/documents/" + fileName;
//        }
//    }
//}
