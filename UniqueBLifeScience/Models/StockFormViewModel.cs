namespace UniqueBLifeScience.Models
{
    public class StockFormViewModel
    {
        public string CompanyName { get; set; }
        public string GSTNumber { get; set; }
        public IFormFile PurchaseBill { get; set; }
        public List<StockSubViewModel> StockSub { get; set; } = new List<StockSubViewModel>();
        public float? PackingGST { get; set; }
        public int? Total { get; set; }
        public int StockID { get; set; }

    }

    public class StockSubViewModel
    {
        public int StockID { get; set; }
        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public string HSNCode { get; set; }
        public string Batch { get; set; }
        public float MRP { get; set; }
        public float Rate { get; set; }
        public float GST { get; set; }
        public int Quantity { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int StockSubID { get; internal set; }
        public int Packing { get; set; }
    }
}
