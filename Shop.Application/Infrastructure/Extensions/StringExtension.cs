namespace Shop.Application.Infrastructure.Extensions
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string value)
        {
            if (value == null)
                return true;

            return value.Trim().Length == 0;
        }

        public static bool EqualsNoCase(this string value, string other)
        {
            return string.Compare(value, other, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
