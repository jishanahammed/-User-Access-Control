using app.icsmva.DAO.dao.userRole;
using app.icsmva.DAO.dao.users;
using app.icsmva.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.app.Role
{
    public interface IAppRole
    {
        public List<ROLES> GetUsersRolesList();
        public string AddRecode(UserRoleViewModel model);
        public ROLES DeleteRecode(int id);
        public string UpdateRecode(UserRoleViewModel model);
    }
}
