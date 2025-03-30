using System.ComponentModel.DataAnnotations;

namespace UniqueBLifeScience.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string HSNCode { get; set; }
        public double MRP { get; set; }
        public double GST { get; set; }
        public string Batch { get; set; }
        public double Rate { get; set; }
        public int Packing { get; set; } = 1;

    }
}
