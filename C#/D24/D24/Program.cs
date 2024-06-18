using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace D24
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Simulation mysim = new Simulation(@"..\..\..\input.txt",0);
            mysim.Simulate(out int result);
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(result);
            bool ok = false;
            int boost = 1;
            while (!ok)
            {
                mysim = new Simulation(@"..\..\..\input.txt", boost);
                ok = mysim.Simulate(out result);
                boost++;
            }
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(result);
        }
    }
    public class Simulation
    {
        static Regex t = new Regex(@"\([^\]]*\)");
        static Regex d = new Regex(@"that does [0-9]+ [a-z]+ damage");
        List<Army> armies = new List<Army>();
        public Simulation(string file, int boost)
        {
            int armytype = 0;
            int nr = 1;
            using(StreamReader sr = new StreamReader(file))
            {
                sr.ReadLine();
                while (!sr.EndOfStream) 
                {
                    string line = sr.ReadLine();
                    if (line == string.Empty)
                    {
                        armytype = 1;
                        sr.ReadLine();
                        nr = 1;
                        continue;
                    }                       
                    int count = int.Parse(line.Split(' ')[0]);                    
                    int initiative = int.Parse(line.Split(' ').Last());

                    string dmg = d.Match(line).Value;
                    int hp = int.Parse(line.Split(' ')[4]);
                    int damage = int.Parse(dmg.Split(' ')[2]);
                    string type = dmg.Split(' ')[3];

                    Unit myunit = new Unit(hp, type, damage + (armytype == 0 ? boost : 0));
                    Army newArmy = new Army(nr,armytype, count, myunit, initiative);
                    if (line.Contains('('))
                    {
                        string[] traits = t.Match(line).Value.Replace("(", "").Replace(")", "").Split(';');
                        for (int i = 0; i < traits.Length; i++)
                        {
                            string trait = traits[i].Trim().Split(' ')[0];
                            int isweak = 0;
                            if (trait == "weak")
                            {
                                isweak = 1;
                            }
                            traits[i] = traits[i].Replace($"{trait} to", "").Replace(",","").Trim();
                            foreach(string tr in traits[i].Split(' '))
                            {
                                if (isweak == 1)
                                    newArmy.Weaknesses.Add(tr);
                                else
                                    newArmy.Immunities.Add(tr);
                            }
                        }
                    }
                    nr++;
                    armies.Add(newArmy);
                    
                }
            }
        }
        public bool Simulate(out int count)
        {
            while(armies.Where(x => x.Type == 0 && x.UnitCount > 0).Any() && armies.Where(x => x.Type == 1 && x.UnitCount > 0).Any())
            {
                List<Army> targetselection = new List<Army>();
                foreach(Army army in armies)
                    targetselection.Add(army);
                while(targetselection.Count > 0)
                {
                    Army current = targetselection.First();
                    foreach (Army army in targetselection)
                    {
                        if(army.Effective >= current.Effective)
                        {
                            if (army.Initiative > current.Initiative || army.Effective > current.Effective)
                                current = army;
                        }
                    }
                    targetselection.Remove(current);
                    int maxdamage = 0;
                    Army? toconsider = null;
                    foreach(Army army in armies.Where(x => x.Type != current.Type && !x.wasTargeted))
                    {
                        int currentdamage = current.UnitCount * current.Unit.Damage;
                        if (army.Weaknesses.Contains(current.Unit.type))
                            currentdamage *= 2;
                        else if (army.Immunities.Contains(current.Unit.type))
                            currentdamage = -1;
                        if(currentdamage >= maxdamage)
                        {
                            if(toconsider == null)
                            {
                                toconsider = army;
                                maxdamage = currentdamage;
                            }
                            else if(currentdamage > maxdamage)
                            {
                                toconsider = army;
                                maxdamage = currentdamage;
                            }   
                            else if(toconsider.Effective <= army.Effective)
                            {
                                if(toconsider.Effective < army.Effective)
                                {
                                    toconsider = army;
                                    maxdamage = currentdamage;
                                }
                                else if(toconsider.Initiative < army.Initiative)
                                {
                                    toconsider = army;
                                    maxdamage = currentdamage;
                                }
                            }
                        }
                    }
                    if (toconsider != null)
                    {
                        toconsider.wasTargeted = true;
                        current.target = toconsider;
                    }                        
                }
                List<Army> attackorder = armies.Where(x => x.target != null).ToList();
                bool anycasualities = false;
                while(attackorder.Count > 0)
                {
                    Army toattack = attackorder.First();
                    foreach (Army army in attackorder)
                        if (army.Initiative > toattack.Initiative)
                            toattack = army;
                    attackorder.Remove(toattack);
                    int damage = toattack.Effective;
                    if (toattack.target.Weaknesses.Contains(toattack.Unit.type))
                        damage *= 2;
                    if (toattack.target.Immunities.Contains(toattack.Unit.type))
                        damage = 0;
                    int casualities = damage / toattack.target.Unit.HP;
                    toattack.target.UnitCount -= casualities;
                    if (casualities > 0)
                        anycasualities = true;

                    if(toattack.target.UnitCount <= 0)
                    {
                        attackorder.Remove(toattack.target);
                        armies.Remove(toattack.target);
                    }

                }
                if (!anycasualities) // this will softlock the sim into an unwinnable state, need to skip
                {
                    count = -1;
                    return false;
                }
                foreach(Army army in armies)
                {
                    army.wasTargeted = false;
                    army.target = null;
                }
            }
            
            count = 0;
            foreach(Army army in armies)
            {
                count += army.UnitCount;
            }
            if (armies.First().Type == 0)
                return true;
            return false;
        }
    }
    
    public class Unit
    {
        public int HP;
        public string type;
        public int Damage;
        public Unit(int hP, string type, int damage)
        {
            HP = hP;
            this.type = type;
            Damage = damage;
        }
    }
    public class Army
    {
        public int Type;
        int ID;
        public int UnitCount;
        public Unit Unit;
        public int Effective { get { return UnitCount * Unit.Damage; } }
        public List<string> Weaknesses = new List<string>();
        public List<string> Immunities = new List<string>();
        public int Initiative;
        public Army? target = null;
        public bool wasTargeted = false;

        public Army(int id, int type, int count, Unit u, int initiative)
        {
            ID = id;
            Type = type;
            Unit = u;
            UnitCount = count;
            Initiative = initiative;
        }
        public override string ToString()
        {
            return (Type == 0 ? "Immune" : "Infection") + " group " + ID;
        }
    }
}