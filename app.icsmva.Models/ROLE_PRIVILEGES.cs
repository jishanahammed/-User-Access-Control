using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.Models
{
    public class ROLE_PRIVILEGES
    {
        [Key]
        public long RolePrivilegeID { get; set; }
        public int RoleID { set; get; }
        public string PrivilegeID { set; get; }
        public DateTime CreationDate { set; get; }
        public int CreatedBy { set; get; }
        public DateTime LastUpdatedDate { set; get; }
        public int LastUpdatedBy { set; get; }
        public DateTime? IsDeleted { set; get; }
        public virtual ROLES Role { get; set; }
    }
}
