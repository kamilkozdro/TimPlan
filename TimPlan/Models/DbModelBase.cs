using System;
using System.Reflection;

namespace TimPlan.Models
{
    public abstract class DbModelBase
    {
        public abstract string DbTableName { get; }
        public abstract int Id { get; set; }
        public const string DbIdCol = "id";

        public override string ToString()
        {
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string returnString = "Property Names: ";

            foreach (PropertyInfo property in properties)
            {
                returnString += $"{property.Name}={property.GetValue(this)}, ";
            }

            return returnString.TrimEnd(' ', ',');
        }
    }
}
