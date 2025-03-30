using System.ComponentModel.DataAnnotations.Schema;

namespace UniqueBLifeScience.Models
{
    public class PrintBillViewModel
    {
        public Sales Sales { get; set; }
        public OwnTable CompanyData { get; set; }
        public List<string> CompanyNames { get; set; }

    }
}
