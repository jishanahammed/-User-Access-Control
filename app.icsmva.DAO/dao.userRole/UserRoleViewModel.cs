using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.userRole
{
    public class UserRoleViewModel
    {
        public int RoleID { set; get; }
        public string RoleName { set; get; }
        public string ApplicationName { set; get; }
        public string Remarks { set; get; }
        public DateTime CreationDate { set; get; }
        public int CreatedBy { set; get; }
        public DateTime LastUpdatedDate { set; get; }
        public int LastUpdatedBy { set; get; }
        public short IsDeleted { set; get; }
        public List<RolePrivilegeViewModel> mapprivilege { set; get; }

    }
}
