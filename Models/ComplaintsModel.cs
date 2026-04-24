using System.ComponentModel.DataAnnotations;

namespace AVADH_PRIME_Consume.Models
{
    public class ComplaintsModel
    {
        public int Complaint_Id { get; set; }

        // Foreign Key
        [Required]
        public int Student_Id { get; set; }

        // Complaint Details
        [Required]
        public string Complaint_Title { get; set; }

        [Required]
        public string Complaint_Description { get; set; }

        [Required]
        public string Complaint_Type { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public string Status { get; set; }

        public int? Assigned_To { get; set; }

        [Required]
        public DateTime Complaint_Date { get; set; }

        public DateTime? Resolved_Date { get; set; }

        public string? Resolution_Remarks { get; set; }
        public string? Attachment_Path { get; set; }
        public IFormFile? AttachmentFile { get; set; }

        public string? Feedback { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int? Rating { get; set; }

        // Audit Fields
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
