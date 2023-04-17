using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.Models
{
    public class USERS
    {
        [Key]
        public int UserID { set; get; }
        public string LoginName { set; get; }
        public string LoginPWD { set; get; }
        public int EmployeeNo { set; get; }
        public string FullName { set; get; }
        public string Remarks { set; get; }
        public string ApplicationName { set; get; }
        public int RoleID { set; get; }
        public DateTime CreationDate { set; get; }
        public int CreatedBy { set; get; }
        public DateTime LastUpdatedDate { set; get; }
        public int LastUpdatedBy { set; get; }
        public short IsDeleted { set; get; }
        public virtual ROLES Role { get; set; }
    }
}
