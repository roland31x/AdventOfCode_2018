using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace D17
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map(@"..\..\..\input.txt");
            Tuple<int, int> result = map.Sim();
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(result.Item1);
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(result.Item2);
        }
    }
    public class Map
    {
        public int[,] map = new int[2000, 2000];
        int SpringI = 0;
        int SpringJ = 500;
        int MinY = int.MaxValue;
        int MaxY = int.MinValue;
        public Map(string file)
        {
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines)
            {
                int pivot = int.Parse(line.Split('=')[1].Split(',')[0]);
                int startrange = int.Parse(line.Split('=')[2].Split("..")[0]);
                int endrange = int.Parse(line.Split('=')[2].Split("..")[1]);
                if (line.Split('=')[0] == "x")
                {
                    for (int i = startrange; i <= endrange; i++)
                        map[i, pivot] = 1;
                    if (startrange < MinY)
                        MinY = startrange;
                    if (endrange > MaxY)
                        MaxY = endrange;
                }

                else
                {
                    if (pivot < MinY)
                        MinY = endrange;
                    if (pivot > MaxY)
                        MaxY = endrange;
                    for (int i = startrange; i <= endrange; i++)
                        map[pivot, i] = 1;
                }

            }
        }
        public Tuple<int, int> Sim()
        {
            Down(SpringI, SpringJ);

            int countall = 0;
            int settled = 0;
            for (int i = MinY; i <= MaxY; i++)
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 2) // for some reason there is a bug where a line of 2s is left inbetween a "sea of settled water" so we just check neighbors to see if it's actually a 2 or not
                    {
                        bool ok = true;
                        int threes = 0;
                        for (int h1 = i - 1; h1 <= i + 1; h1++)
                            for (int h2 = j - 1; h2 <= j + 1; h2++)
                            {
                                if (map[h1, h2] == 0)
                                    ok = false;
                                if (map[h1, h2] == 3)
                                    threes++;
                            }

                        if (ok && threes > 0)
                            map[i, j] = 3;
                    }
                    if (map[i, j] >= 2)
                        countall++;
                    if (map[i, j] == 3)
                        settled++;
                }

            return new Tuple<int, int>(countall, settled);
        }
        public void Down(int I, int J)
        {
            if (I == MaxY + 2)
                return;

            map[I, J] = 2;

            if (map[I + 1, J] == 0)
            {
                Down(I + 1, J);
            }
            else if (map[I + 1, J] != 2)
            {
                int ok = 0;
                int left = J;
                while ((map[I, left - 1] != 1) && (map[I + 1, left - 1] == 1 || map[I + 1, left - 1] == 3))
                {
                    left--;
                    map[I, left] = 2;
                }
                if (map[I, left - 1] == 1)
                {
                    ok++;
                }
                else
                {
                    Down(I, left - 1);
                }

                int right = J;
                while ((map[I, right + 1] != 1) && (map[I + 1, right + 1] == 1 || map[I + 1, right + 1] == 3))
                {
                    right++;
                    map[I, right] = 2;
                }
                if (map[I, right + 1] == 1)
                {
                    ok++;
                }
                else
                {
                    Down(I, right + 1);
                }

                if (ok == 2)
                {
                    for (int i = left; i <= right; i++)
                        map[I, i] = 3;
                    Down(I - 1, J);
                }

            }

        }
    }
}