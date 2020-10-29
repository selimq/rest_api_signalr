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
        public UsersController (ILogin _login)
        {
            login = _login;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Person personParam)
        {
            Person user =await login.Authenticate(personParam.Ad, personParam.Sifre);

            if (user == null)
                return BadRequest();
            return Ok(user);
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
    }
}
