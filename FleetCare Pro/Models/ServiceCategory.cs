namespace FleetCare_Pro.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = default!;
        public string Description { get; set; } =default!;
        public int RecommendedIntervalMonths { get; set; } 
        public ICollection<VendorService> VendorServices { get; set; }

        public ICollection<ServiceLineItem> ServiceLineItems { get; set; }
    }
}
