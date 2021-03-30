using Login.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Login.Data;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace LoginWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _Giris;
        // protected HttpClient ClientFireBase;
        public LoginController(ILogin giris)
        {
            _Giris = giris;
            //ClientFireBase = StartFireBase();
        }

        /* private HttpClient StartFireBase()
         {
             var client = new HttpClient();
             client.BaseAddress = new Uri("https://fcm.googleapis.com");
             return client;
         }*/

        [Authorize]

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Person login)
        {
            if (login == null)
            {
                return BadRequest();
            }
            POJO model = await _Giris.Save(login);
            if (model == null)
            {
                return NotFound();
            }
            else
            {
                await _Giris.Save(login);
                return Ok(model);
            }


        }
        /*  [Route("withmail/{mail}")]
          [HttpGet("{mail}")]
          public async Task<ActionResult> GetLoginWithMail(String mail)
          {
              if (mail.Equals(null))
              {
                  return BadRequest();
              }
              Person model = await _Giris.GetLoginWithMail(mail);
              if (model == null)
              {
                  return NotFound();
              }
              return Ok(model);
          }
          [Route("withmails/{mail}")]
          [HttpGet("{mail}")]
          public async Task<ActionResult> GetLoginsWithMail(String mail)
          {
              if (mail.Equals(null))
              {
                  return BadRequest();
              }
              List<Person> model = await _Giris.GetLoginsWithMail(mail);
              if (model == null)
              {
                  return NotFound();
              }
              return Ok(model);
          }*/
        [Authorize]

        [HttpGet("{id}")]
        public async Task<ActionResult> GetLogin(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Person model = await _Giris.GetLogin(id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult GetLogins()
        {
            IQueryable<Person> model = _Giris.GetLogins;
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }
        [Authorize]

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            POJO model = await _Giris.Delete(id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);

        }
        [Authorize]

        [Route("withmails/")]
        public IActionResult GetId()
        {
            //token decode
            string token = Request.Headers["Authorization"];
            //bearer kısmı olmadan decode etmek gerek
            var _token = token.Replace("Bearer ", "");
            var tokenStr = new JwtSecurityToken(jwtEncodedString: _token);
            //  unique_name kısmı id veriyor
            Console.WriteLine("id => " + tokenStr.Claims.First(c => c.Type == "unique_name").Value);
            var name = tokenStr.Claims.First(c => c.Type == "unique_name").Value;
            return Ok(name);
        }


    }
}