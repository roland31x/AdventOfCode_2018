using System.Reflection.Emit;

namespace D19
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyProgram program = new MyProgram(@"..\..\..\input.txt");
            program.Run(optimized: false);
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(program.Regvalue(0));
            program = new MyProgram(@"..\..\..\input.txt");
            program.OverrideReg(0, 1);
            program.Run(optimized: true);
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(program.Regvalue(0));
        }
    }
    public class MyProgram
    {
        public static List<string> ops = new List<string>() { "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "seti", "setr", "gtri", "gtir", "gtrr", "eqri", "eqir", "eqrr" };

        int[] regs = new int[6];
        int IP;
        int Driver { get { return regs[IP]; } set { regs[IP] = value; } }
        List<string> lines;
        public MyProgram(string file)
        {
            lines = File.ReadAllLines(file).ToList();
            IP = int.Parse(lines[0].Split(' ')[1]);
            lines.RemoveAt(0);
        }
        public void OverrideReg(int reg, int value)
        {
            regs[reg] = value;
        }
        public int Regvalue(int reg)
        {
            return regs[reg];
        }
        public void Run(bool optimized)
        {
            int times = 0;
            while(Driver < lines.Count)
            {
                Execute(lines[Driver]);
                Driver++;
                times++;
                if(optimized && times > 100) // so the program is similar to AOC 2017 day 13, which was an anti-primality counter between two numbers, this one has a number in register 2, for which it checks every possibility of multiplication wether it's a divisor or not, if it's a divisor it adds it to register 0 value
                {
                    int sumofdivisors = 0;
                    for(int i = 1; i <= regs[2]; i++)
                    {
                        if (regs[2] % i == 0)
                        {
                            sumofdivisors += i;
                        }
                            
                    }
                    regs[0] = sumofdivisors;
                    break;
                }
            }
        }
        public void Execute(string command)
        {
            string[] cmd = command.Split(' ');
            string com = cmd[0];
            int target1 = int.Parse(cmd[1]);
            int target2 = int.Parse(cmd[2]);
            int resulttarget = int.Parse(cmd[3]);

            Execute(com, target1, target2, resulttarget);
        }
        void Execute(string com, int target1, int target2, int resulttarget)
        {

            switch (com)
            {
                case "addr": ADDR(target1, target2, resulttarget); break;
                case "addi": ADDI(target1, target2, resulttarget); break;
                case "mulr": MULR(target1, target2, resulttarget); break;
                case "muli": MULI(target1, target2, resulttarget); break;
                case "banr": BANR(target1, target2, resulttarget); break;
                case "bani": BANI(target1, target2, resulttarget); break;
                case "borr": BORR(target1, target2, resulttarget); break;
                case "bori": BORI(target1, target2, resulttarget); break;
                case "seti": SETI(target1, target2, resulttarget); break;
                case "setr": SETR(target1, target2, resulttarget); break;
                case "gtir": GTIR(target1, target2, resulttarget); break;
                case "gtri": GTRI(target1, target2, resulttarget); break;
                case "gtrr": GTRR(target1, target2, resulttarget); break;
                case "eqir": EQIR(target1, target2, resulttarget); break;
                case "eqri": EQRI(target1, target2, resulttarget); break;
                case "eqrr": EQRR(target1, target2, resulttarget); break;
                default:
                    throw new Exception("unkown command");
            }
        }
        void ADDR(int t1, int t2, int target)
        {
            regs[target] = regs[t1] + regs[t2];
        }
        void ADDI(int t1, int t2, int target)
        {
            regs[target] = regs[t1] + t2;
        }
        void MULR(int t1, int t2, int target)
        {
            regs[target] = regs[t1] * regs[t2];
        }
        void MULI(int t1, int t2, int target)
        {
            regs[target] = regs[t1] * t2;
        }
        void BANR(int t1, int t2, int target)
        {
            regs[target] = regs[t1] & regs[t2];
        }
        void BANI(int t1, int t2, int target)
        {
            regs[target] = regs[t1] & t2;
        }
        void BORR(int t1, int t2, int target)
        {
            regs[target] = regs[t1] | regs[t2];
        }
        void BORI(int t1, int t2, int target)
        {   
            regs[target] = regs[t1] | t2;
        }
        void SETR(int t1, int t2, int target)
        {
            regs[target] = regs[t1];
        }
        void SETI(int t1, int t2, int target)
        {
            regs[target] = t1;
        }
        void GTIR(int t1, int t2, int target)
        {
            if (t1 > regs[t2])
                regs[target] = 1;
            else
                regs[target] = 0;
        }
        void GTRI(int t1, int t2, int target)
        {
            if (regs[t1] > t2)
                regs[target] = 1;
            else
                regs[target] = 0;
        }
        void GTRR(int t1, int t2, int target)
        {
            if (regs[t1] > regs[t2])
                regs[target] = 1;
            else
                regs[target] = 0;
        }
        void EQIR(int t1, int t2, int target)
        {
            if (t1 == regs[t2])
                regs[target] = 1;
            else
                regs[target] = 0;
        }
        void EQRI(int t1, int t2, int target)
        {
            if (regs[t1] == t2)
                regs[target] = 1;
            else
                regs[target] = 0;
        }
        void EQRR(int t1, int t2, int target)
        {
            if (regs[t1] == regs[t2])
                regs[target] = 1;
            else
                regs[target] = 0;
        }
    }
}