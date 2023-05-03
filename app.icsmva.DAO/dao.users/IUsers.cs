using app.icsmva.Models;
using app.icsmva.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.users
{
    public interface IUsers
    {
        PagedModel<USERS> GetUserPagedListAsync(int page, int pageSize);
        USERS GetUser(string name);
        UserViewModel GetUserbyid(int userid);
        int  Adduser(UserViewModel model);
        bool  Updateuser(UserViewModel model);
        bool  Deleteuser(int id);


    }
}
