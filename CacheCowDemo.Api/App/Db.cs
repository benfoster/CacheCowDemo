using CacheCowDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CacheCowDemo.Api.App
{
    public class Db
    {
        private static readonly ICollection<Contact> contacts = new List<Contact>();

        public IEnumerable<Contact> GetContacts(Func<IEnumerable<Contact>, IEnumerable<Contact>> query = null)
        {
            if (query != null)
            {
                return query(contacts);
            }
            
            return contacts;
        }

        public Contact GetContact(int id)
        {
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        public void Store(Contact contact)
        {
            contact.Id = GetNextId();
            contacts.Add(contact);
        }

        public void Delete(int id)
        {
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                contacts.Remove(contact);
            }
        }

        private int GetNextId()
        {
            return contacts.Any() ? (contacts.Max(c => c.Id) + 1) : 1;
        }
    }
}