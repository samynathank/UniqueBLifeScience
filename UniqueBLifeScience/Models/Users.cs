using System.ComponentModel.DataAnnotations;

namespace UniqueBLifeScience.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserLevel { get; set; } = "Normal";
    }
}
