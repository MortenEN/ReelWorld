using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.SqlServer
{
    public abstract class BaseDao
    {
        public string ConnectionString { get; private set; }
        protected BaseDao(string connectionString) => ConnectionString = connectionString;
        public IDbConnection CreateConnection() => new SqlConnection(ConnectionString);
    }
}
