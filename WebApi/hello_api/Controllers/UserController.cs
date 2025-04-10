using Microsoft.AspNetCore.Mvc;

namespace hello_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IConfiguration config) : ControllerBase
{
    private readonly DapperContext _dapper = new(config);

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("test/{userId}")]
    public string[] Test(string userId)
    {
        string[] users = ["Ram", "Shaym", userId];

        return users;
    }
}
