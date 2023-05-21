using app.icsmva.DAO.dao.app.user;
using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.userRole;
using app.icsmva.DAO.dao.users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace app.icsmva.UI.Controllers.Admin
{
    [Authorize]
    public class MvaUserModifyController : Controller
    {
        private readonly IUsers users;
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        private readonly Iappusers iappusers;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MvaUserModifyController(IUsers users, IUsersRoles usersRoles, IApplicationName application, Iappusers iappusers, IHttpContextAccessor _httpContextAccessor)
        {
            this.users = users;
            this.usersRoles = usersRoles;
            this.application = application;
            this.iappusers = iappusers;
            this._httpContextAccessor = _httpContextAccessor;
        }
        [Authorize("Authorization")]
        public IActionResult User_Modify(int userId)
        {
            UserViewModel user = users.GetUserbyid(userId);
            ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
            ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
            return View(user);
        }

        [HttpPost]
        public IActionResult User_Modify(UserViewModel model)
        {
            var res3 = iappusers.UpdateRecode(model);
            var LoginName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "LoginName").Value;
            var FullName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "FullName").Value;
  
            var pram = ("ApplicationName:" + model.ApplicationName + ",RoleID:" + model.RoleID + ",Remarks:" + model.Remarks + ",Curent_User:"+ LoginName + ","+FullName+"").ToString();
            if (res3 == "successfilly")
            {
                ModelState.Clear();
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                ViewBag.message = "successfully updated".ToString();
               
                var resd = ("Log Type:Information,Source: MvaUserModify/User_Modify,SQL Query:USERS_Update,Messages:Information Updated Successfully," + pram + "").ToString();
                Log.Information("\r\n" + resd + "\r\n");
                return View(model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, res3.ToString());
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                var resd = ("Log Type:Warning,Source:MvaUserModify/User_Modify,SQL Query:USERS_Update,Messages:" + res3.ToString() + "," + pram + "").ToString();
                Log.Warning("\r\n" + resd + "\r\n");
                return View(model);
            }
        }

    }
}
