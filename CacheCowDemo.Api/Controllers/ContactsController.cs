using CacheCowDemo.Api.App;
using CacheCowDemo.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Linq;

namespace CacheCowDemo.Api.Controllers
{
    public class ContactsController : ApiController
    {
        private readonly Db db;

        public ContactsController()
        {
            db = new Db();
        }
        
        public IEnumerable<Contact> Get(int page = 1, int pageSize = 10)
        {
            return db.GetContacts(query => query.Skip((page - 1) * pageSize).Take(pageSize));
        }

        public HttpResponseMessage Get(int id)
        {
            return Request.CreateResponse<Contact>(HttpStatusCode.OK, GetContact(id));
        }

        public HttpResponseMessage Post(AddContactCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            
            var contact = new Contact { FirstName = cmd.FirstName, LastName = cmd.LastName };
            db.Store(contact);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public void Patch(int id, PatchContactCommand cmd)
        {
            PatchEntity(cmd, GetContact(id));
        }

        public void Delete(int id)
        {
            db.Delete(id);
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

        // private helpers
        private static ConcurrentDictionary<Type, PropertyInfo[]> TypePropertiesCache =
            new ConcurrentDictionary<Type, PropertyInfo[]>();

        private void PatchEntity<TPatch, TEntity>(TPatch patch, TEntity entity)
            where TPatch : class
            where TEntity : class
        {
            PropertyInfo[] properties = TypePropertiesCache.GetOrAdd(
                patch.GetType(),
                (type) => type.GetProperties(BindingFlags.Instance | BindingFlags.Public));

            foreach (PropertyInfo prop in properties)
            {
                PropertyInfo orjProp = entity.GetType().GetProperty(prop.Name);
                object value = prop.GetValue(patch);
                if (value != null)
                {
                    orjProp.SetValue(entity, value);
                }
            }
        }
    }
}
