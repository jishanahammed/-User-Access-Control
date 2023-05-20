using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.userRole;
using app.icsmva.DAO.dao.users;
using app.icsmva.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace app.icsmva.UI.Controllers.Admin
{
    [Authorize]
    public class MvaUserDeleteController : Controller
    {
        private readonly IUsers users;
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        public MvaUserDeleteController(IUsers users, IUsersRoles usersRoles, IApplicationName application)
        {
            this.users = users;
            this.usersRoles = usersRoles;
            this.application = application;
        }
        
        [Authorize("Authorization")]
        [HttpGet]
        public IActionResult User_Delete(int id)
        {
            var res = users.Deleteuser(id);
            var pram = ("ApplicationName:" + res.ApplicationName + ",RoleID:" + res.RoleID + ",Remarks:" + res.Remarks + "").ToString();
            if (res==null)
            {
                return Json(false);
                var resd2 = ("Log Type:Error/r/nSource: MvaUserDelete/User_Delete/r/nSQL Query:USERS_Update/r/nMessages:Delete Not Successfully/r/n" + pram + "").ToString();
                Log.Information(resd2);
            }
            var resd = ("Log Type:Information/r/nSource: MvaUserDelete/User_Delete/r/nSQL Query:USERS_Update/r/nMessages:Delete Successfully/r/n" + pram + "").ToString();
            Log.Information(resd);
            return Json(true);
        }
    }
}
