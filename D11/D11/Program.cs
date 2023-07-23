using System.Drawing;

namespace D11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int SN = 7403; // your input
            FuelGrid map = new FuelGrid(SN);
            
            Tuple<int, int, int> P1Res = map.GetBestSquare(3, out _);
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(P1Res.Item1 + "," + P1Res.Item2);

            Tuple<int, int, int> P2Res = map.GetOverallSquare();
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(P2Res.Item1 + "," + P2Res.Item2 + "," + P2Res.Item3);
        }
    }
    public class FuelGrid
    {
        int[,] mat = new int[300, 300];
        int SN;
        public FuelGrid(int sn)
        {
            SN = sn;
            for(int i = 0; i < 300; i++)
            {
                for(int j = 0; j < 300; j++)
                {
                    int rackID = i + 1 + 10;
                    int pwr = rackID * (j + 1);
                    pwr += SN;
                    pwr *= rackID;
                    pwr = (pwr % 1000) / 100;
                    pwr -= 5;
                    mat[i, j] = pwr;
                }
            }
        }
        public Tuple<int,int,int> GetOverallSquare()
        {
            Tuple<int, int, int> toreturn = new Tuple<int, int, int>(-1, -1, 0);
            int bestscoresofar = 0;
            for (int i = 1; i < 299; i++)
            {
                Tuple<int, int, int> check = GetBestSquare(i, out int checkscore);
                if (check.Item1 == -1 && check.Item2 == -1)
                    break;
                if (checkscore > bestscoresofar)                  
                {
                    bestscoresofar = checkscore;
                    toreturn = check;
                }
            }
            return toreturn;
        }
        public Tuple<int,int,int> GetBestSquare(int size, out int bestscore)
        {
            bestscore = 0;
            Tuple<int, int, int> toreturn = new Tuple<int, int, int>(-1, -1, size);
            for(int i = 0; i < 300 - size; i++)
            {
                for(int j = 0; j < 300 - size; j++)
                {
                    int score = 0;
                    for(int k = i; k < i + size; k++)
                        for (int l = j; l < j + size; l++)
                            score += mat[k, l];
                    if(score > bestscore)
                    {
                        bestscore = score;
                        toreturn = new Tuple<int, int, int>(i + 1, j + 1, size);
                    }
                }
            }

            return toreturn;
        }
    }
}