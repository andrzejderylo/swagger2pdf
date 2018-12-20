using System;
using System.Text;

namespace Swagger2Pdf
{
    public static class StringHelper
    {
        public static string FirstCharToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            StringBuilder sb = new StringBuilder(s.Length);
            sb.Append(char.ToUpper(s[0]));
            for (int i = 1; i < s.Length; i++) sb.Append(s[i]);
            return sb.ToString();
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static void IfNotNull(this string s, Action<string> action)
        {
            if (s.IsNullOrEmpty()) return;
            action?.Invoke(s);
        }
    }
}
