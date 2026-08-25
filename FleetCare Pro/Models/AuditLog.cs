namespace FleetCare_Pro.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string ActionDetails { get; set; }
    }
}