using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniqueBLifeScience.Models
{
    public class SalesSub
    {
        [Key]
        public int SalesSubID { get; set; }
        [ForeignKey("SalesID")]
        public int SalesID { get; set; }
        public Sales Sales { get; set; }
        public string ProductName { get; set; }
        [ForeignKey("ProductID")]
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string HSNCode { get; set; }
        public string Batch { get; set; }
        public double MRP { get; set; }
        public double Rate { get; set; }
        public double GST { get; set; }
        public DateTime SalesDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Packing { get; set; } = 1;
    }
}
