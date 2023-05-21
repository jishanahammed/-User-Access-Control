using app.icsmva.DAO.dao.app.Role;
using app.icsmva.DAO.dao.app.user;
using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.Privilege;
using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
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
    public class UserRoleAddController : Controller
    {
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        private readonly IRolePrivilegemap privilegemap;
        private readonly IPrivilege privilege;
        private readonly IAppRole appRole;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRoleAddController(IUsersRoles usersRoles, IAppRole appRole, IApplicationName application, IRolePrivilegemap privilegemap, IPrivilege privilege, IHttpContextAccessor _httpContextAccessor)
        {
            this.usersRoles = usersRoles;
            this.application = application;
            this.privilegemap = privilegemap;
            this.privilege = privilege;
            this.appRole = appRole;
            this._httpContextAccessor = _httpContextAccessor;
        }

        [Authorize("Authorization")]
        public IActionResult Role_Add(int id = 0)
        {
            UserRoleViewModel userRoleViewModel = new UserRoleViewModel();
            if (id != 0)
            {
                var role = usersRoles.GetRole(id);
                userRoleViewModel.RoleName = role.RoleName;
                userRoleViewModel.RoleID = role.RoleID;
                userRoleViewModel.ApplicationName = role.ApplicationName;
                userRoleViewModel.CreationDate = role.CreationDate;
                userRoleViewModel.LastUpdatedDate = role.LastUpdatedDate;
                userRoleViewModel.Remarks = role.Remarks;
                userRoleViewModel.mapprivilege = privilege.GetAllprivilige(id);
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                return View(userRoleViewModel);
            }
            else
            {
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                userRoleViewModel.mapprivilege = privilege.GetAllprivilige(0);
                return View(userRoleViewModel);
            }

        }

        [HttpPost]
        public IActionResult Role_Add(UserRoleViewModel model)
        {
            var res3 = appRole.AddRecode(model);
            var LoginName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "LoginName").Value;
            var FullName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "FullName").Value;
            var pram = ("RoleName:" + model.RoleName + ",ApplicationName:" + model.ApplicationName + ",Remarks:" + model.Remarks + ",Curent_User:" + LoginName + "," + FullName + "").ToString();
            if (res3 == "successfilly")
            {
                ModelState.Clear();
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                model.mapprivilege = privilege.GetAllprivilige(0);
                ViewBag.message = "successfully".ToString();

                var resd = ("Log Type:Information,Source: UserRoleAdd/Role_Add,SQL Query:ROLES_Create,Messages:Information Added Successfully," + pram + "").ToString();
                Log.Information("\r\n" + resd + "\r\n");
                return View(model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, res3.ToString());
                var resd = ("Log Type:Error,Source: UserRoleAdd/Role_Add,SQL Query:ROLES_Create,Messages:" + res3.ToString() + "," + pram + "").ToString();
                Log.Information("\r\n" + resd + "\r\n");
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                model.mapprivilege = privilege.GetAllprivilige(0);
                return View(model);
            }
        }


        //    [HttpPost]
        //public IActionResult Role_Add(UserRoleViewModel userRoleViewModel)
        //{
        //    var res = usersRoles.GetRolename(userRoleViewModel.RoleName);
        //    if (res == null)
        //    {
        //        int id = usersRoles.Addrole(userRoleViewModel);
        //        if (id == 0)
        //        {
        //            ModelState.AddModelError(string.Empty, "Entry Faild");
        //            ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
        //            userRoleViewModel.mapprivilege = privilege.GetAllprivilige(0);
        //            return View(userRoleViewModel);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Role_Modify", "UserRoleModify", new { id = id });
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("RoleName", "This Role Name is  Already Exists");
        //        ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
        //        userRoleViewModel.mapprivilege = privilege.GetAllprivilige(0);
        //        return View(userRoleViewModel);
        //    }

        //}
    }
}
