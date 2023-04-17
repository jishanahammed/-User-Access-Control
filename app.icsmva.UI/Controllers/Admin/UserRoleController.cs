using app.icsmva.DAO.dao.userRole;
using Microsoft.AspNetCore.Mvc;

namespace app.icsmva.UI.Controllers.Admin
{
    public class UserRoleController : Controller
    {
        private readonly IUsersRoles usersRoles;
        public UserRoleController(IUsersRoles usersRoles)
        {
            this.usersRoles = usersRoles;
        }
        public IActionResult Role_View(int page = 1, int pagesize = 10)
        {
            if (page < 1)
                page = 1;
            var result= usersRoles.GetRolesPagedListAsync(page, pagesize);  
            return View(result);
        }
        [HttpGet]
        public IActionResult Getpage(int page = 1, int pagesize = 10)
        {
            if (page < 1)
                page = 1;
            var result= usersRoles.GetRolesPagedListAsync(page, pagesize);
            return PartialView("_userRolespaginatedpartial", result);
        }
        
    }
}
