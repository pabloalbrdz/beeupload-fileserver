using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace BeeuploadFileServer.jwt
{
    public class JWTService
    {

        public static bool verifyUserToken(long userid, string token) 
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var claim = jwt.Claims.FirstOrDefault();
            long value = (long)Convert.ToDouble(claim.Value);
            if (value == userid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
