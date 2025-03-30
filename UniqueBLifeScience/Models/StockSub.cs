using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniqueBLifeScience.Models
{
    public class StockSub
    {
        [Key]
        public int? StockSubID { get; set; }
        [ForeignKey("StockID")]
        public int StockID { get; set; }
        public Stocks Stock { get; set; }
        public string ProductName { get; set; }
        [ForeignKey("ProductID")]
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string HSNCode { get; set; }
        public string Batch { get; set; }
        public double MRP { get; set; }
        public double Rate { get; set; }
        public double GST { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Packing { get; set; } = 1;

    }
}
