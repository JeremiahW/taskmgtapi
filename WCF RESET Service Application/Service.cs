using HHTD.DbObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Windows.Forms;
using HHTD.Model;

namespace HHTD.Service
{
    [ServiceContract]
    public interface IService
    {
   
        #region 文件上传下载

        [WebInvoke(
            Method = "POST",
            UriTemplate = "/file/add",
            BodyStyle = WebMessageBodyStyle.Bare
            )]
        void UploadFile(Stream stream);

        [WebInvoke(Method = "POST", UriTemplate = "/login",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, 
            RequestFormat = WebMessageFormat.Json)]
        UserModel Login(string user, string password);

        [WebInvoke(Method = "POST", UriTemplate = "/gettask",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        List<TaskModel> GetTask(string userid, string statusid, string clientid, string page, string pagesize);

          [WebInvoke(Method = "POST", UriTemplate = "/getuser",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        List<UserModel> GetUsers();

        [WebInvoke(Method = "POST", UriTemplate = "/assigntask",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        bool AssignTask(string taskId, string userId);

        #endregion
    }


    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
                 InstanceContextMode = InstanceContextMode.PerCall,
                 IgnoreExtensionDataObject = true,
                 IncludeExceptionDetailInFaults = true)]
    public class Service : IService
    {
        private HHTD.DbObject.DbEntities db;

        public Service()
        {
            db = new DbEntities();
        }

        #region Up Down
        public void UploadFile(Stream stream)
        {
            string folder_path = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "upload");
            if (!Directory.Exists(folder_path))
                Directory.CreateDirectory(folder_path);
            string file_path = Path.Combine(folder_path,Guid.NewGuid().ToString()+".doc");

            long length = 0;
            using (FileStream writer = new FileStream(file_path, FileMode.Create))
            {
                int readCount;
                var buffer = new byte[8192];
                while ((readCount = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    writer.Write(buffer, 0, readCount);
                    length += readCount;
                }
            }

            long _len = length;

        }

        #endregion

        public UserModel Login(string user, string password)
        {
            var users = db.Users.Where(s => s.loginname.Equals(user) 
                && s.loginpw.Equals(password, StringComparison.OrdinalIgnoreCase)
                && s.isDel == 0).Select(m => new UserModel()
                {
                    DispName = m.name,
                    Username = m.loginname,
                    Password = m.loginpw,
                    Id = m.id,
                    RoleId = (int)m.type.Value
                });
            return users.ToList().Any() ? users.ToList()[0] : null;
        }

        public List<TaskModel> GetTask(string userId, string statusId, string clientId, string page, string pageSize)
        {
            string sql = GetSql.GetTaskSql(Convert.ToInt32(statusId), 
                                           Convert.ToInt32(userId), 
                                           Convert.ToInt32(clientId), 
                                           Convert.ToInt32(page), 
                                           Convert.ToInt32(pageSize));
            var dt = this.db.Database.SqlQueryForDataTatable(sql);
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

                models.Add(model);
            }

            return models;
        }

        public List<UserModel> GetUsers()
        {
            var users = db.Users.Where(s => s.isDel == 0);
            var models = new List<UserModel>();
            users.ToList().ForEach((s) =>
            {
                UserModel m = new UserModel();
                m.Id = s.id;
                m.DispName = s.name;
                m.Username = s.loginname;
                m.Password = s.loginpw;
                m.RoleId = Convert.ToInt32(s.type);
                models.Add(m);
            });
            return models;
        }

        public bool AssignTask(string taskId, string userId)
        {
            int rowsAffected = -1;
            rowsAffected = this.db.Database.ExecuteSqlCommand(GetSql.AssignTaskSql(taskId, userId));
            if(rowsAffected > 0)
                return true;
            else
                return false;
        }
    }
}
