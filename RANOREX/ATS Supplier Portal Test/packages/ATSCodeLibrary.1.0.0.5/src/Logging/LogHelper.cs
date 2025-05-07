using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.Logging
{
    public class LogHelper
    {
        /// <summary>
        /// Get a log4net logger
        /// Typical usage is class field defined as follows:
        /// private static readonly ILog log = LogHelper.GetLogger();
        /// </summary>
        /// <param name="filename">Do not enter a value</param>
        /// <returns>log4net logger</returns>
        public static log4net.ILog GetLogger([CallerFilePath]string filename = "")
        {
            return log4net.LogManager.GetLogger(System.IO.Path.GetFileName(filename));
        }        

        /// <summary>
        /// Get the current method's name using compiler services
        /// </summary>
        /// <param name="name">Do not enter a value</param>
        /// <returns>The name of the method calling this method or the value of parameter name</returns>
        public static string GetCurrentMethodName([CallerMemberName]string name = "")
        {
            return name;
        }

        /// <summary>
        /// Get the current method and argument names and values as a string
        /// Example usage in a method defined as
        /// private static void Test(string arg1, string arg2)
        /// GetArguments(System.Reflection.MethodBase.GetCurrentMethod(), arg1, arg2);
        /// Would return "Method=Test, System.String arg1=Value1, System.String arg2=Value2"
        /// </summary>
        /// <param name="method">The method that the name and parameters will be read from</param>
        /// <param name="values">The values corresponding to the parameters, order is important and all parameters must be passed</param>
        /// <returns>Method name, argument types, names and values or "Error: method parameters do not match values"</returns>
        public static string GetArguments(MethodBase method, params object[] values)
        {            
            var names = method.GetParameters();
            if (names.Length != values.Length)
                return "Error: method parameters do not match values";

            var sb = new StringBuilder();
            sb.Append($"Method={method.Name}, ");
            for (int i = 0; i < names.Length; i++)
                sb.Append(names[i] + "=" + values[i] + ", ");

            return sb.ToString().Trim().TrimEnd(',');            
        }
    }
}
