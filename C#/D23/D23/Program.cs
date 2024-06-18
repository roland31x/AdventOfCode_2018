using System.Text.RegularExpressions;

namespace D23
{
    internal class Program
    {       
        static void Main(string[] args)
        {
            BeaconMap beaconmap = new BeaconMap(@"..\..\..\input.txt");
            beaconmap.Part1();
            beaconmap.Part2();
        }
    }
    public class BeaconMap
    {
        public static Regex V3 = new Regex(@"[-]?[0-9]+,[-]?[0-9]+,[-]?[0-9]+");
        List<Beacon> beacons = new List<Beacon>();
        Beacon best = null;
        int minx = int.MaxValue;
        int miny = int.MaxValue;
        int minz = int.MaxValue;
        int maxx = int.MinValue;
        int maxy = int.MinValue;
        int maxz = int.MinValue;
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
                    if(x < minx) minx = x;
                    if(x > maxx) maxx = x;
                    int y = int.Parse(pos.Split(',')[1].Trim());
                    if(y < miny) miny = y;
                    if(y > maxy) maxy = y;
                    int z = int.Parse(pos.Split(',')[2].Trim());
                    if(z < minz) minz = z;
                    if(z > maxz) maxz = z;

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
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(count);
        }
        public void Part2()
        {
            int division = 10000000;
            int pwr = 1;
            int startx = minx / division - 1;
            int starty = miny / division - 1;
            int startz = minz / division - 1;
            int endx = maxx / division + 1;
            int endy = maxy / division + 1;
            int endz = maxz / division + 1;

            Point3D best = null;
            int disttocenter = int.MaxValue;
            int bestcount = 0;

            while (division > 0)
            {
                for(int i = startx; i <= endx; i++)
                {
                    for(int j = starty; j <= endy; j++)
                    {
                        for(int k = startz; k <= endz; k++)
                        {
                            int count = 0;
                            
                            Point3D actual = new Point3D(i , j , k );
                            foreach (Beacon b in beacons)
                            {
                                Point3D bactual = new Point3D(b.coords.X / division, b.coords.Y / division, b.coords.Z / division);
                                if (bactual.DistTo(actual) <= (b.radius / division))
                                    count++;
                            }
                            int centertohere = actual.DistTo(new Point3D(0, 0, 0));
                            if (count >= bestcount)
                            {
                                if(count == bestcount)
                                {
                                    if(centertohere <= disttocenter)
                                    {
                                        best = actual;
                                        bestcount = count;
                                        disttocenter = centertohere;
                                    }
                                }
                                else
                                {
                                    best = actual;
                                    bestcount = count;
                                    disttocenter = centertohere;
                                }
                            }
                        }
                    }
                }
                division /= 10;
                pwr *= 10;
                if (division == 0)
                    break;
                startx = (best.X - 2) * 10;
                starty = (best.Y - 2) * 10;
                startz = (best.Z - 2) * 10;
                endx = (best.X + 2) * 10;
                endy = (best.Y + 2) * 10;
                endz = (best.Z + 2) * 10;

                best = null;
                disttocenter = int.MaxValue;
                bestcount = 0;

            }
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(best.X + best.Y + best.Z);
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