using BeeuploadFileServer.jwt;
using Microsoft.AspNetCore.Mvc;

namespace BeeuploadFileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        public AuthController()
        {
            
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> verifyUserToken(long userid, string token)
        {
            try
            {
                bool value = JWTService.verifyUserToken(userid, token);
                if (value)
                {
                    return Ok(true);
                }
                else
                {
                    return Unauthorized("Error: No tienes permiso para esta accion.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: No se ha podido verificar el token.");
            }
        }

    }
}
