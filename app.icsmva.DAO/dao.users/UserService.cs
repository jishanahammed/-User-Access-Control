using app.icsmva.Models;
using app.icsmva.Utility.Miscellaneous;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.users
{
    public class UserService : IUsers
    {
        private readonly icsmvaDBContext db;
        public UserService(icsmvaDBContext db)
        {
            this.db = db;
        }

        public USERS GetUser(string name)
        {
            USERS uSERS = db.USERS.Where(f => f.IsDeleted == 1 && f.LoginName == name).FirstOrDefault();
            return uSERS;
        }

        public PagedModel<USERS> GetUserPagedListAsync(int page, int pageSize)
        {
            IQueryable<USERS> list =  db.USERS.Where(f => f.IsDeleted==1).AsQueryable();
            int resCount = list.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = list.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<USERS> pagedModel = new PagedModel<USERS>()
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

