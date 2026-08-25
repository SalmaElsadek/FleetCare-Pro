namespace FleetCare_Pro.Models
{
    public class ServiceCenter
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Address { get; set; } = default!;
        public bool IsActive { get; set; }
        public ICollection<ServiceRecord> ServiceRecords { get; set; }
        public ICollection<VendorService> VendorServices { get; set; }
    }
}