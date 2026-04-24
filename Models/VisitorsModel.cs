using System.ComponentModel.DataAnnotations;

namespace AVADH_PRIME_Consume.Models
{
    public class VisitorsModel
    {
        public int Visitor_Id { get; set; }

        // Personal Info
        [Required]
        public string First_Name { get; set; }

        [Required]
        public string Last_Name { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // ID Proof
        public string? Id_Proof_Type { get; set; }
        public string? Id_Proof_Number { get; set; }

        // Address
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        [Required]
        public string Country { get; set; }

        // Visit Details
        [Required]
        public DateTime Visit_Date { get; set; }

        [Required]
        public DateTime Check_In_Time { get; set; }

        public DateTime? Check_Out_Time { get; set; }

        public string? Purpose_Of_Visit { get; set; }

        public int? Person_To_Meet_Id { get; set; }
        public string? Person_Type { get; set; }

        public string? Vehicle_Number { get; set; }

        // Approval
        public bool Is_Approved { get; set; }
        public int? Approved_By { get; set; }

        public string? Remarks { get; set; }

        // Audit Fields
        public DateTime Created_At { get; set; }
        public int? Created_By { get; set; }

        public DateTime? Updated_At { get; set; }
        public int? Updated_By { get; set; }

        public bool Is_Deleted { get; set; }
    }
}
