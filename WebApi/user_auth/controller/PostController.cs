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

            try
            {
                DbResponse op = await _dapper.ExecuteSp(spName: "spTblPosts", mode: int.Parse(mode), user: User, json);

                if (op.Id == 1)
                {
                    return StatusCode(op.StatusCode, new { statusCode = op.StatusCode, result = op.Response, isSuccess = true, errorMessage = "" });
                }
                else
                {
                    return BadRequest(new { statusCode = 400, isSuccess = false, errorMessage = op.Response });
                }
            }
            catch (System.Exception)
            {
                return BadRequest(new { statusCode = 400, isSuccess = false, errorMessage = "Internal server error."});
            }
        }

    }
}