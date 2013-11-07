using System.ComponentModel.DataAnnotations;

namespace CacheCowDemo.Common
{
    public class AddContactCommand
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
    }
}