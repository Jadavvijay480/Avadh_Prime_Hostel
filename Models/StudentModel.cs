using System.ComponentModel.DataAnnotations;

namespace AVADH_PRIME_Consume.Models
{
    public class StudentModel
    {
        public int Student_Id { get; set; }

        // Personal Information
        public string? student_image { get; set; }

        // Upload (not stored in DB)
        public IFormFile? ImageFile { get; set; }

        public IFormFile? IDProofFile { get; set; } // ✅ ADD THIS

        [Required]
        public string H_Enrollment_No { get; set; }

        [Required]
        public string First_Name { get; set; }

        [Required]
        public string Last_Name { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string BloodGroup { get; set; }

        // Contact
        [Required]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Pincode { get; set; }

        // Academic
        [Required]
        public string U_Enrollment_No { get; set; }

        [Required]
        public string Course { get; set; }

        [Required]
        public string Branch { get; set; }

        [Required]
        public int Semester { get; set; }

        [Required]
        public string CollegeName { get; set; }

        // Hostel
        [Required]
        public int Hostel_Id { get; set; }

        [Required]
        public DateTime AdmissionDate { get; set; }

        // Family
        [Required]
        public string Father_Name { get; set; }

        [Required]
        public string Mother_Name { get; set; }

        [Required]
        public string Parent_Phone { get; set; }

        [Required]
        public string Emergency_Contact { get; set; }

        // ID Proof
        [Required]
        public string ID_Proof_Type { get; set; }

        [Required]
        public string ID_Proof_No { get; set; }

        public string? ID_Proof_Path { get; set; } // ✅ NO REQUIRED

        // Status
        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string Status { get; set; }

        // Audit
        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
   
}
