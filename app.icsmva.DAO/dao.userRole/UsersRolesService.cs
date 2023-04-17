using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
using app.icsmva.Models;
using app.icsmva.Utility.Miscellaneous;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.userRole
{
    public class UsersRolesService:IUsersRoles
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

        public ROLES GetRole(int id)
        {
           ROLES rOLES = db.ROLES.Where(f=>f.RoleID==id).FirstOrDefault();
            return rOLES;
        }

        public PagedModel<ROLES> GetRolesPagedListAsync(int page, int pageSize)
        {
            IQueryable<ROLES> list = db.ROLES.Where(f => f.IsDeleted == 1).AsQueryable();
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
    }
}
