using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeeuploadFileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFolderController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserFolderController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> createUserFolder(long userid)
        {
            try
            {
                if (!System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid)))
                {
                    System.IO.Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid));
                    System.IO.File.Copy(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\0\\profilepic.jpg"), Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\profilepic.jpg"));
                    System.IO.Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\document"));
                    System.IO.Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\image"));
                    System.IO.Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\music"));
                    System.IO.Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid + "\\video"));
                    return Ok();
                }
                else
                {
                    return Conflict("Error: Usuario ya tiene carpeta.");
                }
            }
            catch (Exception ex) {
                return StatusCode(500, "Error: No se ha podido crear la carpeta de usuario.");
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> deleteUserFolder(long userid)
        {
            try
            {
                if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid)) && userid != 0)
                {
                    foreach (string dir in System.IO.Directory.GetDirectories(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid)))
                    {
                        string[] files = Directory.GetFiles(dir);
                        foreach (string file in files)
                        {
                            System.IO.File.Delete(file);
                        }
                        System.IO.Directory.Delete(dir, true);
                    }
                    foreach (string file in System.IO.Directory.GetFiles(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid)))
                    {
                        System.IO.File.Delete(file);
                    }
                    System.IO.Directory.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + userid), true);
                    return Ok();
                }
                else
                {
                    return BadRequest("Error: Usuario no existe o no tiene carpeta.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido eliminar la carpeta de usuario.");
            }
        }

    }
}
