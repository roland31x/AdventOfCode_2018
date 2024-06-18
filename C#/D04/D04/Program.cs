using System.Text.RegularExpressions;

namespace D04
{
    internal class Program
    {
        public static Regex date = new Regex(@"[0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+");
        static void Main(string[] args)
        {
            List<Activity> activities = new List<Activity>();
            using(StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine()!;
                    string dt = date.Match(line).Value;
                    DateTime match = DateTime.Parse(dt);
                    activities.Add(new Activity(match, line));
                }
            }
            activities.Sort((x1,x2) => x1.date.CompareTo(x2.date));
            Guard Current;
            int currentmin = 0;
            bool isAsleep = false;
            int driver = 0;
            while(driver < activities.Count) 
            {
                currentmin = 0;
                Current = Guard.Get(int.Parse(activities[driver].activity.Split(' ')[3].Replace("#","")));
                driver++;
                while (driver < activities.Count && !activities[driver].activity.Contains("Guard"))
                {                  
                    if (activities[driver].activity.Contains("wakes"))
                    {
                        isAsleep = false;
                        for(; currentmin < activities[driver].date.Minute; currentmin++)
                        {
                            Current.activity[currentmin]++;
                            Current.total++;
                        }
                    }
                    else if (activities[driver].activity.Contains("falls"))
                    {
                        isAsleep = true;
                        for (; currentmin < activities[driver].date.Minute; currentmin++)
                        {
                            
                        }
                    }
                    driver++;    
                }
                for(; currentmin < 60; currentmin++)
                {
                    if(isAsleep)
                        Current.activity[currentmin]++;
                }
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(Guard.GetPart1());
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(Guard.GetPart2());
        }
    }
    public class Guard 
    {
        public static List<Guard> Guards = new List<Guard>();
        public static Guard Get(int ID)
        {
            foreach(Guard g in Guards)
                if(g.ID == ID)
                    return g;
            return new Guard(ID);
        }
        public int[] activity = new int[60];
        public int total = 0;
        public int ID;
        public Guard(int ID)
        {
            this.ID = ID;
            Guards.Add(this);
        }
        public static int GetPart1()
        {
            Guard best = Guards[0];
            foreach(Guard g in Guards)
            {
                if (g.total > best.total)
                    best = g;
            }
            int idx = 0;
            for(int i = 1; i < 60; i++)
            {
                if (best.activity[i] > best.activity[idx])
                    idx = i;
            }
            return idx * best.ID;
        }
        public static int GetPart2()
        {
            Guard best = Guards[0];
            int bestamount = 0;
            int minidx = -1;
            foreach (Guard g in Guards)
            {
                int idx = 0;
                for (int i = 1; i < 60; i++)
                {
                    if (g.activity[i] > g.activity[idx])
                        idx = i;
                }
                if (g.activity[idx] > bestamount)
                {
                    bestamount = g.activity[idx];
                    minidx = idx;
                    best = g;
                }
            }
            
            return minidx * best.ID;
        }
    }

    public class Activity
    {
        public DateTime date;
        public string activity;
        public Activity(DateTime date, string act)
        {
            activity = act;
            this.date = date;
        }
    }
}