using System.Drawing;

namespace D06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map(@"..\..\..\input.txt");
            

            Tuple<int, int> result = map.GetResult();
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(result.Item1);
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(result.Item2);

        }
    }
    public class Map
    {
        int maxsearchX = 1500; // increase these if you don't get right answer
        int maxsearchY = 1500;
        List<Point> POI = new List<Point>();
        bool[] inf;
        int[] area;
        public Map(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine()!;
                    POI.Add(new Point(int.Parse(line.Split(',')[0].Trim()) + maxsearchX / 2, int.Parse(line.Split(',')[1].Trim()) + maxsearchY / 2));
                }
            }
            inf = new bool[POI.Count];
            area = new int[POI.Count];
        }
        public Tuple<int,int> GetResult()
        {
            int count = 0;
            for(int i = 0; i < maxsearchX; i++)
            {
                for (int j = 0; j < maxsearchY; j++)
                {
                    int distance = int.MaxValue;
                    int idx = -1;
                    bool ok = true;
                    int totaldist = 0;
                    for(int p = 0; p < POI.Count; p++)
                    {
                        int manhattan = Math.Abs(POI[p].X - i) + Math.Abs(POI[p].Y - j);
                        if (manhattan <= distance)                        
                        {
                            if (manhattan < distance)
                            {
                                distance = manhattan;
                                idx = p;
                            }
                            else
                            {
                                ok = false;
                            }
                        }
                        totaldist += manhattan;
                                
                    }
                    if (totaldist < 10000)
                        count++;
                    if (ok)
                    {
                        //dist[i, j] = idx;
                        area[idx]++;
                    }                       
                    if (i == 0 || j == 0 || i == maxsearchY - 1 || j == maxsearchY - 1)
                        if (ok)
                            inf[idx] = true;                           
                }
            }

            int max = 0;
            for(int i = 0; i < area.Length; i++)
            {
                if (area[i] > max && !inf[i])
                    max = area[i];
            }
            return new Tuple<int,int>(max,count);
        }
    }
}