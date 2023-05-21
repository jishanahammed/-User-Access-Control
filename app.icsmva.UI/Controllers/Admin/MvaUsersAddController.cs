using app.icsmva.DAO.dao.app.user;
using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.Privilege;
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
    public class MvaUsersAddController : Controller
    {
        private readonly IUsers users;
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        private readonly Iappusers iappusers;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MvaUsersAddController(IUsers users, IUsersRoles usersRoles, IApplicationName application, Iappusers iappusers, IHttpContextAccessor httpContextAccessor)
        {
            this.users = users;
            this.usersRoles = usersRoles;
            this.application = application;
            this.iappusers = iappusers;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize("Authorization")]
        public IActionResult User_Add()
        {
            UserViewModel user = new UserViewModel();

            ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
            ViewBag.rolelist = usersRoles.GetROLEs().Select(s => new { Id = s.RoleID, Name = s.RoleName, ApplicationName = s.ApplicationName });
            return View(user);
        }
        [HttpPost]
        public IActionResult User_Add(UserViewModel model)
        {
            var res3 = iappusers.AddRecode(model);
            var LoginName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "LoginName").Value;
            var FullName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "FullName").Value;
            var pram = ("LoginName:" + model.LoginName + ",FullName:" + model.FullName + ",ApplicationName:" + model.ApplicationName + ",LoginPWD:" + model.LoginPWD + ",RoleID:" + model.RoleID + ",Remarks:" + model.Remarks + ",Curent_User:"+ LoginName +","+ FullName +"").ToString();
            if (res3=="successfilly")
            {
                ModelState.Clear();
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                ViewBag.rolelist = usersRoles.GetROLEs().Select(s => new { Id = s.RoleID, Name = s.RoleName, ApplicationName = s.ApplicationName });
                ViewBag.message = "successfully".ToString();

                var resd = ("Log Type:Information,Source: MvaUserModify/User_Modify,SQL Query:USERS_Create,Messages:Information Added Successfully," + pram + "").ToString();
                Log.Information("\r\n" + resd + "\r\n");
                return View(model);
            }
            else 
            {
                ModelState.AddModelError(string.Empty, res3.ToString());
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                ViewBag.rolelist = usersRoles.GetROLEs().Select(s => new { Id = s.RoleID, Name = s.RoleName, ApplicationName = s.ApplicationName });
                var resd=("Log Type:Error,Source: MvaUsersAdd/User_Add,SQL Query:USERS_Create,Messages:" + res3.ToString() + ","+ pram + "").ToString();
                Log.Information("\r\n" + resd + "\r\n");
                return View(model);
            }
  
           
        }
    }
}
