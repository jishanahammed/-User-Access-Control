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
            ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
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
                ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                ViewBag.message = "successfully".ToString();

                var resd = ("Information, Execution Time:" + DateTime.UtcNow + ", Source: MvaUserModify/User_Modify, Messages:Information Added Successfully, "+ pram + "").ToString();
                Log.Information(resd);
                return View(model);
            }
            else 
            {
                ModelState.AddModelError(string.Empty, res3.ToString());
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                var resd=("Error, Execution Time:" + DateTime.UtcNow + ", Source: MvaUsersAdd/User_Add, Messages:" + res3.ToString() + ","+ pram + "").ToString();
                Log.Error(resd);
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
