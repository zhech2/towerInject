using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Name
    {
        [Required]
        public string First { get; set; }
        public string Middle { get; set; }
        [Required]
        public string Last { get; set; }
    }
}