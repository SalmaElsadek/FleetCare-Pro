using System.ComponentModel.DataAnnotations;
using FleetCare_Pro.Validations;

namespace FleetCare_Pro.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN must be 17 characters.")]
        [ValidVIN]
        public string VIN { get; set; } //unique

        public string LicensePlate { get; set; } = default!;
        public string Make { get; set; }= default!;
        public string Model { get; set; } = default!;
        public int Year { get; set; } 
        public decimal PurchasePrice { get; set; } 
        public VehicleStatus Status { get; set; }
        public int Mileage { get; set; } 
        public string VehicleImageURL { get; set; } = default!;

        public Authentication Driver { get; set; }
        public string DriverId { get; set; }
        public ICollection<ServiceRecord> ServiceRecords { get; set; }
    }
}
