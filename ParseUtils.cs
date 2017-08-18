using System;
using System.Drawing;

namespace Mdameer.Watermark
{
    public static class ParseUtils
    {
        public static int ParseInt(string value)
        {
            int _value = 0;
            if (int.TryParse(value, out _value))
            {
                return _value;
            }

            return 0;
        }

        public static T ParseEnum<T>(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return (T)Enum.GetValues(typeof(T)).GetValue(0);
            }

            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static bool ParseBoolean(string value)
        {
            bool _value = false;
            if (bool.TryParse(value, out _value))
            {
                return _value;
            }

            return false;
        }

        public static Color ParseColor(string value)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                if (value.StartsWith("#"))
                {
                    return ColorTranslator.FromHtml(value);
                }
                else
                {
                    return Color.FromName(value);
                }
            }

            return Color.Black;
        }
    }
}