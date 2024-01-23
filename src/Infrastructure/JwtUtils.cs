using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure
{
    public class JwtUtils
    {
        public static long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = handler.ReadJwtToken(token);

            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;

            var ticks = long.Parse(tokenExp);

            return ticks;
        }

        public static bool CheckTokenIsValid(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var tokenTicks = GetTokenExpirationTime(token);

                var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

                var now = DateTime.Now.ToUniversalTime();

                var valid = tokenDate >= now;

                return valid;
            }

            return false;
        }
    }
}
