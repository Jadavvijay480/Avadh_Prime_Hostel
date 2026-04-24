using System;

namespace AVADH_PRIME_Consume.Models
{
    public class FeesReceiptsModel
    {
        public int Receipt_Id { get; set; }

        public int Fees_Id { get; set; }
        public int Student_Id { get; set; }

        public decimal Paid_Amount { get; set; }
        public string Payment_Mode { get; set; }
        public string Transaction_Id { get; set; }

        public string Receipt_No { get; set; }

        public DateTime Payment_Date { get; set; }

        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}