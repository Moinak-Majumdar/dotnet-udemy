using hello_api.context;
using hello_api.Dto;
using hello_api.models;
using Microsoft.AspNetCore.Mvc;

namespace hello_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IConfiguration config) : ControllerBase
{
    private readonly DapperContext _dapper = new(config);

    [HttpGet("getAll")]
    public List<User> GetAllUser()
    {
        string sql = "select * from  TblUsers";

        List<User> users = _dapper.LoadData<User>(sql).ToList();

        return users;
    }


    [HttpGet("getById/{userId}")]
    public User GetUser(long userId)
    {
        string sql = "select * from TblUsers where UserId =" + userId;

        User u = _dapper.LoadDataSingle<User>(sql);

        return u;
    }


    [HttpPost("addUser")]
    public IActionResult AddUser(AddUserDto u)
    {
        string sql = @"Insert into TblUsers (FirstName, LastName, Email, Gender, IsActive) values"
            + "('" + u.FirstName + "', '" + u.LastName + "', '" + u.Email + "', '" + u.Gender + "', '" + u.IsActive + "');";

        // Console.WriteLine(sql);

        bool res = _dapper.ExecuteSql(sql);

        if (res)
        {
            return Ok();
        }

        throw new Exception("Unable to create user");
    }

    [HttpPost("updateUser")]
    public IActionResult UpdateUser(User u)
    {
        string sql = @"Update TblUsers set " +
        "FirstName = '" + u.FirstName + "', " +
        "LastName= '" + u.LastName + "', " +
        "Email= '" + u.Email + "', " +
        "Gender= '" + u.Gender + "', " +
        "IsActive= '" + u.IsActive + "'" +
        "Where UserId = " + u.UserId;

        bool res = _dapper.ExecuteSql(sql);

        if (res)
        {
            return Ok();
        }

        throw new Exception("Unable to update user");

    }
}
