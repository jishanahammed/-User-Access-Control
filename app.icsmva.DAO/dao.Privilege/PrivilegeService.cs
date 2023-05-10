using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
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

     

        public List<RolePrivilegeViewModel> GetAllprivilige(int roleId)
        {
            List<RolePrivilegeViewModel> models = new List<RolePrivilegeViewModel>();   
           var res = db.PRIVILEGES.Where(f => (DateTime)f.IsDeleted == null).OrderBy(f=>f.Precedence).ToList();   
            foreach (var item in res)
            {
              
                RolePrivilegeViewModel model= new RolePrivilegeViewModel(); 
                model.PrivilegeID = item.PrivilegeID;
                model.PrivilegeName = item.PrivilegeName;   
                model.UIName = item.UserInterfaceName; 
                model.ActionName = item.ActionName; 
                model.ActionPrecedence = item.ActionPrecedence;
                if (roleId > 0)
                {
                    var data = db.ROLE_PRIVILEGES.Where(f => f.PrivilegeID == item.PrivilegeID && f.RoleID == roleId&& (DateTime)f.IsDeleted==null).FirstOrDefault();
                    if (data != null)
                    {
                        model.IsAssign = true;
                    }
                    else
                    {
                        model.IsAssign = false;
                    }
                }
                models.Add(model);  
            }
            return models;
        }

        public PagedModel<PRIVILEGES> GetpriviligePagedListAsync(int page, int pageSize)
        {
            IQueryable<PRIVILEGES> list = db.PRIVILEGES.Where(f => (DateTime)f.IsDeleted ==null).AsQueryable();
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
