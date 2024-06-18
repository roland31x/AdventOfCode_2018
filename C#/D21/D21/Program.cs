namespace D21
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyProgram prog = new MyProgram(@"..\..\..\input.txt");
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(prog.Run(Part1: true));
            prog = new MyProgram(@"..\..\..\input.txt");
            Console.WriteLine("Part 2 solution: ( will take a while to calculate, just be patient )"); // i really don't like reverse engineering these codes, it does has multiple loops inside eachother that keep applying bitwise operations on target register
            Console.WriteLine(prog.Run(Part1: false));
        }
    }
    public class MyProgram
    {
        public static List<string> ops = new List<string>() { "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "seti", "setr", "gtri", "gtir", "gtrr", "eqri", "eqir", "eqrr" };

        int[] regs = new int[6];
        int IP;
        int Driver { get { return regs[IP]; } set { regs[IP] = value; } }
        List<string> lines;
        List<int> equalitychecks = new List<int>();
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
        public int Run(bool Part1)
        {
            while (Driver < lines.Count)
            {
                if(Driver == 28 && Part1) // only part where register 0 value is checked, first time check goes if it returns true it means the program should halt, it is an equality check between reg 5 ( for me ) and 0 so reg 0 should contain what 5 has
                {
                    return regs[int.Parse(lines[Driver].Split(' ')[1])];                    
                }
                else if(Driver == 28)
                {
                    int toadd = regs[int.Parse(lines[Driver].Split(' ')[1])];
                    if (!equalitychecks.Contains(toadd))
                    {
                        equalitychecks.Add(toadd);
                    }
                    else
                    {
                        return equalitychecks.Last();
                    }
                    regs[int.Parse(lines[Driver].Split(' ')[3])] = 0;
                    Driver++;                   
                    continue;
                }
                Execute(lines[Driver]);
                Driver++;
            }

            return -1;
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