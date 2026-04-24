using AVADH_PRIME_Consume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AVADH_PRIME_Consume.Controllers
{
    public class StudentController : Controller
    {
        private readonly HttpClient _httpClient;

        public StudentController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("api");
        }

        // 🔹 GET: Student list
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("Student");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "API Error while fetching students";
                    return View(new List<StudentModel>());
                }

                var data = await response.Content.ReadAsStringAsync();
                var students = JsonConvert.DeserializeObject<List<StudentModel>>(data)
                               ?? new List<StudentModel>();

                return View(students);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return View(new List<StudentModel>());
            }
        }

        // 🔹 GET: Add
        public IActionResult Add()
        {
            return View(new StudentModel
            {
                AdmissionDate = DateTime.Today,
                IsActive = true,
                Status = "Active"
            });
        }

        // 🔹 GET: Edit
        public async Task<IActionResult> Edit(int Student_Id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Student/{Student_Id}");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Failed to load student";
                    return RedirectToAction("Index");
                }

                var data = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<StudentModel>(data);

                if (student == null)
                    return RedirectToAction("Index");

                return View("Add", student);
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
        public async Task<IActionResult> Add(StudentModel student)
        {
            if (!ModelState.IsValid)
                return View(student);

            StudentModel? oldStudent = null;

            // 🔹 GET OLD DATA (FOR EDIT)
            if (student.Student_Id != 0)
            {
                var existing = await _httpClient.GetAsync($"Student/{student.Student_Id}");
                var data = await existing.Content.ReadAsStringAsync();
                oldStudent = JsonConvert.DeserializeObject<StudentModel>(data);
            }

            // ✅ IMAGE UPLOAD
            if (student.ImageFile != null)
            {
                student.student_image = await SaveImage(student.ImageFile);
            }
            else if (oldStudent != null)
            {
                student.student_image = oldStudent.student_image;
            }

            // ✅ DOCUMENT UPLOAD
            if (student.IDProofFile != null)
            {
                student.ID_Proof_Path = await SaveDocument(student.IDProofFile);
            }
            else if (oldStudent != null)
            {
                student.ID_Proof_Path = oldStudent.ID_Proof_Path;
            }

            // ✅ DATE HANDLING
            if (student.Student_Id == 0)
                student.CreatedAt = DateTime.Now;
            else
                student.UpdatedAt = DateTime.Now;

            var json = JsonConvert.SerializeObject(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            if (student.Student_Id == 0)
                response = await _httpClient.PostAsync("Student", content);
            else
                response = await _httpClient.PutAsync($"Student/{student.Student_Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "API Error: " + error);
                return View(student);
            }

            TempData["Success"] = student.Student_Id == 0
                ? "Student added successfully!"
                : "Student updated successfully!";

            return RedirectToAction("Index");
        }

        // 🔹 DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Student_Id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Student/{Student_Id}");

                if (!response.IsSuccessStatusCode)
                    TempData["Error"] = "Delete failed!";
                else
                    TempData["Success"] = "Student deleted successfully!";

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = "Something went wrong!";
                return RedirectToAction("Index");
            }
        }

        // 🔹 DETAILS
        public async Task<IActionResult> Details(int Student_Id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Student/{Student_Id}");

                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var data = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<StudentModel>(data);

                if (student == null)
                    return NotFound();

                return View(student);
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

            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/students");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/students/" + fileName;
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


