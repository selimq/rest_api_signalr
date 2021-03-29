using System.Threading.Tasks;
using Login.Data;
using Login.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginWebApi.Controllers
{
 //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly IPhoto _photo;
        public PhotoController(IPhoto photo)
        {
            _photo = photo;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Photo photo)
        {
            if (photo == null)
            {
                return BadRequest();

            }
            POJO model = await _photo.SavePhoto(photo);
            if (model.Flag == false)
            {
                return NotFound(model);
            }
            else
            {
                await _photo.SavePhoto(photo);
                return Ok(photo);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                return BadRequest();

            }
            Photo photo = await _photo.GetPhoto(id);
            return Ok(photo);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();

            }
            POJO pojo = await _photo.DeletePhoto(id);
            if (pojo.Flag == false)
            {
                return NotFound(pojo);
            }
            else
            {
                return Ok(pojo);

            }

        }
    }
}