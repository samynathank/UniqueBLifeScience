using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqueBLifeScience.Models
{
    public class Stocks
    {
        [Key]
        public int StockID { get; set; }
        public string? CompanyName { get; set; }
        public string? GSTNumber { get; set; }
        public double? PackingGST { get; set; }
        public string? PurchaseBill { get; set; }
        public int? Total { get; set; }
        public ICollection<StockSub> StockSub { get; set; }
    }
}
