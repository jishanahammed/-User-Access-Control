using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.Models
{
    public class APPLICATIONNAME
    {
        [Key]
        public int ApplicationId { get; set; }    
        public string Applicationname{ get; set; }    
        public DateTime CreationDate { set; get; }
        public int CreatedBy { set; get; }
        public DateTime LastUpdatedDate { set; get; }
        public int LastUpdatedBy { set; get; }
        public short IsDeleted { set; get; }
    }
}
