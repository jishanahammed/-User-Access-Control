using app.icsmva.Models;
using app.icsmva.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.Privilege
{
    public interface IPrivilege
    {
        PagedModel<PRIVILEGES> GetRolesPagedListAsync(int page, int pageSize);
    }
}
