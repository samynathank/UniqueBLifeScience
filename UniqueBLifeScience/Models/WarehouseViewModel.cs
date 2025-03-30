namespace UniqueBLifeScience.Models
{
    public class WarehouseViewModel
    {
        public string ProductName { get; set; }
        public string HSNCode { get; set; }
        public string Batch { get; set; }
        public double MRP { get; set; }
        public double Rate { get; set; }
        public int StockQuantity { get; set; }
        public int SalesQuantity { get; set; }
        public int AvailableQuantity => StockQuantity - SalesQuantity;
    }
}
