using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using TaskMgt.DataAccess;

namespace TaskMgtApi.Controllers
{
    public class ClientApiController : ApiController
    {
        private pdxtEntities _ctx;

        public ClientApiController()
        {
            this._ctx = new pdxtEntities();
        }

        [AcceptVerbs(WebRequestMethods.Http.Get, WebRequestMethods.Http.Post)]
        public HttpResponseMessage GetClients(string cname="", int page=1, int pageSize=50)
        {
            int skipRecord = (page-1) * pageSize;

            List<Customer> clients;
            if (string.IsNullOrEmpty(cname))
            {
                clients = this._ctx.Customers.ToList();
            }
            else
            {
                clients = this._ctx.Customers.Where(c => c.name.Contains(cname)).ToList();
            }

            clients = clients.Where(c => c.isDel == 0).OrderBy(c => c.id).Skip(skipRecord).Take(pageSize).ToList();
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(clients)),
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            return response;
        }

  
    }
}
