using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.Models
{
    public class icsmvaDBContext:DbContext
    {
        public icsmvaDBContext(DbContextOptions<icsmvaDBContext> option) : base(option)
        {

        }
        public virtual DbSet<USERS> USERS { get; set; }
        public virtual DbSet<PRIVILEGES> PRIVILEGES { get; set; }
        public virtual DbSet<ROLES> ROLES { get; set; }
        public virtual DbSet<ROLE_PRIVILEGES> ROLE_PRIVILEGES { get; set; }
        public virtual DbSet<NA_APPLICATIONS> NA_APPLICATIONS { get; set; }
    }
}
