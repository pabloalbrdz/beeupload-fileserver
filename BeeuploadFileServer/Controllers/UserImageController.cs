﻿using Microsoft.AspNetCore.Http;
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
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido eliminar la imagen.");
            }
        }

    }
}