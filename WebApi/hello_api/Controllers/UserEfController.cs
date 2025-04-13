
using AutoMapper;
using hello_api.context;
using hello_api.Dto;
using hello_api.models;
using Microsoft.AspNetCore.Mvc;

namespace hello_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserEfController : ControllerBase
    {
        readonly EFContext _ef;
        IMapper _mapper;

        public UserEfController(IConfiguration config)
        {
            _ef = new EFContext(config);
            _mapper = new Mapper(new MapperConfiguration(cfg =>{
                cfg.CreateMap<UserDto, User>();
            }));
        }

        [HttpGet("GetUserById/{userId}")]

        public User GetUser(long userId)
        {
            User? user = _ef.Users
                        .Where(u => u.UserId == userId)
                        .FirstOrDefault<User>();

            if (user != null)
            {
                return user;
            }

            throw new Exception("Failed to Get User");
        }

        [HttpGet("getAllUser")]
        public IEnumerable<User> GetAllUser()
        {
            IEnumerable<User> users = [.. _ef.Users];
            return users;
        }


        [HttpPost("updateUser")]
        public IActionResult UpdateUser(User u)
        {
            User? dbUser = _ef.Users
            .Where(user => user.UserId == u.UserId)
            .FirstOrDefault<User>();

            if (dbUser != null)
            {
                dbUser.IsActive = u.IsActive;
                dbUser.FirstName = u.FirstName;
                dbUser.LastName = u.LastName;
                dbUser.Email = u.Email;
                dbUser.Gender = u.Gender;
            }

            if (_ef.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to update user");
        }

        [HttpPost("addUser")]
        public IActionResult AddUser(UserDto u)
        {
            User dbUser = _mapper.Map<User>(u);
            _ef.Add(dbUser);
            if (_ef.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to update user");
        }
    }
}