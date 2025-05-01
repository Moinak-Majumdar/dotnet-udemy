using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_auth.context;
using user_auth.Dto;
using user_auth.models;

namespace user_auth.controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController(IConfiguration config) : ControllerBase
    {
        private readonly DapperContext _dapper = new(config);

        [HttpGet("MyPosts")]
        public async Task<IActionResult> MyPostsAsync()
        {
            string sql = @"
                Select [PostId], [PostTitle], [PostContent], [CreatedOn], [UpdatedOn] 
                From TblPosts Where IsDeleted = 0 AND " +
                "UserId = " + User.FindFirst("UserId")?.Value;

            IEnumerable<Post> posts = await _dapper.LoadData<Post>(sql);

            return Ok(new { response = posts });
        }

        [HttpPost("NewPost")]
        public async Task<IActionResult> CreatePost(PostAddDto p)
        {
            string sql = @"
                Insert Into TblPosts [UserId], [PostTitle], [PostContent]
                Values (@UserId, @PostTitle, PostContent);
            ";

            object values = new
            {
                UserId = User.FindFirst("UserId")?.Value,
                p.PostTitle,
                p.PostContent,
            };

            bool status = await _dapper.ExecuteWithParams(sql, param: values);

            if (status)
            {
                return Ok(new { message = "Post created successfully !" });
            }
            else
            {
                return BadRequest(new { errorMessage = "Failed to create post" });
            }

        }

    }
}