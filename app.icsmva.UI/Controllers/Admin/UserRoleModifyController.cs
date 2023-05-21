using app.icsmva.DAO.dao.app.Role;
using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.Privilege;
using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
using app.icsmva.DAO.dao.userRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace app.icsmva.UI.Controllers.Admin
{
    [Authorize]
    public class UserRoleModifyController : Controller
    {
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        private readonly IRolePrivilegemap privilegemap;
        private readonly IPrivilege privilege;
        private readonly IAppRole appRole;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRoleModifyController(IUsersRoles usersRoles, IHttpContextAccessor _httpContextAccessor, IAppRole appRole, IApplicationName application, IRolePrivilegemap privilegemap, IPrivilege privilege)
        {
            this.usersRoles = usersRoles;
            this.application = application;
            this.privilegemap = privilegemap;
            this.privilege = privilege;
            this.appRole = appRole;
            this._httpContextAccessor = _httpContextAccessor;
        }

        [Authorize("Authorization")]
        public IActionResult Role_Modify(int id = 0)
        {
            UserRoleViewModel userRoleViewModel = new UserRoleViewModel();
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
       
        [HttpPost]
        public IActionResult Role_Modify(UserRoleViewModel model)
        {
            var res3 = appRole.UpdateRecode(model);
            var LoginName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "LoginName").Value;
            var FullName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "FullName").Value;
            var pram = ("RoleName:" + model.RoleName + ",ApplicationName:" + model.ApplicationName + ",Remarks:" + model.Remarks + ",Curent_User:" + LoginName + "," + FullName + "").ToString();
            if (res3 == "successfilly")
            {
                ModelState.Clear();
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                model.mapprivilege = privilege.GetAllprivilige(0);
                var resd = ("Log Type:Information,Source: UserRoleModify/Role_Modify,SQL Query:ROLES_Update,Messages:Updated Added Successfully," + pram + "").ToString();
                Log.Information("\r\n" + resd + "\r\n");
                return RedirectToAction("Role_Modify", new { id = model.RoleID });
            }
            else
            {
                ModelState.AddModelError(string.Empty, res3.ToString());
                var resd = ("Log Type:Error,Source: UserRoleModify/Role_Modify,SQL Query:ROLES_Update,Messages:" + res3.ToString() + "," + pram + "").ToString();
                Log.Information("\r\n" + resd + "\r\n");
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationName, Name = s.ApplicationName }), "Id", "Name");
                model.mapprivilege = privilege.GetAllprivilige(0);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ActiveInactive(string id, int roleId)
        {
            var LoginName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "LoginName").Value;
            var FullName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "FullName").Value;
            var pram = ("PrivilegeID:" + id + ",roleId:" + roleId + ",Curent_User:" + LoginName + "," + FullName + "").ToString();

            var res = privilegemap.Getsingleupdate(id, roleId);
            if (res)
            {
                var resd = ("Log Type:Information,Source: UserRoleModify/ActiveInactive,Messages:Privilege Assign Successfully," + pram + "").ToString();
                Log.Information("\r\n" + resd + "\r\n");
            }
            else
            {
                var resd = ("Log Type:Warning,Source: UserRoleModify/ActiveInactive,Messages:Privilege Assign not Successfully," + pram + "").ToString();
                Log.Information("\r\n" + resd + "\r\n");
            }
            return Json(res);
        }
    }
}
