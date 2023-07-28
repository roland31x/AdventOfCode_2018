using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace D22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] target = new int[] { 7, 782 };      // INPUTS     
            int depth = 11820;                        // here
            CaveMap map = new CaveMap(depth, target);
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(map.RiskCalc());
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(map.FindShortestRoute());
        }
    }
    public class CaveMap
    {
        public static List<int[]> dirs = new List<int[]>()
        {
            new int[] { -1, 0 },
            new int[] { 0, 1 },
            new int[] { 0, -1 },
            new int[] { 1, 0 },
        };
        public static int size = 1500;
        int[,] erosionmap = new int[size, size];
        int[,] map = new int[size, size];
        int[,,] marked = new int[size, size, 3];
        int depth;
        int[] target; // X , Y
        public CaveMap(int depth, int[] target)
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    erosionmap[i, j] = -1;

            this.depth = depth;
            this.target = target;
            int max = size - 1;
            for (int Y = 0; Y <= max; Y++)
            {
                for (int X = 0; X <= max; X++)
                {
                    map[Y, X] = GetErosion(X, Y) % 3;
                }
            }
        }
        public int FindShortestRoute()
        {
            PriorityQueue<Player, int> players = new PriorityQueue<Player, int>();
            List<Player> ok = new List<Player>();
            players.Enqueue(new Player(0, 0, 0, 1), 1);
            players.Enqueue(new Player(1, 0, 0, 8), 8);
            marked[0, 0, 0] = 1;
            marked[0, 0, 1] = 8;
            while (players.Count > 0)
            {
                Player p = players.Dequeue();
                if (p.X == target[0] && p.Y == target[1])
                {
                    ok.Add(p);
                    continue;
                }
                if (marked[p.Y, p.X, p.Tool] == 1)
                    continue;
                marked[p.Y, p.X, p.Tool] = 1;
                foreach (int[] dir in dirs)
                {
                    int newY = p.Y + dir[0];
                    int newX = p.X + dir[1];
                    if (newY < 0 || newX < 0 || newY >= size || newX >= size)
                        continue;
                    int[] tools = GetTools(newX, newY);
                    if (p.Tool == tools[0])
                    {
                        players.Enqueue(new Player(p.Tool, newX, newY, p.Mark + 1), p.Mark + 1);
                        players.Enqueue(new Player(tools[1], newX, newY, p.Mark + 1 + 7), p.Mark + 1 + 7);
                    }
                    else if (p.Tool == tools[1])
                    {
                        players.Enqueue(new Player(p.Tool, newX, newY, p.Mark + 1), p.Mark + 1);
                        players.Enqueue(new Player(tools[0], newX, newY, p.Mark + 1 + 7), p.Mark + 1 + 7);
                    }
                    else
                    {
                        players.Enqueue(new Player(tools[0], newX, newY, p.Mark + 1 + 7), p.Mark + 1 + 7);
                        players.Enqueue(new Player(tools[1], newX, newY, p.Mark + 1 + 7), p.Mark + 1 + 7);
                    }
                }
            }
            int best = int.MaxValue;
            foreach(Player p in ok)
            {
                if (p.Tool == 0)
                {
                    if (p.Mark < best)
                        best = p.Mark - 1;
                }                   
                else if (p.Mark + 7 < best)
                        best = p.Mark + 7 - 1;
            }
            return best;
            
        }
        public int[] GetTools(int x, int y)
        {
            switch (map[y, x])
            {
                case 0:
                    return new int[] { 0, 1 };
                case 1:
                    return new int[] { 1, 2 };
                case 2:
                    return new int[] { 2, 0 };
            }
            throw new NotImplementedException();
        }
        public void Show()
        {
            for (int i = 0; i <= target[0]; i++)
            {
                for (int j = 0; j <= target[1]; j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
        public int RiskCalc()
        {
            int count = 0;
            for (int X = 0; X <= target[0]; X++)
                for (int Y = 0; Y <= target[1]; Y++)
                    count += map[Y, X];
            return count;
        }
        int GetGeoIndex(int X, int Y)
        {
            if (Y == 0 && X == 0)
                return 0;
            if (X == target[0] && Y == target[1])
                return 0;
            if (Y == 0)
                return X * 16807;
            if (X == 0)
                return Y * 48271;
            else
                return GetErosion(X - 1, Y) * GetErosion(X, Y - 1);
        }
        int GetErosion(int X, int Y)
        {
            if (erosionmap[Y, X] != -1)
                return erosionmap[Y, X];

            int tor = (GetGeoIndex(X, Y) + depth) % 20183;
            erosionmap[Y, X] = tor;
            return tor;
        }
    }
    public class Player
    {
        public int Mark;
        public int X;
        public int Y;
        public int Tool; // 0 - torch, 1 - climbing, 2 - none
        // 0 - 0 , 1
        // 1 - 1 , 2
        // 2 - 2 , 0
        public Player(int tool, int x, int y, int mark)
        {
            X = x;
            Y = y;
            Mark = mark;
            Tool = tool;
        }
    }
}