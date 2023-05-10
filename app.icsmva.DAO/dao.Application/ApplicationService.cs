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
        public List<NA_APPLICATIONS> Getlist()
        {
            List <NA_APPLICATIONS> list = db.NA_APPLICATIONS.Where(f=> (DateTime)f.IsDeleted==null).ToList(); 
          return list;
        }
    }
}
