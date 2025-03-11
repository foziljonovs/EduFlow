namespace EduFlow.BLL.Common.Security;

public static class PasswordHelper
{
    public static (string Hash, string Salt) Hash(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);

        return (Hash: hash, Salt: salt);
    }

    public static bool Verify(string password, string hash, string salt)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
