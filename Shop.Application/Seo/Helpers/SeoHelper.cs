using System.Text;

namespace Shop.Application.Seo.Helpers
{
    public static class SeoHelper
    {
        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }
        public static string ToSeoFriendly(string name)
        {
            bool prevdash = false;

            var sb = new StringBuilder();

            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (c >= 'a' && c <= 'z' || c >= '0' && c <= '9')
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    sb.Append('-');
                    prevdash = true;
                }
                else if (c >= 128)
                {
                    sb.Append(RemapInternationalCharToAscii(c));
                    prevdash = false;
                }
            }

            if (prevdash == true) return sb.ToString().Substring(0, sb.Length - 1);

            return sb.ToString();
        }
        public static string GetIncrementedUrl(string url)
        {
            var parts = url.Split('-');
            var lastProtion = parts.LastOrDefault();
            bool incExisting;
            if (int.TryParse(lastProtion, out int numToInc))
            {
                incExisting = true;
            }
            else
            {
                incExisting = false;
                numToInc = 1;
            }

            var fragToKeep = incExisting ? string.Join("-", parts.Take(parts.Length - 1).ToArray()) : url;

            return fragToKeep + "-" + (numToInc + 1).ToString();
        }
        public static async Task<string> SeekUrlAsync(string name, Func<string, Task<bool>> uniqueCheckAsync)
        {
            while (!await uniqueCheckAsync(name))
            {
                name = GetIncrementedUrl(name);
            }

            return name;
        }
    }
}
