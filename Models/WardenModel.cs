using System.ComponentModel.DataAnnotations;

namespace AVADH_PRIME_Consume.Models
{
    public class WardenModel
    {
        public int Warden_Id { get; set; }

        // Basic Info
        public string? Warden_Image { get; set; }

        // for upload only (NOT stored in DB)
        public IFormFile? ImageFile { get; set; }

        [Required]
        public string Warden_Code { get; set; }

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

        // Contact Info
        [Required]
        [Phone]
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

        // Professional Info
        [Required]
        public string Qualification { get; set; }

        [Required]
        [Range(0, 50)]
        public int Experience_Years { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

        // Hostel Mapping
        [Required]
        public int Hostel_Id { get; set; }

        [Required]
        public string Emergency_Contact { get; set; }

        // ID Proof
        [Required]
        public string ID_Proof_Type { get; set; }

        [Required]
        public string ID_Proof_No { get; set; }

       
        public string? ID_Proof_Path { get; set; }

        public IFormFile? IDProofFile { get; set; } // ✅ ADD THIS

        // Status
        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string Status { get; set; }

        // Audit Fields
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
