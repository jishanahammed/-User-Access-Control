using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.Models
{
    public class ROLES
    {
        public ROLES()
        {
            this.USERS = new HashSet<USERS>();
            this.ROLE_PRIVILEGEs = new HashSet<ROLE_PRIVILEGES>();
        }
        [Key]
        public int RoleID { set; get; }
        public string RoleName { set; get; }
        public string ApplicationName { set; get; }
        public string Remarks { set; get; }
        public DateTime CreationDate { set; get; }
        public int CreatedBy { set; get; }
        public DateTime LastUpdatedDate { set; get; }
        public int LastUpdatedBy { set; get; }
        public DateTime? IsDeleted { set; get; }
        public virtual ICollection<USERS> USERS { get; set; }
        public virtual ICollection<ROLE_PRIVILEGES> ROLE_PRIVILEGEs { get; set; }
    }
}
