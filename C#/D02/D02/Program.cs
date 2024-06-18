using System.Text;

namespace D02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> TwoID = new List<string>();
            List<string> ThreeID = new List<string>();
            List<string> All = new List<string>();
            using(StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                while(!sr.EndOfStream)
                {
                    string id = sr.ReadLine()!;
                    All.Add(id);
                    int[] fq = new int['z' - 'a' + 1];
                    for(int i = 0; i < id.Length; i++)
                    {
                        fq[id[i] - 'a']++;
                    }
                    for(int i = 0; i < fq.Length; i++)
                    {
                        if (fq[i] == 2)
                            if(!TwoID.Contains(id))
                                TwoID.Add(id);
                            
                        if (fq[i] == 3)
                            if(!ThreeID.Contains(id))
                                ThreeID.Add(id);
                    }
                }
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(TwoID.Count * ThreeID.Count);


            bool found = false;
            for(int i = 0; i < All.Count; i++)
            {
                for(int j = i + 1; j < All.Count; j++)
                {
                    int notequal = 0;
                    int idx = -1;
                    for(int k = 0; k < All[i].Length; k++)
                    {
                        if (All[i][k] != All[j][k])
                        {
                            notequal++;
                            idx = k;
                        }
                            
                    }
                    if(notequal == 1)
                    {
                        StringBuilder sb = new StringBuilder();
                        for(int k = 0; k < All[i].Length; k++)
                        {
                            if (k == idx)
                                continue;
                            sb.Append(All[i][k]);
                        }
                        Console.WriteLine("Part 2 solution:");
                        Console.WriteLine(sb.ToString());
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }
            
        }
    }
}