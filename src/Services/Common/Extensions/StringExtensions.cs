using System.Diagnostics;

namespace Common.Extensions;

public static class StringExtensions
{
    [DebuggerStepThrough]
    public static string ReplaceRecursive(this string value, string pattern, string val = "")
    {
        while (value.Contains(pattern))
        {
            value = value.Replace(pattern, val);
        }

        return value;
    }
}
