using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHTD.DbObject
{
    public class GetSql
    {
        public static string GetTaskSql(int status, int userid, int clientid, int page, int pageSize)
        {
            StringBuilder sbSelect = new StringBuilder();

            #region Select

            sbSelect.Append(" select top [top]");
            sbSelect.Append("   t.id,");
            sbSelect.Append("   t.[status],");
            sbSelect.Append("   t.tax,");
            sbSelect.Append("   tc.requesttime,");
            sbSelect.Append("   tc.content,");
            sbSelect.Append("   tc.createtime,");
            sbSelect.Append("   tc.ismain,");
            sbSelect.Append("   tc.feiyong,");
            sbSelect.Append("   tc.type,");
            sbSelect.Append("   tc.createuser,");
            sbSelect.Append(
                "   (select top 1 name from Users where Users.isDel = 0 and Users.id = tc.createuser) createName,");
            sbSelect.Append("   c.name clientName,");
            sbSelect.Append("   c.addr clientAddress,");
            sbSelect.Append("   c.tel clientTel,");
            sbSelect.Append("   c.mobile clientMobile,");
            sbSelect.Append("   c.fax clientFax,");
            sbSelect.Append("   t.visitresult,");
            sbSelect.Append("   t.visitremark,");
            sbSelect.Append("   t.visittime,");
            sbSelect.Append("   t.visituser,");
            sbSelect.Append(
                "   (select top 1 name from Users where Users.isDel = 0 and Users.id = t.visituser) visitUserName,");
            sbSelect.Append("   u.uid,");
            sbSelect.Append("   (select top 1 name from Users where Users.isDel = 0 and Users.id = u.uid) assignedUser");
            sbSelect.Append(" from ");
            sbSelect.Append("   	Task t left join TaskContent tc on t.id = tc.tid   ");
            sbSelect.Append("       left join TaskUser u on t.id = u.tid");
            sbSelect.Append("       left join Customer c on c.id = t.cid");
            sbSelect.Append("   where 1 = 1 ");

            #endregion

            #region
            StringBuilder sbOrder = new StringBuilder();
            sbOrder.Append(" order by tc.requesttime desc ");
            #endregion

            StringBuilder sbWhere = new StringBuilder();

            #region 

            if (status > 0)
            {
                sbWhere.Append("   and t.status =" + status);
            }

            if (userid > 0 && userid != 4)
            {
                sbWhere.Append("   and u.uid =" + userid);
            }

            if (clientid > 0)
            {
                sbWhere.Append("    and c.id=" + clientid);
            }

            #endregion

            return sbSelect.ToString().Replace("[top]", (page*pageSize).ToString()) + sbWhere.ToString() + sbOrder.ToString();

        }

        public static string AssignTaskSql(string tid, string uid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("  begin tran ");
            sb.Append("     delete from TaskUser where tid ="+tid);
            sb.Append(string.Format("     insert into TaskUser(tid,uid) values({0},{1})", tid,uid));
            sb.Append(" commit ");
            return sb.ToString();
        }
    }
}
