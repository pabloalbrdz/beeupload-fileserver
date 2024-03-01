using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeeuploadFileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMusicController : ControllerBase
    {


        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserMusicController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("[action]")]
        [RequestFormLimits(ValueCountLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> uploadUserMusic(long userid, long musicid, IFormFile musicfile)
        {
            try
            {
                if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\music\\")))
                {
                    if (System.IO.Path.GetExtension(musicfile.FileName) == ".mp3")
                    {
                        if (!System.IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\music\\" + musicid + ".mp3")))
                        {
                            string dirPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\music\\");
                            using (var stream = System.IO.File.Create(dirPath + musicid + System.IO.Path.GetExtension(musicfile.FileName)))
                            {
                                await musicfile.CopyToAsync(stream);
                            }
                            return Ok();
                        }
                        else
                        {
                            return Conflict("Error: Musica ya existe.");
                        }
                    }
                    else
                    {
                        return BadRequest("Error: Solo se pudede subir musica en formato .MP3.");
                    }
                }
                else
                {
                    return NotFound("Error: Usuario no existe o no tiene carpeta.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido subir la musica.");
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> deleteUserMusic(long userid, long musicid)
        {
            try
            {
                if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\music\\")))
                {
                    if (System.IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\music\\" + musicid + ".mp3")))
                    {
                        System.IO.File.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\music\\" + musicid + ".mp3"));
                        return Ok();
                    }
                    else
                    {
                        return NotFound("Error: Musica no existe.");
                    }
                }
                else
                {
                    return NotFound("Error: Usuario no existe o no tiene carpeta.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido eliminar la musica.");
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> deleteAllUserMusic(long userid)
        {
            try
            {
                if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\music\\")))
                {
                    string[] files = Directory.GetFiles(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\music\\"));
                    foreach (string file in files)
                    {
                        System.IO.File.Delete(file);
                    }
                    return Ok();
                }
                else
                {
                    return NotFound("Error: Usuario no existe o no tiene carpeta.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido eliminar la musica.");
            }
        }

    }
}
