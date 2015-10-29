using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using TaskMgt.DataAccess;

namespace TaskMgtApi.Controllers
{
    public class ClientApiController : ApiController
    {
        private pdxtEntities _ctx;
        private HttpContext _context;

        public ClientApiController()
        {
            this._ctx = new pdxtEntities();
            this._context = HttpContext.Current;
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

        [HttpGet, HttpPost]
        [Route("api/ClientApi/Add")]
        public HttpResponseMessage Add()
        {
            string cname = _context.Request["cname"]?.ToString() ?? "";
            string pinyin = _context.Request["pinyin"]?.ToString() ?? "";
            string address = _context.Request["address"]?.ToString() ?? "";
            string contact = _context.Request["contact"]?.ToString() ?? "";
            string mobile = _context.Request["mobile"]?.ToString() ?? "";
            string tel = _context.Request["tel"]?.ToString() ?? "";
            string fax = _context.Request["fax"]?.ToString() ?? "";

            Customer model = new Customer();
            model.name = cname;
            model.pinyin = pinyin;
            model.addr = address;
            model.contact = contact;
            model.mobile = mobile;
            model.tel = tel;
            model.fax = fax;
            model.isDel = 0;

            if (string.IsNullOrWhiteSpace(cname))
            {
                _ctx.Customers.Add(model);
                _ctx.SaveChanges();
            }
            else
            {
                model.id = -1;
            }
           
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(model.id)),
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            return response;

        }

    }
}
