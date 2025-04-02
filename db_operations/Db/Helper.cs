using System.Reflection;

namespace db_operations.Db
{
    public static class Helper
    {
        public static void Print<T>(T obj)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            Console.WriteLine("{");
            foreach (PropertyInfo prop in properties)
            {
                Console.WriteLine($"   {prop.Name}: {prop.GetValue(obj)}");
            }
            Console.WriteLine("}");
        }

        public static void PrintMany<T>(IEnumerable<T> objs)
        {
            foreach (T obj in objs)
            {
                Print(obj);
            }
        }


        public static void LogWrite<T>(T obj, StreamWriter sw)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            string line = "Execution: " + DateTime.Now + "\n" + "{" + "\n" + "   ";
            foreach (PropertyInfo prop in properties)
            {
                line += $"{prop.Name}: {prop.GetValue(obj)}, ";
            }
            line += "\n" + "}" + "\n";

            sw.WriteLine(line);
        }

        public static void LogWriteMany<T>(IEnumerable<T> objs, StreamWriter sw)
        {
            string line = "Execution: " + DateTime.Now + "\n";

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            foreach (T obj in objs)
            {
                line += "{" + "\n" + "   ";
                foreach (PropertyInfo prop in properties)
                {
                    line +=  $"{prop.Name}: {prop.GetValue(obj)}, ";
                }
                line += "\n" + "}" + "\n";
            }
            sw.WriteLine(line);
        }
    }
}