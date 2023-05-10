using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.Privilege;
using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
using app.icsmva.DAO.dao.userRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace app.icsmva.UI.Controllers.Admin
{
    [Authorize]
    public class UserRoleModifyController : Controller
    {
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        private readonly IRolePrivilegemap privilegemap;
        private readonly IPrivilege privilege;

        public UserRoleModifyController(IUsersRoles usersRoles, IApplicationName application, IRolePrivilegemap privilegemap, IPrivilege privilege)
        {
            this.usersRoles = usersRoles;
            this.application = application;
            this.privilegemap = privilegemap;
            this.privilege = privilege;
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
            ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationID, Name = s.ApplicationName }), "Id", "Name");
            return View(userRoleViewModel);
        }
        [HttpPost]
        public IActionResult Role_Modify(UserRoleViewModel userRoleViewModel)
        {
            var res2 = usersRoles.GetRolenameexit(userRoleViewModel);
            if (res2 == null)
            {
                int result1 = usersRoles.Updaterole(userRoleViewModel);
                if (result1 == 0)
                {
                    ModelState.AddModelError(string.Empty, "Entry Faild");
                    ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationID, Name = s.ApplicationName }), "Id", "Name");
                    userRoleViewModel.mapprivilege = privilege.GetAllprivilige(0);
                    return View(userRoleViewModel);
                }
                else
                {
                    return RedirectToAction("Role_Modify", new { id = result1 });
                }
            }
            else
            {
                ModelState.AddModelError("RoleName", "This Role Name is  Already Exists");
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.ApplicationID, Name = s.ApplicationName }), "Id", "Name");
                userRoleViewModel.mapprivilege = privilege.GetAllprivilige(0);
                return View(userRoleViewModel);
            }
        }
        [HttpGet]
        public IActionResult ActiveInactive(string id, int roleId)
        {
            var res = privilegemap.Getsingleupdate(id, roleId);
            return Json(res);
        }
    }
}
