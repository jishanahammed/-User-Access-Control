using app.icsmva.DAO.dao.app.user;
using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.userRole;
using app.icsmva.DAO.dao.users;
using app.icsmva.Models;
using DocumentFormat.OpenXml.Drawing;
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
        private readonly Iappusers iappusers;
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MvaUserDeleteController(IUsers users, Iappusers iappusers, IUsersRoles usersRoles, IApplicationName application, IHttpContextAccessor _httpContextAccessor)
        {
            this.users = users;
            this.iappusers = iappusers;
            this.usersRoles = usersRoles;
            this.application = application;
            this._httpContextAccessor = _httpContextAccessor;
        }
        
        [Authorize("Authorization")]
        [HttpGet]
        public IActionResult User_Delete(int id)
        {
            var res = users.Deleteuser(id);
            //var res2 = iappusers.DeleteRecode(id);
            var LoginName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "LoginName").Value;
            var FullName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "FullName").Value;
            var pram = ("ApplicationName:" + res.ApplicationName + ",RoleID:" + res.RoleID + ",Remarks:" + res.Remarks + ",Curent_User:"+ LoginName +","+ FullName +"").ToString();
            if (res==null)
            {

                var resd2 = ("Log Type:Error,Source: MvaUserDelete/User_Delete,SQL Query:USERS_Update,Messages:Delete Not Successfully," + pram + "").ToString();
                Log.Information("\r\n" + resd2 + "\r\n");
                return Json(false);
            }
            var resd = ("Log Type:Information,Source: MvaUserDelete/User_Delete,SQL Query:USERS_Update,Messages:Delete Successfully," + pram + "").ToString();
            Log.Information("\r\n" + resd + "\r\n");
            return Json(true);
        }
    }
}
