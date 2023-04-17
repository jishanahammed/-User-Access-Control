using app.icsmva.Models;
using app.icsmva.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.Privilege
{
    public class PrivilegeService : IPrivilege
    {
        private readonly icsmvaDBContext db;
        public PrivilegeService(icsmvaDBContext db)
        {
            this.db = db;
        }

        public PagedModel<PRIVILEGES> GetRolesPagedListAsync(int page, int pageSize)
        {
            IQueryable<PRIVILEGES> list = db.PRIVILEGES.Where(f => f.IsDeleted == 1).AsQueryable();
            int resCount = list.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = list.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<PRIVILEGES> pagedModel = new PagedModel<PRIVILEGES>()
            {
                Models = pagedList,
                FirstSerialNumber = FirstSerialNumber,
                PagedList = pagers,
                TotalEntity = resCount
            };
            return pagedModel;
        }
    }
}
