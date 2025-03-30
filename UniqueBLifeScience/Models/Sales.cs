using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqueBLifeScience.Models
{
    public class Sales
    {
        [Key]
        public int SalesID { get; set; }
        public string CustomerName { get; set; }
        [ForeignKey("CustomerID")]
        public int CustomerID { get; set; }
        public string Address { get; set; }
        public string GSTNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string DLNumber { get; set; }
        public double? SalesGST { get; set; }
        public int? Total { get; set; }
        public int? Discount { get; set; } = 0;
        public DateTime currentDate { get; set; }
        public ICollection<SalesSub> SalesSub { get; set; }

        public string GenerateInvoiceNumber()
        {
            string customerInitials = CustomerName.Length >= 3 ? CustomerName.Substring(0, 3).ToUpper() : CustomerName.ToUpper();
            string date = DateTime.Now.ToString("ddMMyyyy");
            return $"{customerInitials}{CustomerID}{date}{SalesID}";
        }
    }
}
