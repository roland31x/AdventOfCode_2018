using System.Text.RegularExpressions;

namespace D23
{
    internal class Program
    {       
        static void Main(string[] args)
        {
            BeaconMap beaconmap = new BeaconMap(@"..\..\..\input.txt");
            beaconmap.Part1();
        }
    }
    public class BeaconMap
    {
        public static Regex V3 = new Regex(@"[-]?[0-9]+,[-]?[0-9]+,[-]?[0-9]+");
        List<Beacon> beacons = new List<Beacon>();
        Beacon best = null;
        public BeaconMap(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    string buffer = sr.ReadLine()!;
                    int radius = int.Parse(buffer.Split('=').Last());
                    string pos = V3.Match(buffer).Value;
                    int x = int.Parse(pos.Split(',')[0].Trim());
                    int y = int.Parse(pos.Split(',')[1].Trim());
                    int z = int.Parse(pos.Split(',')[2].Trim());
                    Beacon toadd = new Beacon(x, y, z, radius);
                    if (best == null)
                        best = toadd;
                    else if (best.radius < toadd.radius)
                        best = toadd;
                    beacons.Add(toadd);
                }
            }
        }
        public void Part1()
        {
            int count = 0;
            foreach (Beacon b in beacons)
            {
                if (b.coords.DistTo(best.coords) <= best.radius)
                    count++;
            }
            Console.WriteLine(count);
        }
    }
    public class Point3D
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public int DistTo(Point3D other) => Math.Abs(this.X - other.X) + Math.Abs(this.Y - other.Y) + Math.Abs(this.Z - other.Z);
    }
    public class Beacon
    {
        public Point3D coords { get; }
        public int radius { get; }
        public Beacon(int x,int y, int z, int radius)
        {
            coords = new Point3D(x, y, z);
            this.radius = radius;
        }
    }
    
}