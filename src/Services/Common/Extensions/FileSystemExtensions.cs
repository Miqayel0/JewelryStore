using System.Reflection;

namespace Common.Extensions;

public static class FileSystemExtensions
{
    public static string ReadEmbeddedResource<T>(this string fileName)
    {
        var assembly = typeof(T).GetTypeInfo().Assembly;
        var name = assembly.GetManifestResourceNames()
            .FirstOrDefault(r => r.EndsWith("." + fileName, StringComparison.InvariantCultureIgnoreCase));
        if (name == null)
        {
            return null;
        }

        using var stream = assembly.GetManifestResourceStream(name);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
