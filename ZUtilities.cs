using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace AstroSpider
{
    class ZUtilities
    {
        //UNICODEb字符转为中文对这个方法做一点改进使他支持中英混排   
        public static string ConvertUnicodeStringToChinese(string unicodeString)
        {
            if (string.IsNullOrEmpty(unicodeString))
                return string.Empty;

            string outStr = unicodeString;

            Regex re = new Regex("\\\\u[0123456789abcdef]{4}", RegexOptions.IgnoreCase);
            MatchCollection mc = re.Matches(unicodeString);
            foreach (Match ma in mc)
            {
                outStr = outStr.Replace(ma.Value, ConverUnicodeStringToChar(ma.Value).ToString());
            }
            return outStr;
        }

        private static char ConverUnicodeStringToChar(string str)
        {
            char outStr = Char.MinValue;
            outStr = (char)int.Parse(str.Remove(0, 2), System.Globalization.NumberStyles.HexNumber);
            return outStr;
        }

        static Regex reUnicode = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);
        public static string ConvertChineseToUnicodeString(string chinese)
        {
            MatchCollection mc = reUnicode.Matches(chinese);
            string unicodeString = "";
            foreach (Match ma in mc)
            {
                short c;
                if (short.TryParse(mc.ToString().Remove(0, 1), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out c))
                {
                    unicodeString += ("\\u" + c);
                }
            }
            return unicodeString;
        }


        //public static string XmlSerializeToString(this object objectInstance)
        //{
        //    var serializer = new XmlSerializer(objectInstance.GetType());
        //    var sb = new StringBuilder();

        //    using (TextWriter writer = new StringWriter(sb))
        //    {
        //        serializer.Serialize(writer, objectInstance);
        //    }

        //    return sb.ToString();
        //}

        //public static T XmlDeserializeFromString<T>(this string objectData)
        //{
        //    return (T)XmlDeserializeFromString(objectData, typeof(T));
        //}

        //public static object XmlDeserializeFromString(this string objectData, Type type)
        //{
        //    var serializer = new XmlSerializer(type);
        //    object result;

        //    using (TextReader reader = new StringReader(objectData))
        //    {
        //        result = serializer.Deserialize(reader);
        //    }

        //    return result;
        //}
    }
}
