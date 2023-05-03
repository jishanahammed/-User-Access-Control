
using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.Privilege;
using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
using app.icsmva.DAO.dao.userRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace app.icsmva.UI.Controllers.Admin
{
    public class UserRoleDeleteController : Controller
    {
        private readonly IUsersRoles usersRoles;
        private readonly IApplicationName application;
        private readonly IRolePrivilegemap privilegemap;
        private readonly IPrivilege privilege;

        public UserRoleDeleteController(IUsersRoles usersRoles, IApplicationName application, IRolePrivilegemap privilegemap, IPrivilege privilege)
        {
            this.usersRoles = usersRoles;
            this.application = application;
            this.privilegemap = privilegemap;
            this.privilege = privilege;
        }

        [Authorize("Authorization")]
        public IActionResult Role_Delete(int id)
        {
            var res = usersRoles.Deleterole(id);
            return Json(res);
        }

        //public IActionResult Role_Delete(int id)
        //{
        //       UserRoleViewModel userRoleViewModel = new UserRoleViewModel();
        //        var role = usersRoles.GetRole(id);
        //        userRoleViewModel.RoleName = role.RoleName;
        //        userRoleViewModel.RoleID = role.RoleID;
        //        userRoleViewModel.ApplicationName = role.ApplicationName;
        //        userRoleViewModel.CreationDate = role.CreationDate;
        //        userRoleViewModel.LastUpdatedDate = role.LastUpdatedDate;
        //        userRoleViewModel.Remarks = role.Remarks;
        //        userRoleViewModel.mapprivilege = privilege.GetAllprivilige(id).Where(f=>f.IsAssign==true).ToList();
        //        ViewBag.applicationlist = new SelectList((application.Getlist()).Select(s => new { Id = s.Applicationname, Name = s.Applicationname }), "Id", "Name");
        //        return View(userRoleViewModel);
        //    }
        //[HttpPost]
        //public IActionResult Role_Delete(UserRoleViewModel model)
        //{
        //    var res=usersRoles.Deleterole(model);
        //    if (res>0)
        //    {
        //        return RedirectToAction("Role_View", "UserRole");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Entry Faild");
        //        return View(model);
        //    }
        //}
    }
}
