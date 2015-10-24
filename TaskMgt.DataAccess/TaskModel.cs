using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaskMgt.DataAccess
{
    [DataContract, Serializable]
    public class TaskModel
    {
        [DataMember]
        public int TaskId { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public int Tax { get; set; }

        [DataMember]
        public DateTime RequestTime { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string Contact { get; set; }

        [DataMember]
        public DateTime CreatedTime { get; set; }

        [DataMember]
        public int IsMain { get; set; }

        [DataMember]
        public decimal Fee { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int CreatedUser { get; set; }

        [DataMember]
        public string CreatedUserName { get; set; }

        [DataMember]
        public string ClientName { get; set; }

        [DataMember]
        public string ClientAddress { get; set; }

        [DataMember]
        public string ClientTel { get; set; }

        [DataMember]
        public string ClientMobile { get; set; }

        [DataMember]

        public string ClientFax { get; set; }

        [DataMember]
        public string VisitResult { get; set; }

        [DataMember]
        public string VisitRemark { get; set; }

        [DataMember]
        public DateTime VisitTime { get; set; }

        [DataMember]
        public int VisitUser { get; set; }

        [DataMember]
        public string VisitUserName { get; set; }

        [DataMember]
        public int UId { get; set; }

        [DataMember]
        public string UName { get; set; }
    }
}
