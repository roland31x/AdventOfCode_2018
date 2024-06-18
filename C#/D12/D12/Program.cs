namespace D12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int size = 400;
            int count = 0;
            int[] currentgen;
            List<Seed> seeds = new List<Seed>();
            using(StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                string initial = sr.ReadLine()!.Split(' ')[2];
                currentgen = new int[initial.Length + size];
                for (int i = 0; i < initial.Length; i++)
                {
                    if (initial[i] == '.')
                        currentgen[i + size / 2] = 0;
                    else
                    {
                        currentgen[i + size / 2] = 1;
                    }
                        
                }
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    seeds.Add(new Seed(sr.ReadLine()!));
                }
            }

            for(int times = 0; times < 200; times++)
            {
                int[] newgen = new int[currentgen.Length];
                for (int i = 0; i < currentgen.Length; i++)
                {
                    int[] pattern = new int[5];
                    for(int k = i - 2, idx = 0; k <= i + 2; k++, idx++)
                    {
                        if(k >= 0 && k < currentgen.Length)
                            pattern[idx] = currentgen[k];
                    }
                    foreach(Seed s in seeds)
                    {
                        bool found = false;
                        for(int k = 0; k <= 4; k++)
                        {
                            if (pattern[k] != s.pattern[k])
                                break;
                            if (k == 4)
                            {
                                newgen[i] = s.product;
                                if (s.product == 1)
                                {
                                    
                                }
                                found = true;
                            }                             
                        }
                        if (found) 
                            break;
                    }
                }
                currentgen = newgen;
                
                if(times == 19)
                {
                    for (int i = 0; i < currentgen.Length; i++)
                    {
                        if (currentgen[i] == 1)
                        {
                            int num = i - size / 2;
                            count += num;
                        }
                    }

                    Console.WriteLine("Part 1 solution:");
                    Console.WriteLine(count);
                }

                // so basically after a some steps the pattern becomes #.#.#.#.#.#.#.#.#.#.#.#.#.#.#
                // with whatever length and gets offset by 1 pot each generation,
                // so we find the first generation that has this pattern then we can calc the 50 billionth

                bool okPattern = true;
                int h = 0;
                int patternlength = 0;
                int startidx = 0;
                while (currentgen[h] != 1)
                {
                    h++;
                }
                startidx = h - size / 2;
                while (currentgen[h] == 1 && currentgen[h + 1] == 0)
                {
                    h += 2;
                    patternlength += 2;
                }
                for(; h < currentgen.Length; h++)
                {
                    if (currentgen[h] == 1)
                    {
                        okPattern = false;
                        break;
                    }
                }            
                
                if (okPattern)
                {
                    long sum = 0;
                    for(long l = startidx + 50_000_000_000 - (times + 1); l < startidx + 50_000_000_000 - (times + 1) + patternlength; l += 2) // times + 1 coz generation 1 is times = 0
                    {
                        sum += l;
                    }
                    Console.WriteLine("Part 2 solution:");
                    Console.WriteLine(sum);
                    break;
                }

            }

        }
    }
    public class Seed
    {
        public int[] pattern = new int[5];
        public int product = 0;
        public Seed(string pattern)
        {
            string p = pattern.Split("=>")[0].Trim();
            string res = pattern.Split("=>")[1].Trim();
            if (res == "#")
                product = 1;
            for (int i = 0; i < 5; i++)
                if (pattern[i] == '#')
                    this.pattern[i] = 1;
        }
    }
}