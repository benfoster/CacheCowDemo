using System.ComponentModel.DataAnnotations;

namespace CacheCowDemo.Common
{
    public class AddContactNumberCommand
    {
        [Required]
        public string NumberType { get; set; }
    
        [Required]
        public string Number { get; set; }
    }
}
