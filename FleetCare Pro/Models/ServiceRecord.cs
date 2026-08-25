using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetCare_Pro.Models
{
    public class ServiceRecord
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a vehicle.")]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required(ErrorMessage = "Please select a service center.")]
        public int ServiceCenterId { get; set; }
        public ServiceCenter? ServiceCenter { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ServiceDate { get; set; } = DateTime.Now;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Mileage must be a positive value.")]
        public int CurrentMileage { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total cost must be greater than zero.")]
        public decimal TotalCost { get; set; }

        [StringLength(255)]
        public string? InvoiceDocumentPath { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        public ServiceRecordStatus Status { get; set; }

        [Required]
        public string CreatedByUserId { get; set; } = default!;
        public ICollection<ServiceLineItem> ServiceLineItems { get; set; } = new List<ServiceLineItem>();
    }
}