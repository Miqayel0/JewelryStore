namespace Common.Extensions;

public static class ReflectionExtension
{
    public static object GetPropertyValue(this object src, string propName)
    {
        return src.GetType().GetProperty(propName)?.GetValue(src, null);
    }
}
