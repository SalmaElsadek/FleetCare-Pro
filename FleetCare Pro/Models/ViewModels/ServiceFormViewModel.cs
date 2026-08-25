using System.ComponentModel.DataAnnotations;
using FleetCare_Pro.Models;

namespace FleetCare_Pro.Models.ViewModels
{
    public class ServiceFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a vehicle.")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Please select a service center.")]
        public int ServiceCenterId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ServiceDate { get; set; } = DateTime.Now;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Mileage must be a positive value.")]
        public int CurrentMileage { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; } = default!;
        public List<ServiceItemFormViewModel> ServiceLineItems { get; set; } = new List<ServiceItemFormViewModel>();

        [Display(Name = "Invoice Document")]
        public IFormFile? InvoiceFile { get; set; }
    }

    public class ServiceItemFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a service category.")]
        public int ServiceCategoryId { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be greater than zero.")]
        public decimal Cost { get; set; }
    }
}