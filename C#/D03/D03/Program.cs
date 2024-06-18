namespace D03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FabricMap map = new FabricMap(@"..\..\..\input.txt");
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(map.Overlap());
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(map.GetNoOverlap());
        }
    }
    public class FabricMap
    {
        int[,] map = new int[1500, 1500];
        string fileread;
        public FabricMap(string file)
        {
            fileread = file;
            using(StreamReader sr = new StreamReader(file))
            {
                while(!sr.EndOfStream)
                {
                    string[] tokens = sr.ReadLine()!.Split(' ');
                    int ID = int.Parse(tokens[0].Replace("#",""));
                    int xpos = int.Parse(tokens[2].Split(',')[0]);
                    int ypos = int.Parse(tokens[2].Split(',')[1].Replace(":", ""));
                    int xsize = int.Parse(tokens[3].Split('x')[0]);
                    int ysize = int.Parse(tokens[3].Split('x')[1]);
                    for(int i = ypos; i < ypos + ysize; i++)
                    {
                        for(int j = xpos; j < xpos + xsize; j++)
                        {
                            map[i, j] += 1;
                        }
                    }
                    
                }
            }
        }
        public int Overlap()
        {
            int count = 0;
            for(int i = 0; i < map.GetLength(0); i++)
                for(int j = 0; j < map.GetLength(1); j++)
                    if (map[i, j] > 1)
                        count++;
            return count;
        }
        public int GetNoOverlap()
        {
            using (StreamReader sr = new StreamReader(fileread))
            {
                while (!sr.EndOfStream)
                {
                    string[] tokens = sr.ReadLine()!.Split(' ');
                    int ID = int.Parse(tokens[0].Replace("#", ""));
                    int xpos = int.Parse(tokens[2].Split(',')[0]);
                    int ypos = int.Parse(tokens[2].Split(',')[1].Replace(":", ""));
                    int xsize = int.Parse(tokens[3].Split('x')[0]);
                    int ysize = int.Parse(tokens[3].Split('x')[1]);
                    bool ok = true;
                    for (int i = ypos; i < ypos + ysize && ok; i++)
                        for (int j = xpos; j < xpos + xsize && ok; j++)
                            if (map[i, j] > 1)
                                ok = false;
                    if (ok)
                        return ID;
                }
            }
            return -1;
        }
    }
}