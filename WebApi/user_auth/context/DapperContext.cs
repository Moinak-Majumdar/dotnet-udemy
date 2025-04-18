using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace user_auth.context
{

    class DapperContext(IConfiguration config)
    {
        private readonly string _dbCon = config.GetConnectionString("DefaultConnection");

        public async Task<IEnumerable<T>> LoadData<T>(string sql)
        {
            DbConnection con = new SqlConnection(_dbCon);
            return await con.QueryAsync<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            DbConnection con = new SqlConnection(_dbCon);
            return con.QuerySingle<T>(sql);
        }

        public async Task<bool> ExecuteSql(string sql)
        {
            DbConnection con = new SqlConnection(_dbCon);
            return await con.ExecuteAsync(sql) > 0;
        }

        public async Task<int> ExecuteSqlWithRowCount(string sql)
        {
            DbConnection con = new SqlConnection(_dbCon);
            return await con.ExecuteAsync(sql);
        }

        public async Task<bool> ExecuteWithParams(string sql, object param)
        {
            DbConnection con = new SqlConnection(_dbCon);
            return await con.ExecuteAsync(sql, param) > 0;
        }
    }

}