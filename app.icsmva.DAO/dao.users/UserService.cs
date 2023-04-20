using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
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
using System.Xml.Linq;

namespace app.icsmva.DAO.dao.users
{
    public class UserService : IUsers
    {
        private readonly icsmvaDBContext db;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRolePrivilegemap privilegemap;
        public UserService(icsmvaDBContext db, IRolePrivilegemap privilegemap, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            this.privilegemap = privilegemap;
            this.httpContextAccessor = httpContextAccessor;
        }

        public int Adduser(UserViewModel model)
        {
            USERS user=new USERS();
            user.LoginName = model.LoginName;
            user.LoginPWD = model.LoginPWD;
            user.FullName = model.FullName;
            user.RoleID = model.RoleID;
            user.ApplicationName = model.ApplicationName;
            user.Remarks = model.Remarks;
            user.EmployeeNo = model.EmployeeNo; 
            var useid = httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "UserId").Value;
            int UserId = Convert.ToInt32(useid);
            user.CreationDate = DateTime.UtcNow;
            user.LastUpdatedDate = DateTime.UtcNow;
            user.CreatedBy = UserId;
            user.IsDeleted = 1;
            db.USERS.Add(user);
            db.SaveChanges();
            if (user.UserID!=0) {
                model.UserID = user.UserID;
            }
            else
            {
                model.UserID = 0;   
            }
            return model.UserID;    
        }

        public bool Deleteuser(UserViewModel model)
        {
            USERS user = db.USERS.FirstOrDefault(f => f.UserID == model.UserID);
            user.IsDeleted = 0;
            db.Entry(user).State = EntityState.Modified;
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

        public USERS GetUser(string name)
        {
            USERS uSERS = db.USERS.Where(f => f.IsDeleted == 1 && f.LoginName == name).FirstOrDefault();
            return uSERS;
        }

        public UserViewModel GetUserbyid(int userid)
        {
            UserViewModel user = new UserViewModel();
            USERS model = db.USERS.Where(f => f.IsDeleted == 1 && f.UserID == userid).FirstOrDefault();
            user.LoginName = model.LoginName;
            user.LoginPWD = model.LoginPWD;
            user.FullName = model.FullName;
            user.RoleID = model.RoleID;
            user.ApplicationName = model.ApplicationName;
            user.Remarks = model.Remarks;
            user.EmployeeNo = model.EmployeeNo;        
            user.CreationDate = model.CreationDate;
            user.LastUpdatedDate =model.LastUpdatedDate;
            user.CreatedBy = model.CreatedBy;
            user.UserID = model.UserID;
            return user;
        }

        public PagedModel<USERS> GetUserPagedListAsync(int page, int pageSize)
        {
            IQueryable<USERS> list =  db.USERS.Where(f => f.IsDeleted==1).OrderByDescending(f=>f.UserID).AsQueryable();
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
                TotalEntity = resCount,
                action = privilegemap.Rolepermition()
            };
            return pagedModel;
        }

        public bool Updateuser(UserViewModel model)
        {
            USERS user = db.USERS.FirstOrDefault(f=>f.UserID==model.UserID);
            user.LoginName = model.LoginName;
            user.LoginPWD = model.LoginPWD;
            user.FullName = model.FullName;
            user.RoleID = model.RoleID;
            user.ApplicationName = model.ApplicationName;
            user.Remarks = model.Remarks;
            user.EmployeeNo = model.EmployeeNo;
            var useid = httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "UserId").Value;
            int UserId = Convert.ToInt32(useid);
            user.LastUpdatedDate = DateTime.UtcNow;
            user.LastUpdatedBy = UserId;
            user.IsDeleted = 1;
            db.Entry(user).State = EntityState.Modified;
            var res=  db.SaveChanges();
            if (res>0)
            {
               return true; 
            }
            else
            {
                return false;
            }
           
        }
    }
}

