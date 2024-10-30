using System;
using System.ComponentModel.DataAnnotations;

namespace ReciclagemApi.Models
{
    public class RecyclingReportModel
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Material { get; set; }

        [Required]
        public int Quantity { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
