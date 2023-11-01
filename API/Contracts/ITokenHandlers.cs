using API.DTOs.Tokens;
using System.Security.Claims;

namespace API.Contracts
{
    public interface ITokenHandlers
    {
        string Generate(IEnumerable<Claim> claims);

        ClaimsDto ExtractClaimsFromJwt(string token);
    }
}
