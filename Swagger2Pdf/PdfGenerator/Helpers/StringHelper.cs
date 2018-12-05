using System;
using System.Linq;
using System.Text;

namespace Swagger2Pdf.PdfGenerator.Helpers
{
    public static class StringHelper
    {
        public static void IfNotNull(this string s, Action<string> func = null)
        {
            if (!s.IsNullOrEmpty())
            {
                func?.Invoke(s);
            }
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            }

            if (char.IsUpper(input.First())) return input;

            var sb = new StringBuilder(input.Length);
            sb.Append(char.ToUpper(input.First()));
            for (var i = 1; i < input.Length; i++)
            {
                sb.Append(char.ToLower(input[i]));
            }

            return sb.ToString();
        }
    }
}