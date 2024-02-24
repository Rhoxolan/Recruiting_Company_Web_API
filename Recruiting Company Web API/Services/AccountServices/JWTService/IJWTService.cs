using Microsoft.AspNetCore.Identity;

namespace Recruiting_Company_Web_API.Services.AccountServices
{
    public interface IJWTService
    {
        string GenerateJWTToken(IdentityUser user);
    }

}
