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
        public IActionResult createUserFolder(long id)
        {
            if (!System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + id)))
            {
                System.IO.Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + id));
                System.IO.File.Copy(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\0\\profilepic.jpg"), Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + id + "\\profilepic.jpg"));
                System.IO.Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + id + "\\document"));
                System.IO.Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + id + "\\image"));
                System.IO.Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + id + "\\music"));
                System.IO.Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + id + "\\video"));
                return Ok();
            }
            else
            {
                return BadRequest("Error: Usuario ya tiene carpeta.");
            }
        }

        [HttpDelete("[action]")]
        public IActionResult deleteUserFolder(long id)
        {
            if (System.IO.Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + id)))
            {
                foreach (string dir in Directory.GetDirectories(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + id)))
                {
                    string[] files = Directory.GetFiles(dir);
                    foreach (string file in files)
                    {
                        System.IO.File.Delete(file);
                    }
                    System.IO.Directory.Delete(dir, true);
                }
                foreach (string file in Directory.GetFiles(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + id)))
                {
                    System.IO.File.Delete(file);
                }
                System.IO.Directory.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\beeuploadfiles\\" + id), true);
                return Ok();
            }
            else
            {
                return BadRequest("Error: Usuario no existe o no tiene carpeta.");
            }
        }

    }
}
