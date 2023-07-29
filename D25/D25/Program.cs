namespace D25
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines(@"..\..\..\input.txt");
            List<Star> stars = new List<Star>();
            List<Constellation> constellations = new List<Constellation>();
            for(int i = 0; i < input.Length; i++)
            {
                string[] coords = input[i].Split(',');
                int x = int.Parse(coords[0]);
                int y = int.Parse(coords[1]);
                int z = int.Parse(coords[2]);
                int w = int.Parse(coords[3]);
                Star star = new Star(new Point4D(x, y, z, w));
                bool found = false;
                foreach(Star s in stars)
                {
                    int dist = star.Pos.Dist(s.Pos);
                    if (!found && dist <= 3)
                    {
                        s.PartOf.Stars.Add(star);
                        star.PartOf = s.PartOf;
                        found = true;
                    }
                    else if(found && dist <= 3 && s.PartOf != star.PartOf) // merge them
                    {
                        constellations.Remove(star.PartOf);
                        foreach(Star st in star.PartOf.Stars)
                        {
                            s.PartOf.Stars.Add(st);
                        }
                        foreach(Star st in s.PartOf.Stars)
                        {
                            st.PartOf = s.PartOf;
                        }
                    }
                }
                stars.Add(star);
                if (!found)
                {
                    Constellation newone = new Constellation();
                    constellations.Add(newone);
                    newone.Stars.Add(star);
                    star.PartOf = newone;                                 
                }
            }
            Console.WriteLine("Final day solution:");
            Console.WriteLine(constellations.Count);
        }
    }
    public class Constellation
    {
        public List<Star> Stars = new List<Star>();
        public Constellation()
        {

        }
    }
    public class Star
    {
        public Constellation? PartOf;
        public Point4D Pos { get; }
        public Star(Point4D pos)
        {
            Pos = pos;
        }
        public override string ToString()
        {
            return $"{Pos.x},{Pos.y},{Pos.z},{Pos.w}";
        }
    }
    public class Point4D
    {
        public int x;
        public int y;
        public int z;
        public int w;
        public Point4D(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public int Dist(Point4D other) => Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z) + Math.Abs(w - other.w);
    }
}