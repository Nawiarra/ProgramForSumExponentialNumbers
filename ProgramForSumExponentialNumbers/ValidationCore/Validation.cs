using System;
using System.Text.RegularExpressions;

namespace ValidationCore
{
    public static class Validation
    {
        private const string _exponentFormPattern = @"(?<mantissa>\d\.\d+)e(?<order>[+-]?\d+)\b";
        private static readonly Regex _valueRegex = new Regex(_exponentFormPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

        public static void CheckMantissaSize(string expForm, int mantisaSize)
        {
            ParseString(expForm, out string mantissa, out short order);

            if (mantissa.Length > mantisaSize)
                throw new FormatException("Incorrect format of input value");
        }

        public static short TryParseShortValueInLine(string line)
        {
            short result;

            if (!short.TryParse(line, out result))
            {
                throw new ArgumentException("Can't parse this string to int");
            }

            return result;
        }

        public static void ParseString(string val, out string mantissa, out short order)
        {
            Match m = _valueRegex.Match(val);
            if (!m.Success)
                throw new FormatException("Incorrect format of input value");

            mantissa = m.Groups["mantissa"].Value;
            if (!short.TryParse(m.Groups["order"].Value, out order))
                throw new FormatException("Incorrect format of input value");
        }
    }
}
