﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gigobyte.Daterpillar.Transformation.Template
{
    /// <summary>
    /// Extension methods for the <see cref="Template"/> namespace.
    /// </summary>
    public static class TemplateExtensions
    {
        /// <summary>
        /// Converts the specified string to camel case.
        /// </summary>
        /// <param name="text">The string to convert to camel case.</param>
        /// <returns>The specified string converted to camel case.</returns>
        /// <remarks>
        /// <list type="bullet"><listheader>Rules</listheader><item>Lower the first letter in the
        /// text.</item><item>Capitalize the first letter for each subsequent
        /// word.</item><item>Concatenate each word.</item></list>
        /// </remarks>
        public static string ToCamelCase(this string text, params char[] separator)
        {
            if (text.Length == 1) return text.ToLower();
            else
            {
                string pascal = ToPascalCase(text, separator);
                return char.ToLower(pascal[0]) + pascal.Substring(1);
            }
        }

        /// <summary>
        /// Converts the specified string to pascal case.
        /// </summary>
        /// <param name="text">The string to convert to pascal case.</param>
        /// <returns>The specified string converted to pascal case.</returns>
        /// <remarks>
        /// <list type="bullet"><listheader>Rules</listheader><item>Capitalize the first letter in
        /// the text.</item><item>Capitalize the first letter for each subsequent
        /// word.</item><item>Concatenate each word.</item></list>
        /// </remarks>
        public static string ToPascalCase(this string text, params char[] separator)
        {
            if (text.Length == 1) return text.ToUpper();
            else
            {
                if (separator.Length == 0) separator = new char[] { ' ' };
                string[] words = text.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
                string pascal = "";

                foreach (var word in words)
                {
                    pascal += char.ToUpper(word[0]) + word.Substring(1);
                }

                return pascal;
            }
        } 

        public static string AS(this string text, string alias)
        {
            return $"{text} AS '{alias}'";
        }

        internal static string AppendPeriod(this string text)
        {
            return text[text.Length - 1] == '.' ? text : (text + ".");
        }

        internal static void RemoveLastComma(this StringBuilder builder)
        {
            int commaIndex = builder.ToString().LastIndexOf(',');
            builder.Remove(commaIndex, 1);
        }

        internal static bool IsKey(this IEnumerable<Index> indexes, string columnName)
        {
            foreach (var index in indexes.Where(x => x.IndexType == IndexType.Primary))
            {
                if (index.Columns.Exists(x => x.Name == columnName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}