using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.Models
{
    public class PRIVILEGES
    {
        [Key]
        public string PrivilegeID { set; get; }
        public string ApplicationName { set; get; }
        public string UserInterfaceName { set; get; }
        public string ActionName { set; get; }
        public short ActionPrecedence { set; get; }
        public string PrivilegeName { set; get; }
        public int Precedence { set; get; }
        public string Remarks { set; get; }
        public DateTime? CreationDate { set; get; }
        public int? CreatedBy { set; get; }
        public DateTime? LastUpdatedDate { set; get; }
        public int? LastUpdatedBy { set; get; }
        public DateTime? IsDeleted { set; get; }
       
    }
}
