using System.Security.Cryptography;
using System.Text;

namespace Common.Extensions;

public static class HashExtensions
{
    public static string Sha256(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }

    public static string Salt(this int maxSaltLength)
    {
        var salt = new byte[maxSaltLength];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetNonZeroBytes(salt);
        }
        return Convert.ToBase64String(salt);
    }

    public static string Sha256(this string input, string salt)
    {
        return Sha256(salt + input);
    }
}
