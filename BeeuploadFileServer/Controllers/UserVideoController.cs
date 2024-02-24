using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeeuploadFileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserVideoController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserVideoController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("[action]")]
        [RequestFormLimits(ValueCountLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> uploadUserVideo(long userid, long videoid, IFormFile videofile)
        {
            try
            {
                if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\video\\")))
                {
                    if (System.IO.Path.GetExtension(videofile.FileName) == ".mp4")
                    {
                        if (!System.IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\video\\" + videoid + ".mp4")))
                        {
                            string dirPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\video\\");
                            using (var stream = System.IO.File.Create(dirPath + videoid + System.IO.Path.GetExtension(videofile.FileName)))
                            {
                                await videofile.CopyToAsync(stream);
                            }
                            return Ok();
                        }
                        else
                        {
                            return Conflict("Error: Video ya existe.");
                        }
                    }
                    else
                    {
                        return BadRequest("Error: Solo se pudeden subir videos en formato .MP4.");
                    }
                }
                else
                {
                    return NotFound("Error: Usuario no existe o no tiene carpeta.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido subir el video.");
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> deleteUserVideo(long userid, long videoid)
        {
            try
            {
                if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\video\\")))
                {
                    if (System.IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\video\\" + videoid + ".mp4")))
                    {
                        System.IO.File.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\video\\" + videoid + ".mp4"));
                        return Ok();
                    }
                    else if (System.IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\video\\" + videoid + ".mp4")))
                    {
                        System.IO.File.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\video\\" + videoid + ".mp4"));
                        return Ok();
                    }
                    else
                    {
                        return NotFound("Error: Video no existe.");
                    }
                }
                else
                {
                    return NotFound("Error: Usuario no existe o no tiene carpeta.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido eliminar el video.");
            }
        }

    }
}
