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

        public MvaUserModifyController(IUsers users, IUsersRoles usersRoles, IApplicationName application, Iappusers iappusers)
        {
            this.users = users;
            this.usersRoles = usersRoles;
            this.application = application;
            this.iappusers = iappusers;
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
        public IActionResult User_Modify_copy(UserViewModel model)
        {
            var res = users.GetUser(model.LoginName);
            if (res==null||res.UserID==model.UserID)
            {
                bool result = users.Updateuser(model);
                if (result ==false)
                {
                    ModelState.AddModelError(string.Empty, "Entry Faild");
                    ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                    ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update Successfully");
                    ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                    ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("LoginName", "This Login Name is  Already Exists");
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                return View(model);
            }
        }
        [HttpPost]
        public IActionResult User_Modify(UserViewModel model)
        {
            var res3 = iappusers.UpdateRecode(model);
            var pram = ("ApplicationName:" + model.ApplicationName + ",RoleID:" + model.RoleID + ",Remarks:" + model.Remarks + "").ToString();
            if (res3 == "successfilly")
            {
                ModelState.Clear();
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                ViewBag.message = "successfully updated".ToString();
               
                var resd = ("Log Type:Information\r\nSource: MvaUserModify/User_Modify\r\nSQL Query:USERS_Update/r/nMessages:Information Updated Successfully/r/n" + pram + "").ToString();
                Log.Information(resd);
                return View(model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, res3.ToString());
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                ViewBag.rolelist = new SelectList((usersRoles.GetROLEs()).Select(s => new { Id = s.RoleID, Name = s.RoleName }), "Id", "Name");
                var resd = ("Log Type:Warning/r/nSource:MvaUserModify/User_Modify/r/nSQL Query:USERS_Update/r/nMessages:" + res3.ToString() + "/r/n" + pram + "").ToString();
                Log.Warning(resd);
                return View(model);
            }
        }

    }
}
