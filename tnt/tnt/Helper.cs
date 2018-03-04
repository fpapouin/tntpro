using System.Text.RegularExpressions;

namespace tnt
{
    class Helper
    {
        public static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).
            Replace("\\*", ".*").
            Replace("\\?", ".") + "$";
        }

        public static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"([{0}']*\.+$)|([{0}']+)", invalidChars);
            return Regex.Replace(name, invalidReStr, "_");
        }

        public static string GetToTheEndOfLine(string value, string find)
        {
            int indexStart = value.IndexOf(find) + find.Length;
            int indexStop = value.IndexOf("\r\n", indexStart);
            return value.Substring(indexStart, indexStop - indexStart);
        }
    }
}
