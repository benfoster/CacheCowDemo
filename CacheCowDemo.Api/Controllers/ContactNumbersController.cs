using CacheCowDemo.Api.App;
using CacheCowDemo.Common;
using System.Net;
using System.Web.Http;

namespace CacheCowDemo.Api.Controllers
{
    public class ContactNumbersController : ApiController
    {
        private readonly Db db;

        public ContactNumbersController()
        {
            db = new Db();
        }

        public void Post(int contactId, AddContactNumberCommand cmd)
        {
            var contact = GetContact(contactId);
            contact.AddNumber(new ContactNumber
            {
                NumberType = cmd.NumberType,
                Number = cmd.Number
            });
        }

        public void Delete(int contactId, int id)
        {
            var contact = GetContact(contactId);
            contact.RemoveNumber(id);
        }

        private Contact GetContact(int id)
        {
            var contact = db.GetContact(id);
            if (contact == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return contact;
        }
    }
}