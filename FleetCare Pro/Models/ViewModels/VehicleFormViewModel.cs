using System.ComponentModel.DataAnnotations;
using FleetCare_Pro.Validations;
using Microsoft.AspNetCore.Http; 
using FleetCare_Pro.Models;   

namespace FleetCare_Pro.Models.ViewModels
{
    public class VehicleFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "VIN is required.")]
        [ValidVIN]
        public string VIN { get; set; } = default!;

        [Required(ErrorMessage = "License Plate is required.")]
        public string LicensePlate { get; set; }=default!;

        [Required(ErrorMessage = "Make is required.")]
        public string Make { get; set; } = default!;

        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; } = default!;

        [Required(ErrorMessage = "Year is required.")]
        [Range(1990, 2030, ErrorMessage = "Please enter a valid year.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Purchase Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public VehicleStatus Status { get; set; }

        [Required(ErrorMessage = "Mileage is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Mileage cannot be negative.")]
        public int Mileage { get; set; }

        public IFormFile? VehicleImage { get; set; }

        // Old ImageUrl for any edits
        public string? ExistingImageURL { get; set; } = default!;

        [Required(ErrorMessage = "Driver must be assigned.")]
        public string DriverId { get; set; } = default!;
    }
}
