namespace D01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int sum = 0;
            List<int> fq = new List<int>();
            fq.Add(sum);
            int firstrepeat = int.MinValue;
            using(StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                while(!sr.EndOfStream)
                {
                    int val = int.Parse(sr.ReadLine()!);
                    sum += val;
                    if (fq.Contains(sum))
                    {
                        if (firstrepeat == int.MinValue)
                            firstrepeat = sum;
                    }                      
                    else
                        fq.Add(sum);
                }
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(sum);
            do
            {
                using (StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        int val = int.Parse(sr.ReadLine()!);
                        sum += val;
                        if (fq.Contains(sum))
                        {
                            firstrepeat = sum;
                            break;
                        }
                        else
                            fq.Add(sum);
                    }
                }
            } while(firstrepeat == int.MinValue);
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(firstrepeat);
        }
    }
}