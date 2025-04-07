
using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace db_operations.Db
{

    public class UseDapper(IConfiguration c)
    {
        private string _db_uri = c.GetConnectionString("default");
   

        public IEnumerable<T> LoadData<T>(string sql)
        {
            DbConnection con = new SqlConnection(_db_uri);
            return con.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            DbConnection con = new SqlConnection(_db_uri);
            return con.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            DbConnection con = new SqlConnection(_db_uri);
            return con.Execute(sql) > 0;
        }
    }
}