using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HHTD.Model
{
    [DataContract]
    public class UserModel
    {
        [DataMember]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [DataMember]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [DataMember]
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        [DataMember]
        public int RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }

        [DataMember]
        public string DispName
        {
            get { return _dispName; }
            set { _dispName = value; }
        }

        private int _id;
        private string _password;
        private string _username;
        private int _roleId;
        private string _dispName;
    }
}
