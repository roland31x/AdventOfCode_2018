using System.Text;

namespace D05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string Polymer;
            using (StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                Polymer = sr.ReadToEnd();
            }

            string saved = Polymer;

            bool canReduce = true;
            while (canReduce)
            {
                Polymer = Reduce(Polymer, ref canReduce);
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(Polymer.Length);
            int minlen = int.MaxValue;
            for(int i = 'a'; i <= 'z'; i++)
            {
                char toremove = Convert.ToChar(i);
                Polymer = saved.Replace(toremove.ToString(), "").Replace(toremove.ToString().ToUpper(),"");
                canReduce = true;
                while (canReduce)
                {
                    Polymer = Reduce(Polymer, ref canReduce);
                }
                if (Polymer.Length < minlen)
                    minlen = Polymer.Length;
            }
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(minlen);
        }
        static string Reduce(string input, ref bool CanReduce)
        {
            bool reduced = false;
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < input.Length; i++)
            {
                if (i < input.Length - 1 && (input[i] - 'a' == input[i + 1] - 'A' || input[i] - 'A' == input[i + 1] - 'a'))
                {
                    i++;
                    reduced = true;
                }                   
                else
                    sb.Append(input[i]);
            }
            CanReduce = reduced;
            return sb.ToString();
        }
    }
}