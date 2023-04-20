using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.Privilege;
using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
using app.icsmva.DAO.dao.userRole;
using app.icsmva.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace app.icsmva.UI.Controllers.Admin
{
    [Authorize]
    public class UserRoleAddController : Controller
    {
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        private readonly IRolePrivilegemap privilegemap;
        private readonly IPrivilege privilege;

        public UserRoleAddController(IUsersRoles usersRoles, IApplicationName application, IRolePrivilegemap privilegemap, IPrivilege privilege)
        {
            this.usersRoles = usersRoles;
            this.application = application;
            this.privilegemap = privilegemap;
            this.privilege = privilege;
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
                userRoleViewModel.Remarks=role.Remarks;
                userRoleViewModel.mapprivilege = privilege.GetAllprivilige(id);
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
                return View(userRoleViewModel);
            }
            else
            {
                ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
                userRoleViewModel.mapprivilege = privilege.GetAllprivilige(0);
                return View(userRoleViewModel);
            }

        }
        [HttpPost]
        public IActionResult Role_Add(UserRoleViewModel userRoleViewModel)
        {
            var res = usersRoles.GetRolename(userRoleViewModel.RoleName);
            if (userRoleViewModel.RoleID > 0)
            {
                var res2 = usersRoles.GetRolenameexit(userRoleViewModel);
                if (res2==null)
                {
                    int result1 = usersRoles.Updaterole(userRoleViewModel);
                    if (result1==0)
                    {
                        ModelState.AddModelError(string.Empty, "Entry Faild");
                        ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
                        userRoleViewModel.mapprivilege = privilege.GetAllprivilige(0);
                        return View(userRoleViewModel);
                    }
                    else
                    {
                        return RedirectToAction("Role_Add", new { id = result1 });
                    }
                }
                else
                {
                    ModelState.AddModelError("RoleName", "This Role Name is  Already Exists");
                    ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
                    userRoleViewModel.mapprivilege = privilege.GetAllprivilige(0);
                    return View(userRoleViewModel);
                }
            }
            else
            {                
                if (res == null)
                {
                    int id = usersRoles.Addrole(userRoleViewModel);
                    if (id == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Entry Faild");
                        ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
                        userRoleViewModel.mapprivilege = privilege.GetAllprivilige(0);
                        return View(userRoleViewModel);
                    }
                    else
                    {
                        return RedirectToAction("Role_Add", new { id = id });
                    }
                }
                else
                {
                    ModelState.AddModelError("RoleName", "This Role Name is  Already Exists");
                    ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
                    userRoleViewModel.mapprivilege = privilege.GetAllprivilige(0);
                    return View(userRoleViewModel);
                }
            }
        }
    }
}
