using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MerchantTribe.Migration
{
    public class TextHelper
    {

        public static string PlaceholderText()
        {
            return PlaceholderText(1024);
        }
        public static string PlaceholderText(int maxLength)
        {
            return TrimToLength("Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", maxLength);
        }
        public static string ForceAlphaNumericOnly(string input)
        {
            return Regex.Replace(input, @"[^a-zA-Z0-9]", "");
        }

        /// <summary>
        /// Pads a given string to a specific length with a given character
        /// </summary>
        /// <param name="sourceString">String to pad</param>
        /// <param name="maxLength">Maximum length of padded string</param>
        /// <param name="padCharacter">Character to append to string less than the max length.</param>
        /// <returns></returns>
        public static string PadString(string sourceString, int maxLength, string padCharacter)
        {

            string result = "";

            if (sourceString.Length > maxLength)
            {
                result = sourceString.Substring(0, maxLength);
            }
            else
            {
                result = sourceString;
                while (result.Length < maxLength)
                {
                    result += padCharacter;
                }
            }

            return result;
        }

        public static string PadStringLeft(string sourceString, int maxLength, string padCharacter)
        {

            string result = "";

            if (sourceString.Length > maxLength)
            {
                result = sourceString.Substring(0, maxLength);
            }
            else
            {
                result = sourceString;
                while (result.Length < maxLength)
                {
                    result = padCharacter + result;
                }
            }

            return result;
        }
        
        public static string TrimToLength(string input, int maxLength)
        {
            if (input == null)
            {
                return input;
            }

            string result = input;
            if (input.Length > maxLength)
            {
                result = input.Substring(0, maxLength);
            }
            return result;
        }

        public static string ConvertLinefeedToBrTag(string input)
        {
            string result = input;

            result = result.Replace(System.Environment.NewLine, "<br />");
            result = result.Replace(Convert.ToChar(13).ToString(), "<br />");
            result = result.Replace(Convert.ToChar(10).ToString(), "<br />");

            return result;
        }

        public static string CleanFileName(string input)
        {
            string result = input;
            result = result.Replace(" ", "-");
            result = result.Replace("\"", "");
            result = result.Replace("&", "and");
            result = result.Replace("?", "");
            result = result.Replace("=", "");
            result = result.Replace("/", "");
            result = result.Replace("\\", "");
            result = result.Replace("%", "");
            result = result.Replace("#", "");
            result = result.Replace("*", "");
            result = result.Replace("!", "");
            result = result.Replace("$", "");
            result = result.Replace("+", "-plus-");
            result = result.Replace(",", "-");
            result = result.Replace("@", "-at-");
            result = result.Replace(":", "-");
            result = result.Replace(";", "-");
            result = result.Replace(">", "");
            result = result.Replace("<", "");
            result = result.Replace("{", "");
            result = result.Replace("}", "");
            result = result.Replace("~", "");
            result = result.Replace("|", "-");
            result = result.Replace("^", "");
            result = result.Replace("[", "");
            result = result.Replace("]", "");
            result = result.Replace("`", "");
            result = result.Replace("'", "");
            result = result.Replace("©", "");
            result = result.Replace("™", "");
            result = result.Replace("®", "");

            return result;
        }

        public static string Slugify(string input)
        {
            return Slugify(input, true);
        }

        public static string Slugify(string input, bool urlEncode)
        {
            return Slugify(input, urlEncode, false);
        }
        public static string Slugify(string input, bool urlEncode, bool allowSlashesAndPeriods)
        {
            string result = input.Replace(' ', '-');

            result = result.Replace(" ", "-");
            result = result.Replace("\"", "");
            result = result.Replace("&", "and");
            result = result.Replace("?", "");
            result = result.Replace("=", "");
            if (!allowSlashesAndPeriods)
            {
                result = result.Replace("/", "");
                result = result.Replace(".", "");
            }
            result = result.Replace("\\", "");
            result = result.Replace("%", "");
            result = result.Replace("#", "");
            result = result.Replace("*", "");
            result = result.Replace("!", "");
            result = result.Replace("$", "");
            result = result.Replace("+", "-plus-");
            result = result.Replace(",", "-");
            result = result.Replace("@", "-at-");
            result = result.Replace(":", "-");
            result = result.Replace(";", "-");
            result = result.Replace(">", "");
            result = result.Replace("<", "");
            result = result.Replace("{", "");
            result = result.Replace("}", "");
            result = result.Replace("~", "");
            result = result.Replace("|", "-");
            result = result.Replace("^", "");
            result = result.Replace("[", "");
            result = result.Replace("]", "");
            result = result.Replace("`", "");
            result = result.Replace("'", "");
            result = result.Replace("©", "");
            result = result.Replace("™", "");
            result = result.Replace("®", "");            

            if (urlEncode)
            {
                result = System.Web.HttpUtility.UrlEncode(result);
            }

            if (allowSlashesAndPeriods)
            {
                result = result.Replace("%2f", "/");
                result = result.Replace("%252f", "/");
                //result = result.Replace(".", "");
            }

            return result;
        }

        public static string RemoveHtmlTags(string input)
        {
            return RemoveHtmlTags(input, string.Empty);
        }
        
        public static string RemoveHtmlTags(string input, string replacementString)
        {
            if (input == null)
            {
                return string.Empty;
            }
         
            string output = Regex.Replace(input, @"<(.|\n)*?>",replacementString);

            return output;
        }
    }
}

