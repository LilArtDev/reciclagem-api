using System.ComponentModel.DataAnnotations;

namespace ReciclagemApi.Models
{
    public class MaterialModel
    {
        [Key]
        public int MaterialId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }
    }
}
