using static System.Net.Mime.MediaTypeNames;

namespace D16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string buffer = "";
            int count = 0;
            bool part1 = true;
            bool part2 = false;
            MyProgram.Init();
            MyProgram main = new MyProgram();
            using(StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                buffer = sr.ReadLine();
                while(!sr.EndOfStream)
                {
                    while (part1 && buffer != null && buffer != string.Empty)
                    {                      
                        string[] bef = buffer.Split(':')[1].Replace(" ","").Replace("[","").Replace("]","").Split(',');
                        int[] before = new int[4];
                        for (int i = 0; i < 4; i++)
                            before[i] = int.Parse(bef[i].Trim());
                        string command = sr.ReadLine();
                        string[] res = sr.ReadLine().Split(':')[1].Replace(" ", "").Replace("[", "").Replace("]", "").Split(',');
                        int[] result = new int[4];
                        for (int i = 0; i < 4; i++)
                            result[i] = int.Parse(res[i].Trim());
                        count += MyProgram.Test(before, command, result);
                        sr.ReadLine();
                        buffer = sr.ReadLine();
                    }
                    sr.ReadLine();

                    MyProgram.CalculateOpcodes();

                    part1 = false;
                    part2 = true;
                    
                    while(part2 && !sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        main.Execute(line);
                    }
                    
                }
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(count);
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(main.regs[0]);
        }
    }
    public class OpCode
    {
        public int OPID;
        public List<int> validops = new List<int>();
        public bool WasSolved = false;
        public OpCode(int id) 
        {
            OPID = id;
            for (int i = 0; i < 16; i++)
                validops.Add(i);
        }
    }
    public class MyProgram
    {
        public static List<string> ops = new List<string>() { "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "seti", "setr", "gtri", "gtir", "gtrr", "eqri", "eqir", "eqrr" };
        public static OpCode[] opcodes = new OpCode[16];
        public static void Init()
        {
            for (int i = 0; i < 16; i++)
                opcodes[i] = new OpCode(i);
        }
        public static void CalculateOpcodes()
        {
            while (opcodes.Where(x => x.validops.Count > 1).Count() > 0)
            {
                foreach(OpCode opc in opcodes)
                {
                    if(opc.validops.Count == 1 && !opc.WasSolved)
                    {
                        opc.WasSolved = true;
                        foreach(OpCode opc2 in opcodes)
                        {
                            if (opc == opc2)
                                continue;
                            if (opc2.validops.Contains(opc.validops[0]))
                                opc2.validops.Remove(opc.validops[0]);
                        }
                    }
                }
            }
        }
        public static int Test(int[] bef, string command, int[] res)
        {
            int count = 0;
            string[] cmd = command.Split(' ');
            int opcode = int.Parse(cmd[0]);
            int target1 = int.Parse(cmd[1]);
            int target2 = int.Parse(cmd[2]);
            int resulttarget = int.Parse(cmd[3]);
            List<int> valid = new List<int>();

            foreach (string s in ops)
            {
                MyProgram test = new MyProgram();
                for (int i = 0; i < 4; i++)
                    test.regs[i] = bef[i];               

                test.Execute(s, target1, target2, resulttarget);

                for(int i = 0; i < 4; i++)
                {
                    if (test.regs[i] != res[i])
                        break;
                    if (i == 3)
                    {
                        count++;
                        valid.Add(ops.IndexOf(s));
                    }                       
                }
            }           

            opcodes[opcode].validops = valid.Where(x => opcodes[opcode].validops.Contains(x)).ToList();
            

            if (count >= 3)
                return 1;
            return 0;            
        }

        public int[] regs = new int[4];
        public MyProgram() 
        {

        }
        public void Execute(string command)
        {
            string[] cmd = command.Split(' ');
            string com = cmd[0];
            if (int.TryParse(com, out int idx))
                com = ops[opcodes[idx].validops[0]];
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
        public void ADDR(int t1,int t2, int target)
        {
            regs[target] = regs[t1] + regs[t2];
        }
        public void ADDI(int t1, int t2, int target)
        {
            regs[target] = regs[t1] + t2;
        }
        public void MULR(int t1, int t2, int target)
        {
            regs[target] = regs[t1] * regs[t2];
        }
        public void MULI(int t1, int t2, int target)
        {
            regs[target] = regs[t1] * t2;
        }
        public void BANR(int t1, int t2, int target)
        {
            regs[target] = regs[t1] & regs[t2];
        }
        public void BANI(int t1, int t2, int target)
        {
            regs[target] = regs[t1] & t2;
        }
        public void BORR(int t1, int t2, int target)
        {
            regs[target] = regs[t1] | regs[t2];
        }
        public void BORI(int t1, int t2, int target)
        {
            regs[target] = regs[t1] | t2;
        }
        public void SETR(int t1, int t2, int target)
        {
            regs[target] = regs[t1];
        }
        public void SETI(int t1, int t2, int target)
        {
            regs[target] = t1;
        }
        public void GTIR(int t1, int t2, int target)
        {
            if (t1 > regs[t2])
                regs[target] = 1;
            else
                regs[target] = 0;
        }
        public void GTRI(int t1, int t2, int target)
        {
            if (regs[t1] > t2)
                regs[target] = 1;
            else
                regs[target] = 0;
        }
        public void GTRR(int t1, int t2, int target)
        {
            if (regs[t1] > regs[t2])
                regs[target] = 1;
            else
                regs[target] = 0;
        }
        public void EQIR(int t1, int t2, int target)
        {
            if (t1 == regs[t2])
                regs[target] = 1;
            else
                regs[target] = 0;
        }
        public void EQRI(int t1, int t2, int target)
        {
            if (regs[t1] == t2)
                regs[target] = 1;
            else
                regs[target] = 0;
        }
        public void EQRR(int t1, int t2, int target)
        {
            if (regs[t1] == regs[t2])
                regs[target] = 1;
            else
                regs[target] = 0;
        }
    }
}