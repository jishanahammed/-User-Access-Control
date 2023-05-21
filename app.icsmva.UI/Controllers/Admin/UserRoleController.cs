using app.icsmva.DAO.dao.Application;
using app.icsmva.DAO.dao.userRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace app.icsmva.UI.Controllers.Admin
{
    [Authorize]
    public class UserRoleController : Controller
    {
        private readonly IUsersRoles usersRoles;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRoleController(IUsersRoles usersRoles, IHttpContextAccessor httpContextAccessor)
        {
            this.usersRoles = usersRoles;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize("Authorization")]
        public IActionResult Role_View(int page = 1, int pagesize = 10)
        {
            if (page < 1)
                page = 1;
            var LoginName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "LoginName").Value;
            var FullName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "FullName").Value;
            var pram = ("page:" + page + ",pagesize:" + pagesize + ",Curent_User:" + LoginName + "," + FullName + "").ToString();
            var result= usersRoles.GetRolesPagedListAsync(page, pagesize);
            var resd = ("Log Type:Information,Source: UserRole/Role_View,Messages:Information Get Successfully," + pram + "").ToString();
            Log.Information("\r\n" + resd + "\r\n");
            return View(result);
        }
        [HttpGet]
        public IActionResult Getpage(int page = 1, int pagesize = 10)
        {
            if (page < 1)
                page = 1;
            var LoginName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "LoginName").Value;
            var FullName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "FullName").Value;
            var pram = ("page:" + page + ",pagesize:" + pagesize + ",Curent_User:" + LoginName + "," + FullName + "").ToString();
            var result= usersRoles.GetRolesPagedListAsync(page, pagesize);
            var resd = ("Log Type:Information,Source: UserRole/Role_View,Messages:Information Get Successfully," + pram + "").ToString();
            Log.Information("\r\n" + resd + "\r\n");
            return PartialView("_userRolespaginatedpartial", result);
        }
        
    }
}
