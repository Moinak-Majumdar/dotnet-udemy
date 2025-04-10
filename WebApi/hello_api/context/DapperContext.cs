using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;

namespace hello_api.context
{

    class DapperContext(IConfiguration config)
    {
        private readonly string _dbCon = config.GetConnectionString("DefaultConnection");

        public IEnumerable<T> LoadData<T>(string sql)
        {
            DbConnection con = new SqlConnection(_dbCon);
            return con.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            DbConnection con = new SqlConnection(_dbCon);
            return con.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            DbConnection con = new SqlConnection(_dbCon);
            return con.Execute(sql) > 0;
        }
    }

}