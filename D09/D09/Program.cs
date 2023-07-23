namespace D09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int nrplayers = 452; // input
            int maxmarbleval = 70784 * 100; // input

            Node current = new Node(0);

            current.next = current;
            current.prev = current;

            int currentplayer = 0;
            int currentmarble = 1;

            long[] players = new long[nrplayers];
            while(currentmarble < maxmarbleval)
            {
                if(currentmarble == maxmarbleval / 100)
                {
                    Console.WriteLine("Part 1 solution:");
                    Console.WriteLine(players.Max());
                }
                if (currentmarble % 23 == 0)
                {
                    players[currentplayer] += currentmarble;

                    for(int i = 0; i < 7; i++)
                    {
                        current = current.prev;
                    }
                    Node bef = current.prev;
                    Node next = current.next;

                    players[currentplayer] += current.Val;
                    bef.next = next;
                    next.prev = bef;
                    current = next;
                }
                else
                {
                    Node next = current.next;
                    Node nextafternext = current.next.next;

                    Node toadd = new Node(currentmarble);

                    toadd.next = nextafternext;
                    toadd.prev = next;
                    toadd.prev.next = toadd;
                    toadd.next.prev = toadd;
                    current = toadd;
                }
                currentmarble++;
                currentplayer++;
                currentplayer %= nrplayers;
            }
            Console.WriteLine("Part 2 solution");
            Console.WriteLine(players.Max());
        }
    }
    public class Node
    {
        public int Val;
        public Node? next;
        public Node? prev;
        public Node(int val)
        {
            Val = val;
        }
        public override string ToString()
        {
            return Val.ToString();
        }
    }
}