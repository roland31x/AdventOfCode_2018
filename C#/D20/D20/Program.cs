namespace D20
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string rg = File.ReadAllText(@"..\..\..\input.txt");
            RegexMap map = new RegexMap(rg);
            //map.Show();
            Tuple<int,int> result = map.Solve();
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(result.Item1);
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(result.Item2);
        }
    }
    public class RegexMap
    {
        static Dictionary<char, int[]> dirs = new Dictionary<char, int[]>() { { 'N', new int[] { -2, 0 } }, { 'W', new int[] { 0, -2 } }, { 'S', new int[] { 2, 0 } }, { 'E', new int[] { 0, 2 } } };
        static Dictionary<char, int[]> inv = new Dictionary<char, int[]>() { { 'S', new int[] { -2, 0 } }, { 'E', new int[] { 0, -2 } }, { 'N', new int[] { 2, 0 } }, { 'W', new int[] { 0, 2 } } };
        int[,] map = new int[300, 300];
        int startX;
        int startY;
        public RegexMap(string regex)
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    map[i, j] = 1;
            startX = map.GetLength(0) / 2;
            startY = map.GetLength(1) / 2;
            map[startX, startY] = 0;
            int currentX = startX;
            int currentY = startY;
            Stack<char> dir = new Stack<char>();
            int dr = 1;
            while(dr < regex.Length - 1)
            {
                if (regex[dr] == '|' || regex[dr] == ')')
                {
                    while(dir.Peek() != '(')
                    {
                        int[] popped = inv[dir.Pop()];

                        currentX += popped[1] / 2;
                        currentY += popped[0] / 2;
                        map[currentY, currentX] = 0;
                        currentX += popped[1] / 2;
                        currentY += popped[0] / 2;
                        map[currentY, currentX] = 0;
                    }
                    if (regex[dr] == ')')
                    {
                        dir.Pop();
                    }
                    dr++;
                }
                else
                {
                    dir.Push(regex[dr]);
                    if (regex[dr] == '(')
                    {
                        dr++;
                        continue;
                    }
                    int[] togo = dirs[regex[dr]];
                    currentX += togo[1];
                    currentY += togo[0];
                    map[currentY, currentX] = 0;
                    dr++;
                }              
            }
            while (dir.Any())
            {
                int[] popped = inv[dir.Pop()];

                currentX += popped[1] / 2;
                currentY += popped[0] / 2;
                map[currentY, currentX] = 0;
                currentX += popped[1] / 2;
                currentY += popped[0] / 2;
                map[currentY, currentX] = 0;
            }
        }
        public Tuple<int,int> Solve()
        {
            Queue<int[]> lee = new Queue<int[]>();

            int[] start = new int[] { startY, startX };           
            int[,] mark = new int[map.GetLength(0), map.GetLength(1)];
            mark[start[0], start[1]] = 1; // 1 offset so we don't loop forever
            lee.Enqueue(start);
            while(lee.Count > 0)
            {
                int[] deq = lee.Dequeue();
                foreach (int[] dir in dirs.Values)
                {
                    int[] newpos = new int[] { deq[0] + dir[0] / 2, deq[1] + dir[1] / 2 };
                    TryEnqueue(newpos, mark[deq[0],deq[1]], lee, mark);
                }
            }

            int biggest = 0;
            int over1k = 0;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (mark[i, j] > biggest)
                        biggest = mark[i, j];
                    if ((mark[i, j] - 1) / 2 >= 1000 && mark[i,j] % 2 == 1) 
                        over1k++;
                }
            }

            return new Tuple<int,int>((biggest - 1) / 2, over1k);
        }
        void TryEnqueue(int[] newpos, int mark, Queue<int[]> lee, int[,] marks)
        {
            int y = newpos[0];
            int x = newpos[1];
            if (y < 0 || y >= map.GetLength(0) || x < 0 || x >= map.GetLength(1))
                return;
            if (marks[y, x] != 0 || map[y,x] == 1)
                return;
            marks[y, x] = mark + 1;
            lee.Enqueue(newpos);
        }
        public void Show()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 0)
                        Console.Write('.');
                    else Console.Write('#');
                }                    
                Console.WriteLine();
            }               
        }
    }
}