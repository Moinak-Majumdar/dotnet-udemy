using db_operations.Db;
using db_operations.Models;
using Microsoft.Extensions.Configuration;

namespace db_operations
{
    class EntityExample(IConfiguration config)
    {
        readonly Entity ef = new(config);

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

        // hl2 add using entity framework.
        // ef.Add(c1);
        // ef.SaveChanges();
    }
}