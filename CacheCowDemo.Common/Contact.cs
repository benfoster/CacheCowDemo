using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CacheCowDemo.Common
{
    public class Contact
    {       
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int LastNumberId { get; private set; }

        public Contact()
        {
            ContactNumbers = new List<ContactNumber>();
        }

        [Display(Name = "Contact Numbers")]
        public ICollection<ContactNumber> ContactNumbers { get; set; }

        public void AddNumber(ContactNumber number)
        {
            number.Id = GetNextNumberId();
            ContactNumbers.Add(number);
        }

        public void RemoveNumber(int id)
        {
            var number = ContactNumbers.FirstOrDefault(n => n.Id == id);

            if (number != null)
            {
                ContactNumbers.Remove(number);
            }
        }

        private int GetNextNumberId()
        {
            return ++LastNumberId;
        }
    }
}