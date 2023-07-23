using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace D10
{
    internal class Program
    {
        public static Regex Vector = new Regex(@"<[ ]?[-]?[0-9]+,[ ][ ]?[-]?[0-9]+>");
        static void Main(string[] args)
        {
            List<MovingPoint> points = new List<MovingPoint>();
            using(StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine()!;
                    MatchCollection mc = Vector.Matches(line);
                    string pos = mc[0].Value.Replace("<", "").Replace(">", "").Trim();
                    string vel = mc[1].Value.Replace("<", "").Replace(">", "").Trim();
                    int x = int.Parse(pos.Split(',')[0].Trim());
                    int y = int.Parse(pos.Split(',')[1].Trim());
                    int dx = int.Parse(vel.Split(',')[0].Trim());
                    int dy = int.Parse(vel.Split(',')[1].Trim());
                    points.Add(new MovingPoint(x, y, dx, dy));
                }
            }

            int minY = 0, minX = 0, maxY = 0, maxX = 0;
            int mindist = int.MaxValue;
            bool found = false;
            int seconds = 0;
            while (!found)
            {
                minY = int.MaxValue;
                maxY = 0;
                minX = int.MaxValue;
                maxX = 0;
                foreach (MovingPoint p in points)
                {
                    if (p.Pos[1] < minY)
                        minY = p.Pos[1];
                    if (p.Pos[1] > maxY)
                        maxY = p.Pos[1];
                    if (p.Pos[0] < minX)
                        minX = p.Pos[0];
                    if (p.Pos[0] > maxX)
                        maxX = p.Pos[0];
                    
                }
                int currdist = maxY - minY;
                if (currdist < mindist)
                    mindist = currdist;
                else
                    found = true;
                if (found)
                    break;

                foreach (MovingPoint p in points)
                    p.Tick();
                seconds++;
            }

            foreach (MovingPoint p in points)
                p.UnTick();
            seconds--;

            minY = int.MaxValue;
            maxY = 0;
            minX = int.MaxValue;
            maxX = 0;
            foreach (MovingPoint p in points)
            {
                if (p.Pos[1] < minY)
                    minY = p.Pos[1];
                if (p.Pos[1] > maxY)
                    maxY = p.Pos[1];
                if (p.Pos[0] < minX)
                    minX = p.Pos[0];
                if (p.Pos[0] > maxX)
                    maxX = p.Pos[0];

            }

            Console.WriteLine("Part 1 solution:");
            for (int i = minY; i <= maxY; i++)
            {
                for (int j = minX; j <= maxX; j++)
                {
                    bool ok = false;
                    foreach (MovingPoint p in points)
                    {
                        if (p.Pos[0] == j && p.Pos[1] == i)
                        {
                            Console.Write("#");
                            ok = true;
                            break;
                        }
                    }
                    if (!ok)
                        Console.Write(".");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(seconds);
        }
    }
    public class MovingPoint
    {
        public int[] Pos = new int[2];
        public int[] Vel = new int[2];
        public MovingPoint(int x,int y, int dx,int dy)
        {
            Pos[0] = x;
            Pos[1] = y;
            Vel[0] = dx;
            Vel[1] = dy;
        }
        public void Tick()
        {
            Pos[0] += Vel[0];
            Pos[1] += Vel[1];
        }
        public void UnTick()
        {
            Pos[0] -= Vel[0];
            Pos[1] -= Vel[1];
        }
    }
}