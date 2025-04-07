namespace operators
{
    class Program
    {
        public static void Main(string[] args)
        {

            Console.WriteLine(Math.Sqrt(25));
            Console.WriteLine(Math.Pow(25, 2));

            string hello = "\"Hello Word\"";

            Console.WriteLine(hello.Contains("he"));

            string[] chars = "1,2,3".Split(',');
            int[] nos = new int[3];
            bool[] success = new bool[3];

            Console.WriteLine(chars[1]);

            success[0] = int.TryParse(chars[0], out nos[0]);
            success[1] = int.TryParse(chars[1], out nos[1]);

            if (success[0] == success[1])
            {
                Console.WriteLine(nos[0].GetType());
            }

        }
    }
}