using Microsoft.AspNetCore.Identity;

namespace Recruiting_Company_Web_API.Services.AuthenticationServices.JWTService
{
    public interface IJWTService
    {
        string GenerateJWTToken(IdentityUser user);
    }

}
