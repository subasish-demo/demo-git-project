using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.DataUtilities.Reflection
{
    /// <summary>
    /// Includes extension methods to read from generic types and return them in a usable format
    /// See source code for usage examples
    /// </summary>
    public static class ReflectionHelper
    {

        private static BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
        
        /*
             // Example code showing usage with sorting. To implement sorting copy the OrderAttribute definition into an existing type's namespace
                          
             static void Main(string[] args)
             {
                 var list = new List<TestClass>();
                 TestClass.pubStaticField = "static field";
                 TestClass.pubStaticProp = "static prop";
                 list.Add(new TestClass()
                 {
                     pubField = "Public field",
                     pubProp = "Public property"
                 });
                 var table = list.ToDataTable(typeof(OrderAttribute), false, false, false);
             }

             // Attribute definition
             [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
             public sealed class OrderAttribute : Attribute, IOrderedMembers
             {
                 private readonly int order_;
                 public OrderAttribute([CallerLineNumber]int order = 0)
                 {
                     order_ = order;
                 }

                 public int Order { get { return order_; } }
             }

             // Class that has sorted members
             class TestClass
             {
                 [Order]
                 public string pubProp { get; set; }
                 [Order]
                 public string pubField;

                 [Order]
                 public static string pubStaticProp { get; set; }
                 [Order]
                 public static string pubStaticField;
             }        
        */

        private static IEnumerable<MemberInfo> GetFieldsAndPropsAsMember (this Type type, Type sortAttributeType, bool includeProperties, bool includeFields, bool includeStatic)
        {            
            var returnValue = new List<MemberInfo>();
            var flags = ReflectionHelper.flags;
            
            if (includeStatic)
            {
                flags = ReflectionHelper.flags | BindingFlags.Static;
            }

            if (includeProperties)
                returnValue.AddRange(type.GetProperties(flags));

            if (includeFields)
                returnValue.AddRange(type.GetFields(flags));
            
            var ret = from property in returnValue
                        where !property.Name.EndsWith("k__BackingField")
                        let orderAttribute = property.GetCustomAttributes(sortAttributeType).FirstOrDefault()
                        orderby (orderAttribute as IOrderedMembers) == null ? 0 : (orderAttribute as IOrderedMembers).Order 
                        select property;
            
            return ret;
        }

        /// <summary>
        /// Get public field and property names in the provided type
        /// </summary>
        /// <param name="type">Type for which fields and properties will be returned</param>
        /// <param name="sortAttributeType">Class that derives from System.Attribute and provides CustomAttribute in a format that can be sorted. If a valid type is not provided then the return will not be sorted</param>
        /// <param name="includeProperties">Include public property names</param>
        /// <param name="includeFields">Include public field names</param>
        /// <param name="includeStatic">Include static field and properties</param>
        /// <returns>Enumerable sting with names based on values of filters, sorted by sort attribute if provided</returns>
        public static IEnumerable<string> GetFieldsAndProps(this Type type, Type sortAttributeType, bool includeProperties, bool includeFields, bool includeStatic)
        {
            return type.GetFieldsAndPropsAsMember(sortAttributeType, includeProperties, includeFields, includeStatic).Select(x => x.Name);
        }

        /// <summary>
        /// Converts collection of public properties and fields to datatable that can be sorted using custom attributes
        /// </summary>
        /// <typeparam name="T">Collection object type</typeparam>
        /// <param name="data">Collection to convert</param>
        /// <param name="sortAttributeType">Class that derives from System.Attribute and provides CustomAttribute in a format that can be sorted. If a valid type is not provided then the return will not be sorted</param>
        /// <param name="includeProperties">Include public properties in the returned table</param>
        /// <param name="includeFields">Include public fields in the returned table</param>
        /// <param name="includeStatic">Include static members (based on includeProperties and includeFields) in the returned table</param>
        /// <returns>DataTable, columns named after fields and properties</returns>
        public static DataTable ToDataTable<T>(this IList<T> data, Type sortAttributeType, bool includeProperties, bool includeFields, bool includeStatic)
        {
            var members = typeof(T).GetFieldsAndPropsAsMember(sortAttributeType, includeProperties, includeFields, includeStatic);
            DataTable table = new DataTable();
            foreach (var member in members)
            {
                table.Columns.Add(member.Name);
            }

            foreach (var item in data)
            {
                var newRow = table.NewRow();
                foreach (var member in members)
                {
                    if (member is FieldInfo)
                    {
                        var val = (member as FieldInfo).GetValue(item);
                        newRow[member.Name] = val;
                    }
                    else if (member is PropertyInfo)
                    {
                        var val = (member as PropertyInfo).GetValue(item);
                        newRow[member.Name] = val;
                    }
                }

                table.Rows.Add(newRow);
            }

            return table;
        }

        /// <summary>
        /// Converts collection of public non-static properties to unsorted datatable
        /// </summary>
        /// <typeparam name="T">Collection object type</typeparam>
        /// <param name="data">Collection to convert</param>
        /// <returns>DataTable, column names match property names</returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }        
    }
}
