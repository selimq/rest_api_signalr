using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Net;

namespace LoginWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {


        public static IHostingEnvironment _environment;
        public UploadController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public class FileUpload
        {
            public IFormFile files { get; set; }
        }

        [HttpPost]
        public IActionResult UploadImage([FromForm] FileUpload file)
        {
            if (file.files.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + "file"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "file");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "file/" + file.files.FileName))
                    {
                        file.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return Ok(file.files);
                    }
                }
                catch (System.Exception)
                {
                    return BadRequest();
                    throw;
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /*  [HttpGet("{fileName}")]
          public HttpResponseMessage DownloadFile(string fileName)
          {
              if (!string.IsNullOrEmpty(fileName))
              {
                  string fullPath = _environment.WebRootPath + "file/" + fileName;
                  string fullPath1 = _environment.WebRootPath + "file/";

                  if (Directory.Exists(fullPath1))
                  {
                      HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                      var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                      var fileMemStream = new MemoryStream(fileBytes);
                      response.Content = new StreamContent(fileMemStream);
                      var list = fileName.Split('.');
                      String type = "application/" + list[list.Length - 1];
                      response.Content.Headers.ContentType = new MediaTypeHeaderValue(type);
                      response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                      response.Content.Headers.ContentDisposition.FileName = fileName;
                      response.Content.Headers.ContentLength = fileMemStream.Length;
                      return response;
                  }
              }

              return new HttpResponseMessage(HttpStatusCode.NotFound);
          }

  */
        [HttpGet("{fileName}")]
        // [Route("stream")]
        public IActionResult DownloadPdfFile(string fileName)
        {
            string fullPath = _environment.WebRootPath + "file/" + fileName;
            var list = fileName.Split('.');
            String type = "application/" + list[list.Length - 1];
            Stream stream = System.IO.File.OpenRead(path: fullPath);


            return new FileStreamResult(stream, type)
            {
                FileDownloadName = fileName
            };
        }
        [HttpDelete("{fileName}")]
        public IActionResult DeleteFile(string fileName)
        {
            string fullPath = _environment.WebRootPath + "file/" + fileName;
            try
            {
                System.IO.File.Delete(path: fullPath);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(error:e);
            }

        }

    }

}