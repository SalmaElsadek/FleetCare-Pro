using Microsoft.AspNetCore.Identity;

namespace FleetCare_Pro.Models
{
    public class Authentication: IdentityUser
    {
        public string FullName { get; set; }
        public string EmployeeId { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
