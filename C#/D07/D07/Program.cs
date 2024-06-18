using System.Text;

namespace D07
{
    internal class Program
    {
        static void Main(string[] args)
        {            
            Part1();

            Step.List.Clear();

            Part2();         
        }
        public static void Part1()
        {
            using (StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine()!;
                    string stepneeded = line.Split(' ')[1];
                    string newunlock = line.Split(" ")[7];
                    Step.GetStep(stepneeded).Unlocks.Add(Step.GetStep(newunlock));
                    Step.GetStep(newunlock).LockLevel++;
                }
            }

            List<Step> initial = Step.List.Where(x => x.LockLevel == 0).ToList();
            StringBuilder sb = new StringBuilder();
            PriorityQueue<Step, string> queue = new PriorityQueue<Step, string>();
            foreach (Step start in initial)
            {
                queue.Enqueue(start, start.ID);
            }

            while (queue.Count > 0)
            {
                Step step = queue.Dequeue();
                sb.Append(step.ID);
                foreach (Step s in step.Unlocks)
                {
                    s.LockLevel--;
                    if (s.LockLevel == 0)
                        queue.Enqueue(s, s.ID);
                }
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(sb.ToString());
        }
        public static void Part2()
        {
            
            using (StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine()!;
                    string stepneeded = line.Split(' ')[1];
                    string newunlock = line.Split(" ")[7];
                    Step.GetStep(stepneeded).Unlocks.Add(Step.GetStep(newunlock));
                    Step.GetStep(newunlock).LockLevel++;
                }
            }

            List<Step> initial = Step.List.Where(x => x.LockLevel == 0).ToList();
            PriorityQueue<Step, string> queue = new PriorityQueue<Step, string>();
            int amount = Step.List.Count - initial.Count;
            List<Worker> workers = new List<Worker>() { new Worker(), new Worker(), new Worker(), new Worker(), new Worker() };

            int minute = 0;

            foreach (Step start in initial)
            {
                queue.Enqueue(start, start.ID);
            }
            int amountdone = 0;
            while(amountdone < amount)
            {
                foreach(Worker worker in workers)
                {
                    if (worker.isWorking)
                    {
                        worker.Passed++;
                        if(worker.Passed == worker.Needed)
                        {
                            worker.isWorking = false;
                            amountdone++;
                            foreach(Step s in worker.WorkingOn.Unlocks)
                            {
                                s.LockLevel--;
                                if (s.LockLevel == 0)
                                {
                                    queue.Enqueue(s, s.ID);
                                    amountdone++;
                                }
                                    
                            }
                        }
                    }
                }
                foreach(Worker worker in workers)
                {
                    if (!worker.isWorking && queue.Count > 0)
                        worker.Work(queue.Dequeue());
                }
                minute++;
            }
            while(workers.Where(w => w.isWorking).Any())
            {
                foreach (Worker worker in workers)
                {
                    if (worker.isWorking)
                    {
                        worker.Passed++;
                        if (worker.Passed == worker.Needed)
                        {
                            worker.isWorking = false;
                            amountdone++;
                            foreach (Step s in worker.WorkingOn.Unlocks)
                            {
                                s.LockLevel--;
                                if (s.LockLevel == 0)
                                {
                                    queue.Enqueue(s, s.ID);
                                    amountdone++;
                                }

                            }
                        }
                    }
                }
                foreach (Worker worker in workers)
                {
                    if (!worker.isWorking && queue.Count > 0)
                        worker.Work(queue.Dequeue());
                }
                minute++;
            }
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(minute - 1);
        }      
    }
    public class Worker
    {
        public Step WorkingOn;
        public int Passed;
        public int Needed;
        public bool isWorking = false;
        public Worker() 
        {

        }
        public void Work(Step s)
        {
            isWorking = true;
            WorkingOn = s;
            Passed = 0;
            Needed = s.SecVal;
        }
    }
    public class Step
    {
        public static List<Step> List = new List<Step>();
        public static Step GetStep(string id)
        {
            foreach(Step step in List)
            {
                if(step.ID == id)
                    return step;
            }
            return new Step(id);
        }


        public string ID;
        public int SecVal;
        public List<Step> Unlocks = new List<Step>();
        public int LockLevel = 0;
        public Step(string ID)
        {
            this.ID = ID;
            SecVal = ID.ToCharArray()[0] - 'A' + 1 + 60;
            List.Add(this);
        }
        public override string ToString()
        {
            return ID;
        }
    }
}