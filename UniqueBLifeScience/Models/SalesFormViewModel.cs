using System.ComponentModel.DataAnnotations.Schema;

namespace UniqueBLifeScience.Models
{
    public class SalesFormViewModel
    {
        public int SalesID { get; set; }
        public string CustomerName { get; set; }
        public int CustomerID { get; set; }
        public string Address { get; set; }
        public string GSTNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string DLNumber { get; set; }
        public double? SalesGST { get; set; }           
        public int? Total { get; set; }
        public int? Discount { get; set; } = 0;
        //public IEnumerable<SalesSub> SalesSub { get; internal set; }
        public List<SalesSubViewModel> SalesSub { get; set; } = new List<SalesSubViewModel>();

    }

    public class SalesSubViewModel
    {
        public int SalesSubID { get; set; }
        public int SalesID { get; set; }
        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string HSNCode { get; set; }
        public string Batch { get; set; }
        public double MRP { get; set; }
        public double Rate { get; set; }
        public double GST { get; set; }
        public DateTime SalesDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Packing { get; set; }
    }
}
