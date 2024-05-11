using BeeuploadFileServer.jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeeuploadFileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDocumentController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserDocumentController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("[action]")]
        [RequestFormLimits(ValueCountLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> uploadUserDocument(long userid, long docid, IFormFile docfile)
        {
            try
            {
                String token = Request.Headers["Auth"];
                bool authorized = JWTService.verifyUserToken(userid, token);
                if (authorized)
                {
                    if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\document\\")))
                    {
                        if (System.IO.Path.GetExtension(docfile.FileName) == ".pdf")
                        {
                            if (!System.IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\document\\" + docid + ".pdf")))
                            {
                                string dirPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\document\\");
                                using (var stream = System.IO.File.Create(dirPath + docid + System.IO.Path.GetExtension(docfile.FileName)))
                                {
                                    await docfile.CopyToAsync(stream);
                                }
                                return Ok();
                            }
                            else
                            {
                                return Conflict("Error: Documento ya existe.");
                            }
                        }
                        else
                        {
                            return BadRequest("Error: Solo se pudeden subir documentos en formato .PDF.");
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
                return StatusCode(500, "Error: No se ha podido subir el documento.");
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> deleteUserDocument(long userid, long docid)
        {
            try
            {
                String token = Request.Headers["Auth"];
                bool authorized = JWTService.verifyUserToken(userid, token);
                if (authorized)
                {
                    if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\document\\")))
                    {
                        if (System.IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\document\\" + docid + ".pdf")))
                        {
                            System.IO.File.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\document\\" + docid + ".pdf"));
                            return Ok();
                        }
                        else
                        {
                            return NotFound("Error: Documento no existe.");
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
                return StatusCode(500, "Error: No se ha podido eliminar el documento.");
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> deleteAllUserDocuments(long userid)
        {
            try
            {
                String token = Request.Headers["Auth"];
                bool authorized = JWTService.verifyUserToken(userid, token);
                if (authorized)
                {
                    if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\document\\")))
                    {
                        string[] files = Directory.GetFiles(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\document\\"));
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
                return StatusCode(500, "Error: No se ha podido eliminar el documento.");
            }
        }

    }
}
