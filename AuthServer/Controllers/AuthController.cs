using Auth.Services;
using Auth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Auth.Data;

namespace AuthServer.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogin login;
        public AuthController(ILogin _login)
        {
            login = _login;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Person personParam)
        {
            Person user = await login.Authenticate(personParam.Ad, personParam.Sifre);

            if (user == null)
                return BadRequest();
            return Ok(user);
        }

        [Authorize(Roles =Role.Admin)]
        [HttpGet]
        public IActionResult GetLogins()
        {
            IQueryable<Person> model = login.GetLogins;
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogin(int id)
        {
            var model = await login.GetLogin(id);

            if (model == null)
            {
                return NotFound();
            }

            // only allow admins to access other user records
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Role.Admin))
            {
                return Forbid();
            }

            return Ok(model);
        }
    }
}