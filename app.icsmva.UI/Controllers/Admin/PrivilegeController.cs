using app.icsmva.DAO.dao.Privilege;
using app.icsmva.DAO.dao.userRole;
using Microsoft.AspNetCore.Mvc;

namespace app.icsmva.UI.Controllers.Admin
{
    public class PrivilegeController : Controller
    {
        private readonly IPrivilege privilegeservice;
        public PrivilegeController(IPrivilege privilegeservice)
        {
            this.privilegeservice = privilegeservice;
        }
        public IActionResult Index(int page = 1, int pagesize = 10)
        {
            if (page < 1)
                page = 1;
            var result = privilegeservice.GetRolesPagedListAsync(page, pagesize);
            return View(result);
        }
        [HttpGet]
        public IActionResult Getpage(int page = 1, int pagesize = 10)
        {
            if (page < 1)
                page = 1;
            var result = privilegeservice.GetRolesPagedListAsync(page, pagesize);
            return PartialView("_privilegepaginatedpartial", result);
        }
        public IActionResult AssignRole()
        {
            return View();
        }
    }
}
