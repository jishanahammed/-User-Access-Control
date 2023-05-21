using app.icsmva.DAO.dao.userRole;
using app.icsmva.DAO.dao.users;
using app.icsmva.UI.CurrentUser;
using app.icsmva.UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace app.icsmva.UI.Controllers
{
    public class loginController : Controller
    {
        private readonly IUsers usersservice;
        private readonly IUsersRoles usersRoles;
        private readonly ICurentUserGet curentUser;

        public loginController(IUsers usersservice, IUsersRoles usersRoles, ICurentUserGet curentUser)
        {
            this.usersservice = usersservice;
            this.usersRoles = usersRoles;
            this.curentUser = curentUser;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            var user = usersservice.GetUser(model.UserName);
            if (user == null)
            {
                Log.Error("\r\nLog Type: ERROR,Execution Time:" + DateTime.UtcNow + "Source:login/Index ,Messages:user not found ,Username:" + model.UserName + " password:" + model.Password + "");
                ModelState.AddModelError(string.Empty, "User not found");
                return View(model);
            }

            if (user.LoginPWD == model.Password)
            {
                var role = usersRoles.GetRole(user.RoleID);
                var claims = new List<Claim> {
                new Claim("LoginName" ,user.LoginName),
                new Claim("FullName" ,user.FullName),
                new Claim("Role" ,role.RoleName),
                new Claim("RoleId" ,role.RoleID.ToString()),
                new Claim("UserId" ,user.UserID.ToString()),
                };
                var identity = new ClaimsIdentity(
                   claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var prop = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, prop).Wait();
                Log.Information("\r\nLog Type: Information,Source:login/Index,Messages:login success,Username:" + model.UserName + " password:" + model.Password + "\r\n");
                return Redirect("/Admin/Index");
            }
            else
            {
                Log.Error("\r\nLog Type: ERROR,Source:login/Index ,Messages: User not verified,Username:" + model.UserName + " password:" + model.Password + "\r\n");
                ModelState.AddModelError(string.Empty, "User is not verified");
                return View(model);
            }

        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
             await HttpContext.SignOutAsync();
            return Redirect("/login/Index");
        }
    }
}
