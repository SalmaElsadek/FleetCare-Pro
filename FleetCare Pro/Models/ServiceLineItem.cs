using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetCare_Pro.Models
{
    public class ServiceLineItem
    {
        public int Id { get; set; }

        public int ServiceRecordId { get; set; }
        public ServiceRecord? ServiceRecord { get; set; }

        [Required(ErrorMessage = "Please select a service category.")]
        public int ServiceCategoryId { get; set; }
        public ServiceCategory? ServiceCategory { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be greater than zero.")]
        public decimal Cost { get; set; }
    }
}