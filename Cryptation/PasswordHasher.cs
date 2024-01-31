using Microsoft.CodeAnalysis.Scripting;
using BCrypt.Net;

namespace TenderAPI.Cryptation;
public class PasswordHasher
{
    private static readonly string pepper;

    static PasswordHasher()
    {
        try
        {
            pepper = Environment.GetEnvironmentVariable("PEPPER");

            if (pepper == null)
            {
                throw new InvalidOperationException("La variabile d'ambiente PEPPER non è impostata.");
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public static Dictionary<string, string> HashPasswordWithPepper(string password, string? userSalt = null)
    {
        try
        {
            string salt;

            if (userSalt == null)
                salt = BCrypt.Net.BCrypt.GenerateSalt();
            else
                salt = userSalt;

            string pepperedPassword = pepper + salt + password;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(pepperedPassword, salt);

            return new Dictionary<string, string>
            {
                { "HashedPassword", hashedPassword },
                { "Salt", salt }
            };
        }
        catch (Exception)
        {

            throw;
        }
    }
}

