using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.Data;
using Login.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LoginWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogin login;
        private readonly IConnection connection;
        public UsersController (ILogin _login,IConnection _connection)
        {
            login = _login;
            connection = _connection;

        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] Person personParam)
        {
            String token = login.Authenticate(personParam.Mail);

            if (token == null)
                return BadRequest();
            return Ok(token);
        }
        [HttpGet]
        public  IActionResult GetLogins()
        {
            IQueryable<Person> model = login.GetLogins;
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }
        [Route("getconnection/{userName}")]
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetConnection(String userName)
        {
            Connection user = await connection._GetConnection(userName);
            
            return Ok(user);
        }
    }
}
//lsof -i:50005