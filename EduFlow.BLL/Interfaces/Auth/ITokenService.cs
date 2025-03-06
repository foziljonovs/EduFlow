using EduFlow.Domain.Entities.Users;

namespace EduFlow.BLL.Interfaces.Auth;

public interface ITokenService
{
    string GenerateToken(User user);
}
