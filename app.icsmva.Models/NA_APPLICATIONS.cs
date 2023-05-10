using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.Models
{
    public class NA_APPLICATIONS
    {
        [Key]
        public int ApplicationID { get; set; }    
        public string ApplicationName { get; set; }    
        public string ApplicationFullName { get; set; }    
        public DateTime CreationDate { set; get; }
        public int CreatedBy { set; get; }
        public DateTime LastUpdatedDate { set; get; }
        public int LastUpdatedBy { set; get; }
        public DateTime? IsDeleted { set; get; }
    }
}
