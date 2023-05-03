using app.icsmva.Models;
using app.icsmva.Utility.Miscellaneous;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public RolePrivilegeViewModel Getparmition(int RoleId, string actinname, string controllrname)
        {
            RolePrivilegeViewModel model = new RolePrivilegeViewModel();
            ROLESPRIVILEGESMAP vv = new ROLESPRIVILEGESMAP();
            var useid = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "UserId").Value;
            int id = Convert.ToInt32(useid);
            var privlige = db.PRIVILEGES.FirstOrDefault(f => f.ActionName.Trim() == actinname.Trim() && f.PrivilegeName.Trim() == controllrname.Trim());
            var user = db.USERS.FirstOrDefault(f => f.UserID == id && f.IsDeleted == null);
            var role = db.ROLES.FirstOrDefault(f => f.RoleID == RoleId && f.IsDeleted == null);
            if (privlige != null && user != null && role != null)
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
            var role = db.ROLES.Where(d => d.RoleID == RoleId).FirstOrDefault();
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
            var mapdata = db.ROLESPRIVILEGESMAP.FirstOrDefault(f => f.RPMapId == RPMapId && f.IsDeleted == 1);
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

        public bool Getsingleupdate(int PrivilegeId, int roleId)
        {
            var useid = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "UserId").Value;
            int UserId = Convert.ToInt32(useid);
            var mapdata = db.ROLESPRIVILEGESMAP.FirstOrDefault(f => f.PrivilegeID == PrivilegeId && f.RoleID == roleId);
            if (mapdata == null)
            {
                ROLESPRIVILEGESMAP entty = new ROLESPRIVILEGESMAP();
                entty.RoleID = roleId;
                entty.PrivilegeID = PrivilegeId;
                entty.CreationDate = DateTime.UtcNow;
                entty.LastUpdatedDate = DateTime.UtcNow;
                entty.CreatedBy = UserId;
                entty.IsDeleted = 1;
                db.ROLESPRIVILEGESMAP.Add(entty);
                var res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (mapdata.IsDeleted == 0)
                {
                    mapdata.IsDeleted = 1;
                }
                else
                {
                    mapdata.IsDeleted = 0;
                }
               
                mapdata.LastUpdatedDate = DateTime.UtcNow;
                mapdata.LastUpdatedBy = UserId;
                db.Entry(mapdata).State = EntityState.Modified;
                var res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<ActionViewModel> Rolepermition()
        {
            var Rid = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "RoleId").Value;
            int RoleId = Convert.ToInt32(Rid);
            var role = db.ROLES.Where(d => d.RoleID == RoleId).FirstOrDefault();
            var mapdata = db.ROLESPRIVILEGESMAP.Where(f => f.RoleID == RoleId && f.IsDeleted == 1).ToList();
            List<ActionViewModel> models = new List<ActionViewModel>();
            foreach (var item in mapdata)
            {
                ActionViewModel model = new ActionViewModel();
                var privlige = db.PRIVILEGES.FirstOrDefault(f => f.PrivilegeID == item.PrivilegeID);

                model.ActionName = privlige.ActionName.Trim();
                model.ControllerName = privlige.PrivilegeName.Trim();
                model.url = "/" + privlige.PrivilegeName.Trim() + "/" + privlige.ActionName.Trim();
                models.Add(model);
            }
            return models;
        }

        public List<UserMenuPermitionVM> UserMenu(int RoleId)
        {
            List<UserMenuPermitionVM> models = new List<UserMenuPermitionVM>();
            var role = db.ROLES.Where(d => d.RoleID == RoleId).FirstOrDefault();
            var mapdata = db.ROLESPRIVILEGESMAP.Where(f => f.RoleID == RoleId && f.IsDeleted == 1).ToList();

            foreach (var item in mapdata)
            {
                UserMenuPermitionVM model = new UserMenuPermitionVM();
                var privlige = db.PRIVILEGES.FirstOrDefault(f => f.PrivilegeID == item.PrivilegeID && f.ActionPrecedence == 2);
                if (privlige != null && models.FirstOrDefault(d => d.UiNmae == privlige.UIName) == null)
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
