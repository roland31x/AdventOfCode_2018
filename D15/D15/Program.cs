using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace D15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map(@"..\..\..\input.txt", 3);
            map.CalcScore(out int result);
            Console.WriteLine("Part 1 solution:");         
            Console.WriteLine(result);
            
            int elfAD = 4;
            bool found = false;
            while (!found)
            {
                Map special = new Map(@"..\..\..\input.txt", elfAD);
                found = special.CalcScore(out result);
                elfAD++;
            }
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(result);
        }
    }
    public class Map
    {
        static List<int[]> dirs = new List<int[]>()
        {
            new int[] { -1, 0 }, // up        // reading order up-left-right-down
            new int[] { 0, -1 }, // left
            new int[] { 0, 1 }, // right
            new int[] { 1, 0 }, // down           
        };
        public int[,] map;
        public Entity[,] entities;
        public List<Entity> eList = new List<Entity>();
        int n { get { return map.GetLength(0); } }
        int m { get { return map.GetLength(1); } }
        public Map(string file, int elfAD)
        {
            string[] lines = File.ReadAllLines(file);
            map = new int[lines.Length, lines[0].Length];
            entities = new Entity[lines.Length, lines[0].Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for(int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '#')
                        map[i, j] = -1;
                    else if (lines[i][j] == 'E')
                    {
                        Elf elf = new Elf(i, j, elfAD);
                        entities[i, j] = elf;
                        map[i, j] = -1;
                        eList.Add(elf);
                    }                     
                    else if (lines[i][j] == 'G')
                    {
                        Goblin gob = new Goblin(i, j);
                        entities[i, j] = gob;
                        map[i, j] = -1;
                        eList.Add(gob);
                    }                     
                }
            }
        }
        public bool CalcScore(out int result)
        {
            int rounds = 0;
            bool done = false;
            while (!done)
            {              
                if(PlayRound(ref done)) // fact that a whole round of moves has to complete to means we have to check after each hit if game is done or not, if it's done mid round then round shouldn't count
                    rounds++;
            }
            int sum = 0;
            foreach (var e in eList.Where(x => x.isAlive))
            {
                sum += e.HP;
            }
            result = rounds * sum; 

            return eList.Where(x => x.Type == 0 && !x.isAlive).Count() == 0;
        }
        public bool PlayRound(ref bool done)
        {
            for(int i = 0; i < n; i++)
            {
                if (done)
                    return false;
                for(int j = 0; j < m; j++)
                {
                    if (done)
                        return false;
                    if (entities[i,j] != null && !entities[i, j].HasTakenTurn)
                    {
                        Entity current = entities[i, j];
                        TryMove(current);
                        TryAttack(current);
                        current.HasTakenTurn = true;
                        done = !(eList.Where(x => x.Type == 0 && x.isAlive).Count() > 0 && eList.Where(x => x.Type == 1 && x.isAlive).Count() > 0);
                    }
                }
            }

            foreach (Entity entity in entities)
                if (entity != null)
                    entity.HasTakenTurn = false;

            return true;
        }
        public void TryMove(Entity current)
        {
            foreach (int[] dir in dirs)
                if (entities[current.I + dir[0], current.J + dir[1]] != null && entities[current.I + dir[0], current.J + dir[1]].Type != current.Type)
                    return;

            int[,] BFS = new int[n, m];
            for(int i = 0; i < n; i++)
                for(int j = 0; j < m; j++)
                    BFS[i, j] = map[i, j];

            List<int[]> targets = new List<int[]>();
            foreach (Entity e in eList)
            {
                if (e == current || e.Type == current.Type || !e.isAlive)
                    continue;
                foreach (int[] dir in dirs)
                    if (BFS[e.I + dir[0], e.J + dir[1]] != -1)
                        targets.Add(new int[] { e.I + dir[0], e.J + dir[1] });
            }

            BFS[current.I, current.J] = 1;
            Queue<int[]> bfs = new Queue<int[]>();
            bfs.Enqueue(new int[] { current.I, current.J });
            while(bfs.Count > 0)
            {
                int[] pos = bfs.Dequeue();
                foreach (int[] dir in dirs)
                {
                    if(BFS[pos[0] + dir[0], pos[1] + dir[1]] == 0)
                    {
                        BFS[pos[0] + dir[0], pos[1] + dir[1]] = BFS[pos[0], pos[1]] + 1;
                        bfs.Enqueue(new int[] { pos[0] + dir[0], pos[1] + dir[1] });
                    }                  
                }
            }

            int mindist = int.MaxValue;
            foreach (int[] t in targets)
            {
                if(BFS[t[0], t[1]] > 0)             
                    if (BFS[t[0], t[1]] < mindist)
                        mindist = BFS[t[0], t[1]];
            }
            if (mindist == int.MaxValue)
                return;
            int[] actualtarget = new int[] { -1, -1 };
            bool done = false;
            for(int i = 0; i < n; i++)
            {
                if (done)
                    break;
                for(int j = 0; j < m; j++)
                {
                    if (done)
                        break;
                    foreach (int[] t in targets)
                    {
                        if (done)
                            break;
                        if(i == t[0] && j == t[1])
                        {
                            foreach (int[] dir in dirs)
                            {
                                if (done)
                                    break;
                                if (BFS[t[0], t[1]] == mindist)
                                {
                                    actualtarget[0] = t[0];
                                    actualtarget[1] = t[1];
                                    done = true;
                                }
                            }
                        }
                    }
                }
            }

            bfs.Clear();
            BFS = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    BFS[i, j] = map[i, j];
                }
            }
            BFS[actualtarget[0], actualtarget[1]] = 1;
            bfs.Enqueue(new int[] { actualtarget[0], actualtarget[1] });
            while (bfs.Count > 0)
            {
                int[] pos = bfs.Dequeue();
                foreach (int[] dir in dirs)
                {
                    if (BFS[pos[0] + dir[0], pos[1] + dir[1]] == 0)
                    {
                        BFS[pos[0] + dir[0], pos[1] + dir[1]] = BFS[pos[0], pos[1]] + 1;
                        bfs.Enqueue(new int[] { pos[0] + dir[0], pos[1] + dir[1] });
                    }
                }
            }

            int min = int.MaxValue;
            foreach (int[] dir in dirs)
            {
                if (BFS[current.I + dir[0], current.J + dir[1]] > 0 && BFS[current.I + dir[0], current.J + dir[1]] < min)
                    min = BFS[current.I + dir[0], current.J + dir[1]];
            }
            int newI = -1;
            int newJ = -1;
            foreach (int[] dir in dirs)
            {
                if (BFS[current.I + dir[0], current.J + dir[1]] == min)
                {
                    newI = current.I + dir[0];
                    newJ = current.J + dir[1];
                    break;
                }
            }

            map[current.I, current.J] = 0;
            entities[current.I, current.J] = null;
            current.I = newI;
            current.J = newJ;
            map[current.I, current.J] = -1;
            entities[current.I, current.J] = current;

        }
        public void TryAttack(Entity current)
        {
            Entity target = null;
            foreach (int[] dir in dirs)
            {
                if (entities[current.I + dir[0], current.J + dir[1]] == null)
                    continue;
                Entity postarget = entities[current.I + dir[0], current.J + dir[1]];
                if (postarget != null && postarget.Type != current.Type)
                {
                    if (target == null)
                        target = postarget;
                    else if (target.HP > postarget.HP)
                        target = postarget;
                }                 
            }
            if (target == null)
                return;
            else
                target.HP -= current.AD;

            if (!target.isAlive)
            {
                entities[target.I, target.J] = null;
                map[target.I, target.J] = 0;
            }
        }
    }
    public abstract class Entity
    {
        public int I;
        public int J;
        public bool HasTakenTurn = false;
        public int HP = 200;
        public int AD = 3;
        public int Type { get; protected set; }
        public bool isAlive { get { return HP > 0; } }
        public Entity(int i, int j)
        {
            I = i;
            J = j;
        }
    }
    public class Elf : Entity
    {
        public Elf(int i, int j, int elfdmg) : base(i, j)
        {
            Type = 0;
            AD = elfdmg;
        }          
    }
    public class Goblin : Entity
    {
        public Goblin(int i, int j) : base(i, j)
        {
            Type = 1;
        }
    }
}