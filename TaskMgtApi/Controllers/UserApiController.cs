using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using TaskMgt.DataAccess;

namespace TaskMgtApi.Controllers
{
    public class UserApiController : ApiController
    {
        private pdxtEntities _ctx;
        public UserApiController()
        {
            this._ctx = new pdxtEntities();
        }

        [HttpPost, HttpGet]
        public HttpResponseMessage Login(string user, string pwd)
        {
            string encryptedPwd = MD5(pwd);

            var model =
                _ctx.Users.FirstOrDefault(
                    u =>
                        u.loginname.Equals(user) && u.loginpw.Equals(encryptedPwd, StringComparison.OrdinalIgnoreCase) &&
                        u.isDel == 0) ?? new User {id = -1};
            string result =  JsonConvert.SerializeObject(model);

            var response = new HttpResponseMessage
            {
                Content = new StringContent(result),
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            return response;
        }

        [HttpPost, HttpGet]
        public HttpResponseMessage GetUsers(int page=1, int pageSize=50)
        {
            if (HttpContext.Current.Request["page"] != null)
            {
                page = int.Parse(HttpContext.Current.Request["page"]);
            }

            if (HttpContext.Current.Request["pageSize"] != null)
            {
                pageSize = int.Parse(HttpContext.Current.Request["pageSize"]);
            }

            int skipRecord = (page-1) *pageSize;
            var user = this._ctx.Users.Where(u => u.isDel == 0).OrderBy(u => u.id).Skip(skipRecord).Take(pageSize);
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(user)),
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            return response;
        }

        public string MD5(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }


        [HttpPost, HttpGet]
        [Route("api/UserApi/Upload")]
        public HttpResponseMessage Upload()
        {
            string user = HttpContext.Current.Request["user"];
            var response = new HttpResponseMessage
            {
                Content = new StringContent("Yes"),
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            return response;
        }
    }
}
