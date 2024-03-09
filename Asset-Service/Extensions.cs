namespace Asset_Service;

public static class Extensions
{
    public static bool IsNullOrEmpty(this string @this) => string.IsNullOrEmpty(@this);
    public static bool IsNotNullOrEmpty(this string @this) => !IsNullOrEmpty(@this);
}