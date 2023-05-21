
using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.Privilege;
using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
using app.icsmva.DAO.dao.userRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace app.icsmva.UI.Controllers.Admin
{
    public class UserRoleDeleteController : Controller
    {
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        private readonly IRolePrivilegemap privilegemap;
        private readonly IPrivilege privilege;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRoleDeleteController(IUsersRoles usersRoles, IApplicationName application, IRolePrivilegemap privilegemap, IPrivilege privilege, IHttpContextAccessor httpContextAccessor)
        {
            this.usersRoles = usersRoles;
            this.application = application;
            this.privilegemap = privilegemap;
            this.privilege = privilege;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize("Authorization")]
        public IActionResult Role_Delete(int id)
        {
            var res = usersRoles.Deleterole(id);
            var LoginName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "LoginName").Value;
            var FullName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "FullName").Value;
            var pram = ("RoleName:" + res.RoleName + ",ApplicationName:" + res.ApplicationName + ",Remarks:" + res.Remarks + ",Curent_User:" + LoginName + "," + FullName + "").ToString();
            if (res.RoleID>0)
            {
                var resd = ("Log Type:Information,Source: UserRoleDelete/Role_Delete,Messages:Role Deleted Successfully," + pram + "").ToString();
                Log.Information("\r\n" + resd + "\r\n");
                return Json(res.RoleID);
            }
            var resd2 = ("Log Type:Information,Source: UserRoleDelete/Role_Delete,Messages:Role Delete Not Successfully," + pram + "").ToString();
            Log.Information("\r\n" + resd2 + "\r\n");
            return Json(0);
        }

       
    }
}
