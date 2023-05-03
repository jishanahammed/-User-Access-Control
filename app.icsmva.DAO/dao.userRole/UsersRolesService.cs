using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
using app.icsmva.Models;
using app.icsmva.Utility.Miscellaneous;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.userRole
{
    public class UsersRolesService : IUsersRoles
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly icsmvaDBContext db;
        private readonly IRolePrivilegemap privilegemap;
        public UsersRolesService(icsmvaDBContext db, IHttpContextAccessor httpContextAccessor, IRolePrivilegemap privilegemap)
        {
            this.db = db;
            _httpContextAccessor = httpContextAccessor;
            this.privilegemap = privilegemap;
        }

        public int Addrole(UserRoleViewModel model)
        {
            ROLES role = new ROLES();
            role.RoleName = model.RoleName;
            role.ApplicationName = model.ApplicationName;
            role.Remarks = model.Remarks;         
            var useid = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "UserId").Value;
            int UserId = Convert.ToInt32(useid);
            role.CreationDate=DateTime.UtcNow; 
            role.LastUpdatedDate=DateTime.UtcNow; 
            role.CreatedBy = UserId;
            db.ROLES.Add(role);
            db.SaveChanges();
            return role.RoleID;
        }
        public int Updaterole(UserRoleViewModel model)
        {
            ROLES role = db.ROLES.FirstOrDefault(f=>f.RoleID==model.RoleID);
            role.RoleName = model.RoleName;
            role.ApplicationName = model.ApplicationName;
            role.Remarks = model.Remarks;
            var useid = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "UserId").Value;
            int UserId = Convert.ToInt32(useid);
            role.LastUpdatedDate = DateTime.UtcNow;
            role.LastUpdatedBy = UserId;
            db.Entry(role).State = EntityState.Modified;
            var res = db.SaveChanges();
            if (res>0)
            {
                model.RoleID = role.RoleID;
                return role.RoleID;
            }
            return role.RoleID;
        }

        public ROLES GetRole(int id)
        {
            ROLES rOLES = db.ROLES.Where(f => f.RoleID == id).FirstOrDefault();
            return rOLES;
        }

        public ROLES GetRolename(string name)
        {
            var data = db.ROLES.Where(f => f.RoleName.Replace(" ", "").Trim() == name.Replace(" ", "").Trim() || f.RoleName.Contains(name)).FirstOrDefault();
            return data;
        }

        public List<ROLES> GetROLEs()
        {
            List<ROLES> roleList = db.ROLES.ToList();
            return roleList; 
        }

        public PagedModel<ROLES> GetRolesPagedListAsync(int page, int pageSize)
        {
            IQueryable<ROLES> list = db.ROLES.Where(f => f.IsDeleted == null).AsQueryable();
            int resCount = list.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = list.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<ROLES> pagedModel = new PagedModel<ROLES>()
            {
                Models = pagedList,
                FirstSerialNumber = FirstSerialNumber,
                PagedList = pagers,
                TotalEntity = resCount,
                action = privilegemap.Rolepermition()
            };
            return pagedModel;
        }

        public ROLES GetRolenameexit(UserRoleViewModel model)
        {
            ROLES rOLES = db.ROLES.FirstOrDefault(f => f.RoleName == model.RoleName && f.RoleID != model.RoleID);
            return rOLES;
        }

        public int Deleterole(int id)
        {
            ROLES role= db.ROLES.FirstOrDefault(d=>d.RoleID == id);
            role.IsDeleted = DateTime.UtcNow;
            db.Entry(role).State = EntityState.Modified;
            var res = db.SaveChanges();
            if (res>0) {
                return role.RoleID;
            }
            else
            {
                return 0;
            }
        }
    }
}
