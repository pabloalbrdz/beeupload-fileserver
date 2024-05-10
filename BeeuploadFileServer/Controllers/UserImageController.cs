using BeeuploadFileServer.jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeeuploadFileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserImageController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserImageController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("[action]")]
        [RequestFormLimits(ValueCountLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> uploadUserImage(long userid, long imgid, IFormFile imgfile)
        {
            try
            {
                String token = Request.Headers["Auth"];
                bool authorized = JWTService.verifyUserToken(userid, token);
                if (authorized)
                {
                    if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\")))
                    {
                        if (System.IO.Path.GetExtension(imgfile.FileName) == ".jpg" || System.IO.Path.GetExtension(imgfile.FileName) == ".png")
                        {
                            if (!System.IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\" + imgid + ".jpg"))
                                && !System.IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\" + imgid + ".png")))
                            {
                                string dirPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\");
                                using (var stream = System.IO.File.Create(dirPath + imgid + System.IO.Path.GetExtension(imgfile.FileName)))
                                {
                                    await imgfile.CopyToAsync(stream);
                                }
                                return Ok();
                            }
                            else
                            {
                                return Conflict("Error: Imagen ya existe.");
                            }
                        }
                        else
                        {
                            return BadRequest("Error: Solo se pudeden subir imagenes en formato .JPG o .PNG.");
                        }
                    }
                    else
                    {
                        return NotFound("Error: Usuario no existe o no tiene carpeta.");
                    }
                }
                else
                {
                    return Unauthorized("Error: No tienes permiso para esta accion.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido subir la imagen.");
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> deleteUserImage(long userid, long imgid)
        {
            try
            {
                String token = Request.Headers["Auth"];
                bool authorized = JWTService.verifyUserToken(userid, token);
                if (authorized)
                {
                    if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\")))
                    {
                        if (System.IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\" + imgid + ".jpg")))
                        {
                            System.IO.File.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\" + imgid + ".jpg"));
                            return Ok();
                        }
                        else if (System.IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\" + imgid + ".png")))
                        {
                            System.IO.File.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\" + imgid + ".png"));
                            return Ok();
                        }
                        else
                        {
                            return NotFound("Error: Imagen no existe.");
                        }
                    }
                    else
                    {
                        return NotFound("Error: Usuario no existe o no tiene carpeta.");
                    }
                }
                else
                {
                    return Unauthorized("Error: No tienes permiso para esta accion.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido eliminar la imagen.");
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> deleteAllUserImages(long userid)
        {
            try
            {
                String token = Request.Headers["Auth"];
                bool authorized = JWTService.verifyUserToken(userid, token);
                if (authorized)
                {
                    if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\")))
                    {
                        string[] files = Directory.GetFiles(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image\\"));
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
                else 
                {
                    return Unauthorized("Error: No tienes permiso para esta accion.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido eliminar las imagenes.");
            }
        }

    }
}
