using System.ComponentModel.DataAnnotations;

namespace UniqueBLifeScience.Models
{
    public class Customers
    {
        [Key]
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string GSTNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string DLNumber { get; set; }
    }
}
