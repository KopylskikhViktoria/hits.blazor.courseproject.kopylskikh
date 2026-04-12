namespace InventoryControl.Data.Models
{
    public class SalesReportRow
    {
        public string ProductName { get; set; } = "";
        public int TotalQuantity { get; set; }
        public decimal TotalSum { get; set; }
    }
}