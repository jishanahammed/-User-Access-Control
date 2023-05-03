using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.userRole;
using app.icsmva.DAO.dao.users;
using app.icsmva.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        //public IActionResult User_Delete(int userId)
        //{
        //    UserViewModel user = users.GetUserbyid(userId);
        //    ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
        //    ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");

        //    return View(user);
        //}
        //[HttpPost]
        //public IActionResult User_Delete(UserViewModel model)
        //{
        //    bool res=users.Deleteuser(model);
        //    if (res)
        //    {
        //        return RedirectToAction("User_View", "MvaUsers");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Entry Faild");
        //        return View(model);
        //    }

        //}
        [Authorize("Authorization")]
        [HttpGet]
        public IActionResult User_Delete(int id)
        {
            bool res = users.Deleteuser(id);
            return Json(res);
        }
    }
}
