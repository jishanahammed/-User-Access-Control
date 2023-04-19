using app.icsmva.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.Application
{
    public class ApplicationService : IApplicationName
    {
        private readonly icsmvaDBContext db;
        public ApplicationService(icsmvaDBContext db)
        {
            this.db = db;
        }
        public List<APPLICATIONNAME> Getlist()
        {
            List <APPLICATIONNAME> list = db.APPLICATIONNAME.Where(f=>f.IsDeleted==1).ToList(); 
          return list;
        }
    }
}
