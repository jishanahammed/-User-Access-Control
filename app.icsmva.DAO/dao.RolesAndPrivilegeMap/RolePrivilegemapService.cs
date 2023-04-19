using app.icsmva.Models;
using app.icsmva.Utility.Miscellaneous;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.RolesAndPrivilegeMap
{
    public class RolePrivilegemapService : IRolePrivilegemap
    {
        private readonly icsmvaDBContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RolePrivilegemapService(icsmvaDBContext db, IHttpContextAccessor _httpContextAccessor)
        {
            this.db = db;
            this._httpContextAccessor = _httpContextAccessor;
        }

        public RolePrivilegeViewModel Getparmition(int RoleId,string actinname, string controllrname)
        {
            RolePrivilegeViewModel model= new RolePrivilegeViewModel();
            ROLESPRIVILEGESMAP vv = new ROLESPRIVILEGESMAP();
             var privlige = db.PRIVILEGES.FirstOrDefault(f => f.ActionName.Trim() == actinname.Trim()&&f.PrivilegeName.Trim() == controllrname.Trim());
            if (privlige!=null)
            {
                vv = db.ROLESPRIVILEGESMAP.FirstOrDefault(f => f.RoleID == RoleId && f.PrivilegeID == privlige.PrivilegeID);
            }
            else
            {
                return model;
            }
            
            if (vv != null)
            {
                model.ActionName = privlige.ActionName;   
                model.PrivilegeName = privlige.PrivilegeName;  
                return model;   
            }
            return model;
        }

        public List<RolePrivilegeViewModel> GetRoleWise(int RoleId)
        {
            List<RolePrivilegeViewModel> models = new List<RolePrivilegeViewModel>();   
            var role = db.ROLES.Where(d=>d.RoleID==RoleId).FirstOrDefault();
            var mapdata = db.ROLESPRIVILEGESMAP.Where(f => f.RoleID == RoleId).ToList();
            foreach (var item in mapdata)
            {
                RolePrivilegeViewModel model = new RolePrivilegeViewModel();    
                var privlige = db.PRIVILEGES.FirstOrDefault(f => f.PrivilegeID == item.PrivilegeID);
                model.RoleID = RoleId;
                model.RoleName = role.RoleName;
                model.PrivilegeID = privlige.PrivilegeID;
                model.UIName = privlige.UIName;
                model.ActionName = privlige.ActionName;
                model.PrivilegeName = privlige.PrivilegeName;
                model.ActionPrecedence = privlige.ActionPrecedence;
                model.IsAssign = true;
                models.Add(model);  
            }          
           return models;
        }

        public RolePrivilegeViewModel Getsingle(int RPMapId)
        {
            RolePrivilegeViewModel model = new RolePrivilegeViewModel();
            var mapdata = db.ROLESPRIVILEGESMAP.FirstOrDefault(f => f.RPMapId == RPMapId);
            var privlige = db.PRIVILEGES.FirstOrDefault(f => f.PrivilegeID == mapdata.PrivilegeID);
            var role = db.ROLES.Where(d => d.RoleID == mapdata.RoleID).FirstOrDefault();
            model.RoleID = mapdata.RoleID;
            model.RoleName = role.RoleName;
            model.PrivilegeID = privlige.PrivilegeID;
            model.UIName = privlige.UIName;
            model.ActionName = privlige.ActionName;
            model.PrivilegeName = privlige.PrivilegeName;
            model.ActionPrecedence = privlige.ActionPrecedence;
            return model;   
        }

        public List<ActionViewModel> Rolepermition()
      {
            var Rid = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "RoleId").Value;
            int RoleId = Convert.ToInt32(Rid);
            var role = db.ROLES.Where(d => d.RoleID == RoleId).FirstOrDefault();
            var mapdata = db.ROLESPRIVILEGESMAP.Where(f => f.RoleID == RoleId).ToList();
            List<ActionViewModel> models = new List<ActionViewModel>();
            foreach (var item in mapdata)
            {
                ActionViewModel model = new ActionViewModel();
                var privlige = db.PRIVILEGES.FirstOrDefault(f => f.PrivilegeID == item.PrivilegeID);
             
                    model.ActionName = privlige.ActionName.Trim();
                    model.ControllerName = privlige.PrivilegeName.Trim();
                    model.url ="/"+privlige.PrivilegeName.Trim()+"/"+privlige.ActionName.Trim();
                    models.Add(model);               
            }
            return models;
        }

        public List<UserMenuPermitionVM> UserMenu(int RoleId)
        {
            List<UserMenuPermitionVM> models = new List<UserMenuPermitionVM>();
            var role = db.ROLES.Where(d => d.RoleID == RoleId).FirstOrDefault();
            var mapdata = db.ROLESPRIVILEGESMAP.Where(f => f.RoleID == RoleId).ToList();

            foreach (var item in mapdata)
            {
                UserMenuPermitionVM model = new UserMenuPermitionVM();
                var privlige = db.PRIVILEGES.FirstOrDefault(f => f.PrivilegeID == item.PrivilegeID);
                if (models.FirstOrDefault(d => d.UiNmae == privlige.UIName) == null)
                {
                    model.UiNmae = privlige.UIName;
                    model.ActionName = privlige.ActionName;
                    model.ControllerName = privlige.PrivilegeName;
                    models.Add(model);
                }

            }
            return models;
        }
    }
}
