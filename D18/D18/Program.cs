using System.Reflection;

namespace D18
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> states = new List<int>();
            TreeMap map = new TreeMap(@"..\..\..\input.txt");
            for (int times = 0; times < 10; times++)
            {
                map.NextGen();
                states.Add(map.Score());
            }

            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(map.Score());
            Console.WriteLine();

            List<int>[] repeats = new List<int>[1000];
            for (int i = 0; i < repeats.Length; i++)
                repeats[i] = new List<int>();



            int repeatidx = -1;
            int whichisrepeated = -1;
            int ok = 0;

            for (int times = 10; times < 2000; times++) // calc at least 2000 states, see if they repeat, add repeats to a list of indexes
            {
                map.NextGen();
                int sc = map.Score();
                if (states.Contains(sc))
                {
                    whichisrepeated = states.IndexOf(sc);                   
                    repeatidx = times;
                    repeats[whichisrepeated].Add(repeatidx);
                }
                states.Add(sc);
            }

            for(int i = 0; i < repeats.Length; i++) // check repeats, if the repeat seq is long enough it will divide into 1 billion
            {
                if (repeats[i].Count > 15) // 15 samples is enough, usually
                {
                    int idx = i;
                    int repeatdiff = repeats[i][0] - idx;
                    int currentstate = idx + ((1_000_000_000 - idx) / repeatdiff) * repeatdiff;
                    while(currentstate < 1_000_000_000)
                    {
                        currentstate++;
                        idx++;
                    }
                    Console.WriteLine("Part 2 solution:");
                    Console.WriteLine(states[idx - 1]);
                    break;
                    // since the state for minute 1 is stored at index 0, we need index "1bln - 1" for minute 1 billion
                }
            }
        }
    }
    public class TreeMap
    {
        static int size = 50;
        int[,] map = new int[size, size];
        public TreeMap(string file) 
        {
            using(StreamReader sr = new StreamReader(file))
            {
                int i = 0;
                while (!sr.EndOfStream) 
                {
                    string line = sr.ReadLine()!;
                    for(int j = 0; j < line.Length; j++)
                    {
                        switch(line[j])
                        {
                            case '.': // open acre
                                map[i, j] = 0;
                                break;
                            case '|': // tree
                                map[i, j] = 1;
                                break;
                            case '#': // lumberyard
                                map[i, j] = 2;
                                break;
                        }
                    }    
                    i++;
                }
            }
        }
        public void Show()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public void NextGen()
        {
            int[,] newmap = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    newmap[i, j] = Outcome(i, j);
                }
            }
            map = newmap;
        }
        public int Score()
        {
            int wood = 0;
            int yard = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (map[i, j] == 1)
                        wood++;
                    else if (map[i,j] == 2)
                        yard++;
                }                               
            return wood * yard;
        }
        int Outcome(int i, int j)
        {
            int[] fq = new int[3];
            for(int h1 = i - 1; h1 <= i + 1; h1++)
            {
                for(int h2 = j - 1; h2 <= j + 1; h2++)
                {
                    if ((h1 == i && h2 == j) || h1 < 0 || h1 >= size || h2 < 0 || h2 >= size)
                        continue;

                    fq[map[h1, h2]]++;

                }
            }
            if (map[i, j] == 0 && fq[1] >= 3)
                return 1;

            if (map[i, j] == 1)
            {
                if (fq[2] >= 3)
                    return 2;
                return 1;
            }
            if (map[i, j] == 2 && fq[2] >= 1 && fq[1] >= 1)
                return 2;
            return 0;

        }
    }
}