using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.Models
{
    public class ROLESPRIVILEGESMAP
    {
        [Key]
        public int RPMapId { get; set; }
        public int RoleID { set; get; }
        public int PrivilegeID { set; get; }
        public DateTime CreationDate { set; get; }
        public int CreatedBy { set; get; }
        public DateTime LastUpdatedDate { set; get; }
        public int LastUpdatedBy { set; get; }
        public short IsDeleted { set; get; }
    }
}
