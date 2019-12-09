using System.Collections.Generic;
using System.Text;

// ReSharper disable once CheckNamespace
namespace System
{
    internal static class StringExtensions
    {
        private const string Quote = "\"";
        private const char EscapeChar = '\\';
        private static readonly Dictionary<char, char> EscapeTranslations = new Dictionary<char, char> {
            { 'n', '\n' },
            { 'r', '\r' },
            { 't', '\t' }
        };
        internal static bool StartsWithQuote(this string value)
        {
            return value.StartsWith(Quote);
        }

        private static bool EndsWithQuote(this string value)
        {
            return value.EndsWith(Quote);
        }

        internal static string TrimQuote(this string value)
        {
            if (!value.StartsWithQuote() || !value.EndsWithQuote()) return value;
            if (value.Length == 1)
            {
                return string.Empty;
            }
            return value.Substring(1, value.Length - 2);
        }
        internal static string Unescape(this string value)
        {
            StringBuilder? sb = null;
            var charShouldBeEscaped = false;
            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                if (charShouldBeEscaped)
                {
                    if (sb == null)
                    {
                        sb = new StringBuilder(value.Length);
                        if (i > 1)
                        {
                            sb.Append(value.Substring(0, i - 1));
                        }
                    }

                    // General rule: \x ==> x
                    var escapedChar = EscapeTranslations.TryGetValue(c, out var unescaped) ? unescaped : c;
                    sb.Append(escapedChar);
                    charShouldBeEscaped = false;
                }
                else
                {
                    if (c == EscapeChar)
                    {
                        charShouldBeEscaped = true;
                    }
                    else
                    {
                        sb?.Append(c);
                    }
                }
            }
            return sb?.ToString() ?? value;
        }
    }
}
