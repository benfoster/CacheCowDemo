using CacheCow.Client;
using CacheCow.Client.SqlCacheStore;
using CacheCowDemo.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace CacheCowDemo.Client.Controllers
{
    public class ContactsController : Controller
    {
        private readonly HttpClient httpClient;

        public ContactsController()
        {
            var cachingHandler = new CachingHandler(new SqlStore()) {
                    InnerHandler = new HttpClientHandler()
            };
            
            httpClient = new HttpClient(cachingHandler)
            {
                BaseAddress = new Uri(WebConfigurationManager.AppSettings["ApiEndpoint"])
            };
        }
        
        public async Task<ActionResult> Index()
        {
            var response = await httpClient.GetAsync("contacts");
            var contacts = await response.Content.ReadAsAsync<IEnumerable<Contact>>();

            return View(contacts);
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddContactCommand cmd)
        {
            var response = await httpClient.PostAsJsonAsync("contacts", cmd);
            return RedirectToAction("index");
        }
    }
}
