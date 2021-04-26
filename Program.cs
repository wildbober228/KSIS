using System;

namespace TCP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1 - SERVER ; 2 - CLIENT");
            int type = Int32.Parse(Console.ReadLine());
            if (type == 1)
            {
                Server server = new Server();
            }
            else
            {
                Client client = new Client();
            }
        }
    }
}
