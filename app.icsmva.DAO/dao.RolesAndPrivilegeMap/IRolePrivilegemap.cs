using app.icsmva.Models;
using app.icsmva.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.RolesAndPrivilegeMap
{
    public interface IRolePrivilegemap
    {
        List<RolePrivilegeViewModel> GetRoleWise(int RoleId);
        List<UserMenuPermitionVM> UserMenu(int RoleId);
        List<ActionViewModel> Rolepermition();
        RolePrivilegeViewModel Getsingle(int RPMapId);
        bool Getsingleupdate(int id, int roleId);
        RolePrivilegeViewModel Getparmition(int RoleId,string actinname,string controllrname);
    }
}
