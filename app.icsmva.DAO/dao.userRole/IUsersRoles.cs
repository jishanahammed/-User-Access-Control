using app.icsmva.Models;
using app.icsmva.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.userRole
{
    public interface IUsersRoles
    {
        PagedModel<ROLES> GetRolesPagedListAsync(int page, int pageSize);
        ROLES GetRole(int id);
    }
}
