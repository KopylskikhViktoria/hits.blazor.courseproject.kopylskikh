namespace InventoryControl.Data
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; } = "";
        public string Entity { get; set; } = "";
        public DateTime Date { get; set; }
        public string? User { get; set; }
    }

}
