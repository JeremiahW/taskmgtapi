using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using TaskMgt.DataAccess;
using System.Data;
using System.Security.Cryptography.Xml;
using System.Transactions;
using System.Web;

namespace TaskMgtApi.Controllers
{
    public class TaskApiController : ApiController
    {
        private pdxtEntities _ctx;

        public TaskApiController()
        {
            this._ctx = new pdxtEntities();
        }


        [HttpPost, HttpGet]
        public HttpResponseMessage GetTasks(int uid=-1,  int statusid=-1, int cid=-1, int page=1, int pageSize=50)
        {
            uid = HttpContext.Current.Request["uid"] == null ? uid : int.Parse(HttpContext.Current.Request["uid"]);
            statusid = HttpContext.Current.Request["statusid"] == null ? uid : int.Parse(HttpContext.Current.Request["statusid"]);
            cid = HttpContext.Current.Request["cid"] == null ? uid : int.Parse(HttpContext.Current.Request["cid"]);
            page = HttpContext.Current.Request["page"] == null ? uid : int.Parse(HttpContext.Current.Request["page"]);
            pageSize = HttpContext.Current.Request["pageSize"] == null ? uid : int.Parse(HttpContext.Current.Request["pageSize"]);

            string sql = GetSql.GetTaskSql(statusid, uid, cid, page, pageSize);
            var dt = _ctx.Database.SqlQueryForDataTatable(sql);

            List<TaskModel> models = new List<TaskModel>();
            foreach (DataRow item in dt.Rows)
            {
                TaskModel model = new TaskModel();
                model.TaskId = int.Parse(item["id"].ToString());
                model.StatusId = item["status"].ToString() != "" ? int.Parse(item["status"].ToString()) : -1;
                model.Tax = item["tax"].ToString() != "" ? int.Parse(item["tax"].ToString()) : -1;
                model.RequestTime = item["requesttime"].ToString() != ""
                    ? item.Field<DateTime>("requesttime")
                    : DateTime.MaxValue;
                model.Content = item.Field<string>("content");
                model.CreatedTime = item["createtime"].ToString() != ""
                    ? item.Field<DateTime>("createtime")
                    : DateTime.MaxValue;
                model.IsMain = item["ismain"].ToString() != "" ? int.Parse(item["ismain"].ToString()) : -1;
                model.Fee = item["feiyong"].ToString() != "" ? item.Field<decimal>("feiyong") : -1;
                model.Type = item["type"].ToString();
                model.CreatedUser = item["createuser"].ToString() != "" ? int.Parse(item["createuser"].ToString()) : -1;
                model.CreatedUserName = item["createName"].ToString();
                model.ClientName = item["clientName"].ToString();
                model.ClientAddress = item["clientAddress"].ToString();
                model.ClientTel = item["clientTel"].ToString();
                model.ClientMobile = item["clientMobile"].ToString();
                model.ClientFax = item["clientFax"].ToString();
                model.VisitResult = item["visitresult"].ToString();
                model.VisitRemark = item["visitremark"].ToString();
                model.VisitTime = item["visittime"].ToString() != ""
                    ? item.Field<DateTime>("visittime")
                    : DateTime.MaxValue;
                model.VisitUser = item["visituser"].ToString() != "" ? int.Parse(item["visituser"].ToString()) : -1;
                model.VisitUserName = item["visitUserName"].ToString();
                model.UId = item["uid"].ToString() != "" ? int.Parse(item["uid"].ToString()) : -1;
                model.UName = item["assignedUser"].ToString();

                model.Contact = item["contact"].ToString();
                
                models.Add(model);
            }

          
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(models)),
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            return response;
        }

        [HttpPost, HttpGet]
        [Route("api/TaskApi/SetTask")]
        public HttpResponseMessage SetTask()
        {
            int uid = -1;
            int taskid = -1;
            uid = HttpContext.Current.Request["uid"] == null ? uid : int.Parse(HttpContext.Current.Request["uid"]);
            taskid = HttpContext.Current.Request["taskid"] == null
                ? uid
                : int.Parse(HttpContext.Current.Request["taskid"]);

            int rowsAffected =
                this._ctx.Database.ExecuteSqlCommand(GetSql.AssignTaskSql(taskid.ToString(), uid.ToString()));
            var response = new HttpResponseMessage
            {
                Content = new StringContent("AssignSuccessful"),
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            return response;
        }

        [HttpPost, HttpGet]
        [Route("api/TaskApi/Add")]
        public HttpResponseMessage Add()
        {
            int uid = HttpContext.Current.Request["uid"] == null ? -1 : int.Parse(HttpContext.Current.Request["uid"]);
            int assignuid = HttpContext.Current.Request["assignuserid"] == null ? -1 : int.Parse(HttpContext.Current.Request["assignuserid"]);
            int cid = HttpContext.Current.Request["clientid"] == null ? -1 : int.Parse(HttpContext.Current.Request["clientid"]);
            int taskid = -1; //auto increase column
            int serviceid = HttpContext.Current.Request["servicetypeid"] == null ? -1 : int.Parse(HttpContext.Current.Request["servicetypeid"]);
            string content = HttpContext.Current.Request["content"]; 
            float fee = HttpContext.Current.Request["fee"] == null ? -1 : int.Parse(HttpContext.Current.Request["fee"]);
            int intax = HttpContext.Current.Request["intax"] == null ? -1 : int.Parse(HttpContext.Current.Request["intax"]);
            DateTime requestDatetime = HttpContext.Current.Request["requestdate"] == null ? DateTime.MaxValue : DateTime.Parse(HttpContext.Current.Request["requestdate"]);

            Task task = new Task();
            task.cid = cid;
            task.tax = (short)intax;
            task.uid = assignuid;
            task.isDel = 0;
            task.status = 1;
            task.sn = "FWDJ" + DateTime.Now.ToString("yyyyMMdd") +
                      new Random().Next(1000, 9999).ToString();
            using (TransactionScope scope = new TransactionScope())
            {
                _ctx.Tasks.Add(task);
                _ctx.SaveChanges();
                taskid = task.id;

                TaskContent contentModel = new TaskContent();
                contentModel.tid = taskid;
                contentModel.type = serviceid;
                contentModel.feiyong = (decimal)fee;
                contentModel.requesttime = requestDatetime;
                contentModel.content = content;
                contentModel.createtime = DateTime.Now;
                contentModel.createuser = uid;
                contentModel.ismain = 1;
                

                _ctx.TaskContents.Add(contentModel);
                _ctx.SaveChanges();

                this._ctx.Database.ExecuteSqlCommand(GetSql.AssignTaskSql(taskid.ToString(), assignuid.ToString()));
                scope.Complete();
            }
     

            var response = new HttpResponseMessage
            {
                Content = new StringContent("AddSuccessful"),
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            return response;
        }
    }
}
