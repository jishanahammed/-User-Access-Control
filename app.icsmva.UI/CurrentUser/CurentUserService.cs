using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using app.icsmva.DAO.dao.users;
using app.icsmva.Models;
using app.icsmva.DAO.dao.RolesAndPrivilegeMap;

namespace app.icsmva.UI.CurrentUser
{
    public class CurentUserService : ICurentUserGet
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsers usersservice;
        private readonly IRolePrivilegemap privilegemapService;
        public CurentUserService(IHttpContextAccessor _httpContextAccessor, IUsers usersservice, IRolePrivilegemap privilegemapService)
        {
            this._httpContextAccessor = _httpContextAccessor;
            this.usersservice = usersservice;
            this.privilegemapService = privilegemapService;
        }
        public CurrentUserInfoVM getcurrentuser()
        {
            CurrentUserInfoVM vM= new CurrentUserInfoVM();
            vM.LoginName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "LoginName").Value;
            vM.FullName = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "FullName").Value;
            var userid= _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "UserId").Value;
            vM.UserId=Convert.ToInt32(userid);  
            vM.RoleName= _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "Role").Value;
            var Rid= _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "RoleId").Value;
              vM.RoleId = Convert.ToInt32(Rid);
            if (vM.RoleId != 0)
            {
                vM.MenuPermition = privilegemapService.UserMenu(vM.RoleId);
            }
            return vM; 
        }
    }
}
