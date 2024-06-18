namespace D08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] line = File.ReadAllText(@"..\..\..\input.txt").Split(' ');
            int driver = 2;
            Node start = new Node(int.Parse(line[0]), int.Parse(line[1]));
            DFS(start, ref driver, line);

            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(Node.GetMetaDataScore());
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(start.GetScore());
        }
        public static void DFS(Node current, ref int driver, string[] tree)
        {
            for(int i = 0; i < current.Children; i++)
            {
                Node child = new Node(int.Parse(tree[driver]), int.Parse(tree[driver + 1]));
                driver += 2;
                current.Nodes.Add(child);
                DFS(child, ref driver, tree);
            }
            for(int i = 0; i < current.MetaData; i++)
            {
                current.Data.Add(int.Parse(tree[driver]));
                driver++;
            }
        }
    }
    public class Node
    {
        static List<Node> all = new List<Node>();
        public static int GetMetaDataScore()
        {
            int sc = 0;
            foreach(Node n in all)
            {
                foreach(int val in n.Data)
                    sc += val;
            }
            return sc;
        }
        public int Children;
        public int MetaData;
        public List<int> Data = new List<int>();
        public List<Node> Nodes = new List<Node>();
        public Node(int c,int m)
        {
            Children = c;
            MetaData = m;
            all.Add(this);
        }
        public int GetScore()
        {
            int sc = 0;
            if(Children == 0)
            {
                foreach(int data in Data)
                    sc += data;               
            }
            else
            {
                foreach(int idx in Data)
                {
                    try
                    {
                        sc += Nodes[idx - 1].GetScore();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            return sc;
        }
    }
}