using Microsoft.AspNetCore.Mvc;

namespace hello_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public UserController()
    {

    }

    [HttpGet("test/{userId}")]
    public string[] Test(string userId)
    {
        string[] users = ["Ram", "Shaym", userId];

        return users;
    }
}
