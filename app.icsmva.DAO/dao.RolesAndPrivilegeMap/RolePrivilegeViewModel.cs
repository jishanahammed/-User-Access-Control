using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.RolesAndPrivilegeMap
{
    public class RolePrivilegeViewModel
    {
        public int RPMapId { get; set; }
        public int RoleID { set; get; }
        public string PrivilegeID { set; get; }
        public string RoleName { set; get; }
        public string UIName { set; get; }
        public string ActionName { set; get; }
        public short ActionPrecedence { set; get; }
        public string PrivilegeName { set; get; }
        public bool IsAssign { set; get; }
    }
}

