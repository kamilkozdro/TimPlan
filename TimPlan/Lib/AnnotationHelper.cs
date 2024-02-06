using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TimPlan.Lib
{
    public static class AnnotationHelper
    {
        static public List<PropertyInfo> GetPropertiesInfo(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
        }
        static public List<FieldInfo> GetFieldsInfo(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance).ToList();
        }
        static public List<PropertyInfo> GetPropertiesWithColumnAnnotation(Type type)
        {
            return type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => Attribute.IsDefined(prop, typeof(ColumnAttribute)))
            .ToList();
        }
        public static string GetColumnAnnotationValue(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentException("Property not found", nameof(propertyInfo));
            }

            ColumnAttribute columnAttribute = propertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (columnAttribute == null)
            {
                return null; // or throw an exception if you expect the attribute to be always present
            }

            return columnAttribute.Name;
        }

    }
}
