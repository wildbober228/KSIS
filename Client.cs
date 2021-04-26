using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP
{

    public class Client
    {
        
        static int port = 8005; 
        static string address = "127.0.0.1"; 
        static int countPakets = 11;
        bool start = true;
        public Client()
        {
            try
            {
                Console.Write("Введите сообщение:");
                string message = Console.ReadLine();
                for (int i = 0; i < countPakets; i++)
                {
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
                socket.Connect(ipPoint);
                    byte[] data;
                    if (start == true)
                    {
                        string message1 = "10";
                        data = Encoding.Unicode.GetBytes(message1);
                        socket.Send(data);
                    }
                    else
                    {
                        data = Encoding.Unicode.GetBytes(message);
                        socket.Send(data);
                    }
                
            
                data = new byte[256]; 
                StringBuilder builder = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                Console.WriteLine("ответ сервера: " + builder.ToString());

               
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
              }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }

}
