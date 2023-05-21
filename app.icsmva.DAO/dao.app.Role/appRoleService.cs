using app.icsmva.DAO.dao.userRole;
using app.icsmva.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.dao.app.Role
{
    public class appRoleService : IAppRole
    {
        private readonly IConfiguration configuration;
        public appRoleService(IConfiguration configuration)
        {
            this.configuration = configuration;
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
            Providername = "System.Data.sqlClient";
        }
        public string ConnectionString { get; }
        public string Providername { get; }
        public IDbConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
        }

        public string AddRecode(UserRoleViewModel model)
        {
            ROLES users = new ROLES();
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var Role = dbConnection.Query<ROLES>("ROLES_Create",
                        new
                        {
                            RoleName = model.RoleName,
                            ApplicationName = model.ApplicationName,
                            Remarks = model.Remarks,
                        }, commandType: CommandType.StoredProcedure);
                    if (Role != null)
                    {
                        result = "successfilly";

                    }
                    dbConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message.ToString();
                return errorMsg.ToString();
            }
        }

        public ROLES DeleteRecode(int id)
        {
            throw new NotImplementedException();
        }

        public List<ROLES> GetUsersRolesList()
        {
            throw new NotImplementedException();
        }

        public string UpdateRecode(UserRoleViewModel model)
        {
            ROLES users = new ROLES();
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var Role = dbConnection.Query<ROLES>("ROLES_Update",
                        new
                        {
                            RoleID=model.RoleID,
                            RoleName = model.RoleName,
                            Remarks = model.Remarks,
                        }, commandType: CommandType.StoredProcedure);
                    if (Role != null)
                    {
                        result = "successfilly";

                    }
                    dbConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message.ToString();
                return errorMsg.ToString();
            }
        }
    }
}
