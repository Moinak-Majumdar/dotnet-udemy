using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace db_operations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string db_uri = "Server=MOINAK05;Database=DotNetCourseDatabase;TrustServerCertificate=true;Trusted_Connection=true";

            IDbConnection dbConnection = new SqlConnection(db_uri);

            DateTime rightNow = dbConnection.QuerySingle<DateTime>("Select getdate()");

            Console.WriteLine(rightNow);
        }
    }
}