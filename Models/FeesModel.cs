using System.ComponentModel.DataAnnotations;

namespace AVADH_PRIME_Consume.Models
{
    public class FeesModel
    {
        public int Fees_Id { get; set; }

        // Foreign Key
        [Required]
        public int Student_Id { get; set; }

        // Fee Details
        [Required]
        public string Fee_Type { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be positive")]
        public decimal Total_Amount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Paid_Amount { get; set; }

        public decimal Due_Amount { get; set; } // Computed (Total - Paid)

        [Required]
        public string Payment_Status { get; set; }

        public string? Payment_Mode { get; set; }
        public string? Transaction_Id { get; set; }

        // Dates
        [Required]
        public DateTime Fee_Date { get; set; }

        [Required]
        public DateTime Due_Date { get; set; }

        public DateTime? Payment_Date { get; set; }

        // Academic Info
        [Required]
        public int Semester { get; set; }

        [Required]
        public int Year { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Late_Fine { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Audit Fields
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
