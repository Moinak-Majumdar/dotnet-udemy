// See https://aka.ms/new-console-template for more information

namespace HelloWorld
{
    internal class Program
    {
        static void Main(String[] args)
        {

            float f1 = 0.751f;
            float f2 = 0.75f;

            double d1 = 0.751;
            double d2 = 0.75;

            decimal dm1 = 0.751m;
            decimal dm2 = 0.75m;


            Console.WriteLine(f1 - f2);
            Console.WriteLine(d1 -d2);
            Console.WriteLine(dm1 - dm2);

            string [] sr = new string [2];
            sr[0] = "XYZ";
            List<string> sl = [];

            sl.Add("ABC");
            sl.Add("IJK");

            Console.WriteLine(sr[0]);
            Console.WriteLine(sl[1]);

            int[,] multiDimArr = {
                {1,2},
                {3,4},
                {5,6}
            };

            Dictionary<string, int> dict = new Dictionary<string, int>();

            dict["abc"] = 1;
            dict["Efg"] = 2;
        }
    }

}