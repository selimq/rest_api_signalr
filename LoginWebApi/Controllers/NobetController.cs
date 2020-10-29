using Login.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Login.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace LoginWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NobetController : ControllerBase
    {
        private readonly INobet _nobet;
        protected HttpClient ClientFireBase;
        public NobetController(INobet nobet)
        {
            _nobet = nobet;
            ClientFireBase = StartFireBase();
        }
        private HttpClient StartFireBase()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://fcm.googleapis.com");
            return client;
        }
        [HttpPost]
        public async Task<IActionResult> Save ([FromBody] Nobet nobet)
        {
            if (nobet == null)
            {
                return BadRequest();
            }
            POJO model = await _nobet.Save(nobet);
            if (model == null)
            {
                return NotFound();
            }
            else
            {
                await _nobet.Save(nobet);
                var body = new
                {
                    notification = new
                    {
                        body = $"Yeni Nobet: Yer=> {nobet.Nobet_Yer} - Zaman=> {nobet.Nobet_Zaman} - Id => {nobet.Id} ",
                        title = "Yeni nöbet eklendi"
                     
                    },
                    priority = "high",
                    sound = "1",
                    data = new
                    {
                        clickaction = "FLUTTERNOTIFICATIONCLICK",
                        id = "1",
                        status = "done",
                      
                    },
                    to = "/topics/all"
                };

                ClientFireBase.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("key", "=AAAAOc8JSBI:APA91bEf1aV5MtUfI_fjM5cGm1l6lJZ0rMFZB9-titReWuGk7gQjHXfo47pw8W1qbimyLAvwzmd8hSPlqvqC8WmHESJOp_SUzjrB_iDet3rgnwCgn8k6OUIwyCgZMXRzAt_1Do_KHX6v");
                var response = await ClientFireBase.PostAsync("fcm/send", new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"));
                var retFireBase = await response.Content.ReadAsStringAsync();


                return Ok(model);
            }
        }
        [Route ("nobetler/{id}")]
        [HttpGet("id")]
        public async Task<ActionResult> GetNobetsWithId (int id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            List<Nobet> model = await _nobet.GetNobetsWithId(id);
            if(model==null)
            {
                return NotFound();
            }
            return Ok(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetLogins()
        {
            IQueryable<Nobet> model = _nobet.GetNobets;
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }
    }
}
