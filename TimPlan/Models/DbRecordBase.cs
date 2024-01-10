using System.Reflection;

namespace TimPlan.Models
{
    public class DbRecordBase<T>
    {
        public override string ToString()
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string returnString = "Property Names: ";

            foreach (PropertyInfo property in properties)
            {
                returnString += $"{property.Name}={property.GetValue(this)}, ";
            }

            return returnString.TrimEnd(' ', ',');
        }
    }
}
