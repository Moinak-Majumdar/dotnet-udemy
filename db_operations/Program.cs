using System.Collections;
using System.Text.Json;
using db_operations.Db;
using db_operations.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace db_operations
{
    public class Program
    {

        private readonly IConfiguration config;

        public Program()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
        public static void Main(string[] args)
        {
            Program p = new();

            Entity ef = new(p.config);

            // DapperExample de = new(p.config);
            // EntityExample ee = new(p.config);

            // hl3 logging.
            IEnumerable<Computer>? cl2 = ef.Computers;
            using StreamWriter writer = new("log.txt", append: false);
            if (cl2 != null)
            {
                // Helper.PrintMany(cl2);
                // Helper.LogWriteMany<Computer>(cl2, writer);
            }

            string computerJson = File.ReadAllText("computer.json");
            // Console.WriteLine(computerJson);

            IEnumerable<Computer>? cl3 = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computerJson);
            if(cl3 != null) {
                // Helper.PrintMany(cl3);
            }

        }

    }
}