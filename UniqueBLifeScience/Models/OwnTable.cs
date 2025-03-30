using System.ComponentModel.DataAnnotations;

namespace UniqueBLifeScience.Models
{
    public class OwnTable
    {
        [Key]
        public int UniqID { get; set; }
        public string MyCompanyName { get; set; }
        public string Address { get; set; }
        public string GSTNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string DLNumber { get; set; }
    }
}
