using System.Reflection;
using System.Text;

namespace Task1
{
    internal class Program
    {
        static object? StringToObject(string s)
        {
            if (s == null)
                return null;

            string[] arrClass = s.Split('|', StringSplitOptions.RemoveEmptyEntries);
            string[] buildInfo = arrClass[0].Split(':');

            object result = Activator.CreateInstance(null, buildInfo[0].Split(',')[0]).Unwrap();


            if (arrClass.Length > 1 && result != null)
            {
                // Сложность O(n^2) можно оптимизировать но честно говоря нет времени обдумывать
                for (int i = 1; i < arrClass.Length; i++)
                {
                    string[] fieldPair = arrClass[i].Split(':');
                    var fields = result.GetType().GetFields();

                    if (fields.Length > 0)
                    {
                        foreach (var field in fields)
                        {
                            var attr = field.GetCustomAttribute<CustomNameAttribute>();
                            if (attr?.Name == fieldPair[0])
                            {
                                if (field.FieldType == typeof(int))
                                    field.SetValue(result, int.Parse(fieldPair[1]));
                            }
                        }
                    }
                    else
                        break;

                }

                for (int i = 1; i < arrClass.Length; i++)
                {
                    string[] propsPair = arrClass[i].Split(':');
                    var prop = result.GetType().GetProperty(propsPair[0]);                    

                    if (prop?.PropertyType == typeof(int))
                        prop.SetValue(result, int.Parse(propsPair[1]));
                    else if (prop?.PropertyType == typeof(string))
                        prop.SetValue(result, propsPair[1]);
                    else if (prop?.PropertyType == typeof(decimal))
                        prop.SetValue(result, decimal.Parse(propsPair[1]));
                    else if (prop?.PropertyType == typeof(char[]))
                        prop.SetValue(result, propsPair[1].ToCharArray());
                }
            }

            return result;
        }

        static string ObjectToString(object? o)
        {
            Type type = o.GetType();
            StringBuilder sb = new StringBuilder();

            sb.Append($"{type.AssemblyQualifiedName}:");
            sb.Append(type.Name + "|");

            var fields = type.GetFields();
            foreach ( var field in fields )
            {
                var attr = field.GetCustomAttribute<CustomNameAttribute>();
                if ( attr != null )
                {
                    sb.Append($"{attr.Name}:{field.GetValue(o)}|");
                }
            }

            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (prop.PropertyType != typeof(char[]))
                    sb.Append($"{prop.Name}:{prop.GetValue(o)}|");
                else
                    sb.Append($"{prop.Name}:{new string(prop.GetValue(o) as char[])}|");
            }

            return sb.ToString();
        }

        static void Main(string[] args)
        {
            Type testClass = typeof(TestClass);
            var ctor = Activator.CreateInstance(testClass, new object[] { 5, "test", 1.5m, new char[] { 'a', 'b', 'c' } });

            var resString = ObjectToString(ctor);
            Console.WriteLine(resString);

            var resObject = StringToObject(resString);
        }
    }
}
