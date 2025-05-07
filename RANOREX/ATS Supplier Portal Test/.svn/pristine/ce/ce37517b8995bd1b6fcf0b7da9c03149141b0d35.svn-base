using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.DataUtilities.HTML
{
    public static class HtmlHelper
    {
        /// <summary>
        /// Convert DataTable to HTML table in string
        /// </summary>
        /// <param name="dt">Datatable to convert</param>
        /// <returns>String containing HTML table</returns>
        public static string ToHtmlTable(this DataTable dt)
        {
            if (dt == null)
                return null;

            var html = new StringBuilder();
            html.Append("<table>");

            //add header row
            html.Append("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
                html.Append("<td>" + dt.Columns[i].ColumnName + "</td>");
            html.Append("</tr>");

            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                    html.Append("<td>" + dt.Rows[i][j].ToString() + "</td>");
                html.Append("</tr>");
            }

            html.Append("</table>");

            return html.ToString();
        }

        /// <summary>
        /// Convert public properties in object collection to HTML table
        /// </summary>
        /// <typeparam name="T">Type of object that will be converted</typeparam>
        /// <param name="listOfClassObjects">Collection of objects with public properties</param>
        /// <returns>String containing HTML table</returns>
        public static string ToHtmlTable<T>(this IEnumerable<T> listOfClassObjects)
        {
            var ret = string.Empty;

            return listOfClassObjects == null || !listOfClassObjects.Any()
                ? ret
                : "<table>" +
                  listOfClassObjects.First().GetType().GetProperties().Select(p => p.Name).ToColumnHeaders() +
                  listOfClassObjects.Aggregate(ret, (current, t) => current + t.ToHtmlTableRow()) +
                  "</table>";
        }

        private static string ToColumnHeaders<T>(this IEnumerable<T> listOfProperties)
        {
            var ret = string.Empty;

            if (listOfProperties == null || !listOfProperties.Any())
                return ret;

            try
            {
                return "<tr>" +
                  listOfProperties.Aggregate(ret,
                      (current, propValue) =>
                          current +
                          ("<th>" +
                           (Convert.ToString(propValue).Trim().Length <= 100
                               ? Convert.ToString(propValue)
                               : Convert.ToString(propValue).Substring(0, 100) + "...") + "</th>")) +
                  "</tr>";
            }
            catch
            {
                return ret;
            }

        }

        private static string ToHtmlTableRow<T>(this T classObject)
        {
            var ret = string.Empty;

            if (classObject == null)
                return null;

            try
            {
                return classObject == null
                ? ret
                : "<tr>" +
                  classObject.GetType()
                      .GetProperties()
                      .Aggregate(ret,
                          (current, prop) =>
                              current + ("<td>" +
                                         (Convert.ToString(prop.GetValue(classObject)).Trim().Length <= 100
                                             ? Convert.ToString(prop.GetValue(classObject))
                                             : Convert.ToString(prop.GetValue(classObject)).Substring(0, 100) +
                                               "...") +
                                         "</td>")) + "</tr>";
            }
            catch
            {
                return "";
            }


        }

        /// <summary>
        /// Add HTML tag to current string by adding <tag>input</tag>
        /// </summary>
        /// <param name="input">value that will be wrapped</param>
        /// <param name="tag">value of tag (not including triangular brackets or slashes)</param>
        /// <returns>String with HTML tag, example input = value, tag = tr, output will = <tr>value</tr></returns>
        public static string WrapWithTag(this string input, string tag)
        {
            if (string.IsNullOrEmpty(input))
                return $"<{tag}><\\{tag}>";
            else
                return $"<{tag}>{input}<\\{tag}>";
        }
    }
}
