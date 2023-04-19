using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.Privilege;
using app.icsmva.DAO.dao.userRole;
using app.icsmva.DAO.dao.users;
using app.icsmva.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace app.icsmva.UI.Controllers.Admin
{
    [Authorize]
    public class MvaUsersAddController : Controller
    {
        private readonly IUsers users;
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        public MvaUsersAddController(IUsers users, IUsersRoles usersRoles, IApplicationName application)
        {
            this.users = users;
            this.usersRoles = usersRoles;
            this.application = application;
        }
        [Authorize("Authorization")]
        public IActionResult User_Add()
        {
            UserViewModel user = new UserViewModel();

            ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
            ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
            return View(user);
        }
        [HttpPost]
        public IActionResult User_Add(UserViewModel model)
        {
            var res = users.GetUser(model.LoginName);
            if (res == null)
            {
                int result = users.Adduser(model);
                if (result == 0)
                {
                    ModelState.AddModelError(string.Empty, "Entry Faild");
                    ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
                    ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("User_View", "MvaUsers");
                }
            }
            else
            {
                ModelState.AddModelError("LoginName", "This Login Name is  Already Exists");
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
                ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                return View(model);
            }
        }
    }
}
