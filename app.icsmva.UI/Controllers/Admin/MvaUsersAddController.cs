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
        public MvaUsersAddController(IUsers users, IUsersRoles usersRoles, IApplicationName application, Iappusers iappusers)
        {
            this.users = users;
            this.usersRoles = usersRoles;
            this.application = application;
            this.iappusers = iappusers;
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
            var pram = ("LoginName:" + model.LoginName + ",FullName:" + model.FullName + ",ApplicationName:" + model.ApplicationName + ",LoginPWD:" + model.LoginPWD + ",RoleID:" + model.RoleID + ",Remarks:" + model.Remarks + "").ToString();
            if (res3=="successfilly")
            {
                ModelState.Clear();
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                ViewBag.rolelist = usersRoles.GetROLEs().Select(s => new { Id = s.RoleID, Name = s.RoleName, ApplicationName = s.ApplicationName });
                ViewBag.message = "successfully".ToString();

                var resd = ("Log Type:Information/r/nSource: MvaUserModify/User_Modify/r/nSQL Query:USERS_Create/r/nMessages:Information Added Successfully/r/n" + pram + "").ToString();
                Log.Information(resd);
                return View(model);
            }
            else 
            {
                ModelState.AddModelError(string.Empty, res3.ToString());
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                ViewBag.rolelist = usersRoles.GetROLEs().Select(s => new { Id = s.RoleID, Name = s.RoleName, ApplicationName = s.ApplicationName });
                var resd=("Log Type:Error/r/n/Source: MvaUsersAdd/User_Add/r/nSQL Query:USERS_Create/r/nMessages:" + res3.ToString() + "/r/n"+ pram + "").ToString();
                Log.Information(resd);
                return View(model);
            }
  
            //var res = users.GetUser(model.LoginName);
            //if (res == null)
            //{
            //    int result = users.Adduser(model);

            //    if (result == 0)
            //    {
            //        ModelState.AddModelError(string.Empty, "Entry Faild");
            //        ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
            //        ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("User_View", "MvaUsers");
            //    }  
            //}
            //else
            //{
            //    ModelState.AddModelError("LoginName", "This Login Name is  Already Exists");
            //    ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
            //    ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
            //    return View(model);
            //}
        }
    }
}
