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
    public object GetUser(long userId)
    {
        string sql = @"
            select 
                u.UserId, [FirstName], [LastName], [Email], [Gender], [IsActive], 
                j.Department as Department,  j.JobTitle as JobTitle,  j.Salary as Salary,
                [IsDeleted] 
            from TblUsers as U
            join TblJobInfo as J on u.UserId = j.UserId
            where u.UserId = " + userId;

        object u = _dapper.LoadDataSingle<object>(sql);

        return u;
    }


    [HttpPost("addUser")]
    public IActionResult AddUser(AddUserDto u)
    {
        string sql = @"
        DECLARE @UserId BIGINT;

        INSERT INTO TblUsers (FirstName, LastName, Email, Gender, IsActive)
        VALUES (@FirstName, @LastName, @Email, @Gender, @IsActive);

        SET @UserId = SCOPE_IDENTITY();

        INSERT INTO TblJobInfo (UserId, Department, JobTitle, Salary)
        VALUES (@UserId, @Department, @JobTitle, @Salary);
    ";

        var parameters = new
        {
            u.FirstName,
            u.LastName,
            u.Email,
            u.Gender,
            u.IsActive,
            u.Department,
            u.JobTitle,
            u.Salary
        };

        bool res = _dapper.ExecuteWithParams(sql, parameters);

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
