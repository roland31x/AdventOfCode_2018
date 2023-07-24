namespace D14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int input = 846601; // input

            Node start3 = new Node(3);
            Node start7 = new Node(7);
            start3.prev = start7;
            start3.next = start7;
            start7.prev = start3;
            start7.next = start3;
            Node start = start3;
            Node last = start7;
            Node elf1 = start3;
            Node elf2 = start7;

            int len = 2;
            int inputlen = input.ToString().Length;
            bool found = false;
            int foundidx = 0;
            Queue<int> q = new Queue<int>();
            for(int i = 0; i < input + 10 || !found; i++)
            {
                int combined = elf1.Val + elf2.Val;
                if(combined >= 10)
                {
                    last.next = new Node(1);
                    last.next.prev = last;
                    last = last.next;
                    start.prev = last;
                    last.next = start;
                    combined -= 10;
                    len++;
                    if (!found)
                    {
                        q.Enqueue(1);
                        if (q.Count == inputlen)
                        {
                            int res = 0;
                            foreach (int element in q)
                            {
                                res *= 10;
                                res += element;
                            }
                            if (res == input)
                            {
                                found = true;
                                foundidx = len - inputlen;
                            }
                            q.Dequeue();
                        }
                    }

                }
                last.next = new Node(combined);
                last.next.prev = last;
                last = last.next;
                start.prev = last;
                last.next = start;
                len++;
                if (!found)
                {
                    q.Enqueue(combined);
                    if (q.Count == inputlen)
                    {
                        int res = 0;
                        foreach (int element in q)
                        {
                            res *= 10;
                            res += element;
                        }
                        if (res == input)
                        {
                            found = true;
                            foundidx = len - inputlen;
                        }
                        q.Dequeue();
                    }
                }
                

                int elf1times = elf1.Val + 1;
                int elf2times = elf2.Val + 1;

                for (int times = 0; times < elf1times; times++)
                    elf1 = elf1.next;
                for (int times = 0; times < elf2times; times++)
                    elf2 = elf2.next;
            }
            Node current = start;
            for(int i = 0; i < input; i++)
            {
                current = current.next;
            }
            Console.WriteLine("Part 1 solution:");
            for(int i = 0; i < 10; i++)
            {
                Console.Write(current.Val);
                current = current.next;
            }
            Console.WriteLine();
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(foundidx);

        }
    }
    public class Node
    {
        public int Val;
        public Node? next;
        public Node? prev;
        public Node(int value)
        {
            Val = value;
        }
        public override string ToString()
        {
            return Val.ToString();
        }
    }
}