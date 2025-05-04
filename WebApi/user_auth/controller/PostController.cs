using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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

        [HttpPost("ExecuteProcedure/{mode}")]
        public async Task<IActionResult> ExecuteProcedure(string mode, [FromBody] Dictionary<string, dynamic> json)
        {
            int m = int.Parse(mode);

            // Console.WriteLine("Json ===>" + json);

            JsonOutput op = await _dapper.ExecuteSp(spName: "spTblPosts", mode: m, user: User, json);

            if (op.Id == 1)
            {
                return StatusCode(op.StatusCode, new { response = op.Response });
            }
            else
            {
                return BadRequest(new { errorMessage = op.Response });
            }
        }


        [HttpGet("MyPosts")]
        public async Task<IActionResult> MyPostsAsync()
        {
            string sql = @"
                Select [PostId], [PostTitle], [PostContent], [CreatedOn], [UpdatedOn] 
                From TblPosts Where IsDeleted = 0 AND " +
                "UserId = " + User.FindFirst("UserId")?.Value;

            IEnumerable<Post> posts = await _dapper.LoadData<Post>(sql);

            List<object> response = [];

            foreach (Post p in posts)
            {
                response.Add(new
                {
                    p.PostId,
                    p.PostTitle,
                    p.PostContent,
                    p.CreatedOn,
                });
            }

            return Ok(new { response });
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