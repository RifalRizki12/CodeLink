using API.Contracts;
using API.DTOs.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Utilities.Handler
{
    public class TokenHandler : ITokenHandlers
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Generate(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTService:SecretKey"]));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(issuer: _configuration["JWTService:Issuer"],
                audience: _configuration["JWTService:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials);
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return encodedToken;
        }

        public ClaimsDto ExtractClaimsFromJwt(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new ClaimsDto();
            }

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWTService:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWTService:Issuer"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTService:SecretKey"]))
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

                if (claimsPrincipal.Identity is ClaimsIdentity identity)
                {
                    var claims = new ClaimsDto
                    {
                        EmployeeGuid = Guid.Parse(identity.FindFirst("EmployeeGuid")?.Value ?? ""),
                        FullName = identity.FindFirst("Fullname")?.Value,
                        Email = identity.FindFirst("Email")?.Value,
                        Foto = identity.FindFirst("Foto")?.Value,
                        StatusAccount = identity.FindFirst("StatusAccount")?.Value,
                        AverageRating = identity.FindFirst("AverageRating")?.Value,
                    };

                    var roles = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(claim => claim.Value).ToList();
                    claims.Role = roles;

                    return claims;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during token validation and extraction
                Console.WriteLine("Error extracting claims from JWT: " + ex.Message);
            }

            return new ClaimsDto();
        }
    }
}
