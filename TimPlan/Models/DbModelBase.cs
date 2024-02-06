using System.Reflection;

namespace TimPlan.Models
{
    public abstract class DbModelBase<T>
    {
        public abstract string DbTableName { get; }
        public abstract int Id { get; set; }
        public const string DbIdCol = "id";

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
