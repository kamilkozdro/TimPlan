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
        static public List<PropertyInfo> GetPropertiesInfo<T>()
        {
            List<PropertyInfo> properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            return properties;

        }

        static public List<PropertyInfo> GetPropertiesWithColumnAnnotation<T>()
        {
            return typeof(T)
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
