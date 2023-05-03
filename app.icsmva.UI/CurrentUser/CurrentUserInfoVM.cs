using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
using app.icsmva.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.UI.CurrentUser
{
    public class CurrentUserInfoVM
    {
        public string LoginName { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public bool IsRoleActive { get; set; } = true;
        public List<UserMenuPermitionVM>  MenuPermition { get; set; }   
    }
}

