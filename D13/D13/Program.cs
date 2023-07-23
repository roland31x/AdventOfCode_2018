namespace D13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CarriageMap map = new CarriageMap(@"..\..\..\input.txt");
            Tuple<int, int> P2Res = map.Result(out Tuple<int, int> P1Res);
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(P1Res.Item1 + "," + P1Res.Item2);
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(P2Res.Item1 + "," + P2Res.Item2);
        }
    }
    public class CarriageMap
    {
        public static List<int[]> dirs = new List<int[]>()
        {
            new int[] { -1,  0 }, // up
            new int[] {  0,  1 }, // right
            new int[] {  1,  0 }, // down
            new int[] {  0, -1 }, // left
        };
        public int[,] paths;
        public Car[,] cars;
        int AliveCars = 0;
        public CarriageMap(string file)
        {
            string[] lines = File.ReadAllLines(file);
            paths = new int[lines.Length, lines[0].Length];
            cars = new Car[lines.Length, lines[0].Length];
            for(int i = 0; i < lines.Length; i++)
            {
                for(int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '|' || lines[i][j] == '-')
                        paths[i, j] = 1;
                    else if (lines[i][j] == '/')
                        paths[i, j] = 2;
                    else if (lines[i][j] == '\\')
                        paths[i, j] = 3;
                    else if (lines[i][j] == '+')
                        paths[i, j] = 4;
                    else if (lines[i][j] == 'v' || lines[i][j] == '^' || lines[i][j] == '<' || lines[i][j] == '>')
                    {
                        paths[i, j] = 1;
                        cars[i, j] = new Car(GetDir(lines[i][j]), i, j);
                        AliveCars++;
                    }
                }
            }
        }
        public Tuple<int,int> Result(out Tuple<int,int> P1Result)
        {
            P1Result = new Tuple<int, int>(-1, -1);
            bool P1Res = false;
            bool done = false;
            while (!done)
            {
                for(int i = 0; i < paths.GetLength(0); i++)
                {
                    if (done)
                        break;
                    for(int j = 0; j < paths.GetLength(1); j++)
                    {
                        if (done)
                            break;
                        if (cars[i,j] != null && !cars[i,j].HasMoved)
                        {
                            Car current = cars[i,j];
                            int currentI = current.I;
                            int currentJ = current.J;
                            int newI = currentI + dirs[current.Dir][0];
                            int newJ = currentJ + dirs[current.Dir][1];
                            if (paths[newI, newJ] == 2) // means '/'
                            {
                                if (current.Dir == 0 || current.Dir == 2)
                                    current.Dir++;
                                else
                                    current.Dir--;
                            }
                            if (paths[newI,newJ] == 3) // means '\'
                            {
                                if (current.Dir == 0 || current.Dir == 2)
                                    current.Dir--;
                                else
                                    current.Dir++;
                            }
                            if (paths[newI, newJ] == 4) // intersection has different states
                            {
                                if(current.State == 0)
                                    current.Dir--;
                                if (current.State == 2)
                                    current.Dir++;
                                current.State++;
                            }
                            cars[current.I, current.J] = null;
                            current.I = newI;
                            current.J = newJ;
                            if (cars[newI, newJ] != null)
                            {
                                AliveCars -= 2;
                                cars[newI, newJ] = null;
                                if (!P1Res)
                                {
                                    P1Result = new Tuple<int, int>(newJ, newI);
                                    P1Res = true;
                                }
                                if (AliveCars == 1)
                                    done = true;
                                continue;    
                            }
                            cars[newI, newJ] = current;
                            current.HasMoved = true;
                        }
                    }
                }

                foreach(Car c in cars)
                {
                    if (c != null)
                        c.HasMoved = false;
                }
            }

            foreach (Car c in cars)
                if (c != null)
                    return new Tuple<int, int>(c.J, c.I);

            throw new Exception("rip"); // this won't happen, hopefully
        }
        static int GetDir(char c)
        {
            if (c == '^')
                return 0;
            if (c == '>')
                return 1;
            if (c == 'v')
                return 2;
            if (c == '<')
                return 3;
            return -1;
        }
    }
    public class Car
    {
        public int I;
        public int J;
        int _d;
        int _s;
        public int Dir { get { return _d; } set { _d = value; if (_d < 0) { _d += 4; } if (_d >= 4) { _d -= 4; } } }
        public int State { get { return _s; } set { _s = value; if (_s < 0) { _s += 3; } if (_s >= 3) { _s -= 3; } } }
        public bool HasMoved = false;
        public Car(int dir, int i, int j)
        {
            I = i;
            J = j;
            Dir = dir;
        }
    }
}