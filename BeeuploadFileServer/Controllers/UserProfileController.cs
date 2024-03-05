using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeeuploadFileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserProfileController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPut("[action]")]
        [RequestFormLimits(ValueCountLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> changeUserProfilePic(long userid, IFormFile imgfile)
        {
            try
            {
                if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\")))
                {
                    if (System.IO.Path.GetExtension(imgfile.FileName) == ".jpg")
                    {
                        string dirPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\profilepic.jpg");
                        System.IO.File.Delete(dirPath);
                        using (var stream = System.IO.File.Create(dirPath))
                        {
                            await imgfile.CopyToAsync(stream);
                        }
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Error: Solo se pudeden subir imagenes en formato .JPG");
                    }
                }
                else
                {
                    return NotFound("Error: Usuario no existe o no tiene carpeta.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido cambiar la imagen de perfil.");
            }
        }

    }
}
