using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.userRole;
using app.icsmva.DAO.dao.users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.Mime.MediaTypeNames;

namespace app.icsmva.UI.Controllers.Admin
{
    [Authorize]
    public class MvaUserModifyController : Controller
    {
        private readonly IUsers users;
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        public MvaUserModifyController(IUsers users, IUsersRoles usersRoles, IApplicationName application)
        {
            this.users = users;
            this.usersRoles = usersRoles;
            this.application = application;
        }
        [Authorize("Authorization")]
        public IActionResult User_Modify(int userId)
        {
            UserViewModel user = users.GetUserbyid(userId);
            ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
            ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
            return View(user);
        }
        [HttpPost]
        public IActionResult User_Modify(UserViewModel model)
        {
            var res = users.GetUser(model.LoginName);
            if (res==null||res.UserID==model.UserID)
            {
                bool result = users.Updateuser(model);
                if (result ==false)
                {
                    ModelState.AddModelError(string.Empty, "Entry Faild");
                    ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
                    ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update Successfully");
                    ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
                    ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                    return View(model);
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
