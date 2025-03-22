using EduFlow.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace EduFlow.Desktop.Integrated.Security;

public static class TokenHandler
{
    public static IdentitySingelton ParseToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenInfo = tokenHandler.ReadJwtToken(token);

        var identity = new IdentitySingelton
        {
            Token = token
        };

        foreach(var claim in tokenInfo.Claims)
        {
            switch(claim.Type)
            {
                case "Id": identity.Id = long.Parse(claim.Value); break;
                case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": identity.Name = claim.Value; break;
                case "PhoneNumber": identity.PhoneNumber = claim.Value; break;
                case "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": identity.Role = Enum.Parse<UserRole>(claim.Value); break;
            }
        }

        return identity;
    }
}
