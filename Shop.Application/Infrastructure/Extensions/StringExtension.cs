namespace Shop.Application.Infrastructure.Extensions
{
    public static class StringExtension
    {
        private readonly static char[] _invalidFileNameChars = Path.GetInvalidFileNameChars().Concat(new[] { '&' }).ToArray();

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

        public static string NormalizeFileName(this string value)
        {
            return string.Join('_', value.Split(_invalidFileNameChars));
        }
    }
}
