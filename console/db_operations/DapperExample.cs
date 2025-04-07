using db_operations.Db;
using db_operations.Models;
using Microsoft.Extensions.Configuration;

namespace db_operations
{
    class DapperExample(IConfiguration config)
    {
        readonly UseDapper dapper = new(config);

        // hl1  add using dapper sql statement.
        static readonly Computer c1 = new()
        {
            CPUCores = 6,
            HasLTE = true,
            HasWifi = true,
            Motherboard = "PRO X670-P WIFI",
            Price = 200000m,
            ReleaseDate = DateTime.Now,
            VideoCard = "RTX 4070 Ti"
        };
        string insert_str = @"Insert into dbo.TblComputer (
                Motherboard,
                CPUCores,
                HasWifi,
                HasLTE,
                ReleaseDate,
                Price,
                VideoCard
            ) values ('" + c1.Motherboard
                + "','" + c1.CPUCores
                + "','" + c1.HasWifi
                + "','" + c1.HasLTE
                + "','" + c1.ReleaseDate
                + "','" + c1.Price
                + "','" + c1.VideoCard
            + "')";
        // bool success = dapper.ExecuteSql(insert_str);


        // hl1  update using dapper sql statement.
        string update_str = @"update dbo.TblComputer set Price =" + 130000m + " where ComputerId=" + 1;
        // Console.WriteLine(dapper.ExecuteSql(update_str));

        // hl1  dapper query execute.
        // Computer c2 = dapper.LoadDataSingle<Computer>("Select top 1 * from tblComputer order by ComputerId desc");
        // Helper.Print(c2);
        // IEnumerable<Computer> cl = dapper.LoadData<Computer>("Select * from dbo.TblComputer");
        // Helper.PrintMany(cl);

    }
}