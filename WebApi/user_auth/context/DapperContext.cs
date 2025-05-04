using System.Data;
using System.Data.Common;
using System.Security.Claims;
using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using user_auth.models;

namespace user_auth.context
{

    class DapperContext(IConfiguration config)
    {
        private readonly string? _dbCon = config.GetConnectionString("DefaultConnection");

        public async Task<IEnumerable<T>> LoadData<T>(string sql)
        {
            DbConnection con = new SqlConnection(_dbCon);
            return await con.QueryAsync<T>(sql);
        }

        public async Task<T?> LoadDataSingle<T>(string sql)
        {
            DbConnection con = new SqlConnection(_dbCon);
            return await con.QuerySingleOrDefaultAsync<T>(sql);
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

        public async Task<JsonOutput> ExecuteSp(string spName, int mode, ClaimsPrincipal user, Dictionary<string, dynamic> json)
        {
            DbConnection con = new SqlConnection(_dbCon);

            DynamicParameters dynamicParameters = new();

            long userId = long.Parse(user.FindFirst("UserId")?.Value ?? "0");

            dynamicParameters.Add("@Mode", mode);
            dynamicParameters.Add("@UserId", userId);

            string jsonString = JsonConvert.SerializeObject(json);

            dynamicParameters.Add("@json", jsonString);
            dynamicParameters.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
            await con.ExecuteAsync(spName, dynamicParameters, commandType: CommandType.StoredProcedure);

            string output = dynamicParameters.Get<string>("@output");

            // Console.WriteLine(output);

            dynamic? parsedOp = JsonConvert.DeserializeObject(output);

            // Console.WriteLine(serializeOp);

            string res = "Invalid json format";

            if ((parsedOp != null) && parsedOp?.Response is not string)
            {
                res = JsonConvert.SerializeObject(parsedOp!.Response);
            }

            return new JsonOutput()
            {
                Id = parsedOp?.Id ?? 0,
                StatusCode = parsedOp?.StatusCode ?? 400,
                Response = res
            };
        }
    }

}