using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP
{
    public class Server
    {
        static int port = 8005; 
        static int countPakets = 1;
        static int ccountPakets = 0;
        static int actualPakets = 0;
        float actualReadBytes = 0;
        bool start = true;
        public Server()
        {

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

     
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {

                listenSocket.Bind(ipPoint);


                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                Stopwatch stopwatch = new Stopwatch();
                while (countPakets > 0)
                {
                                    
                    stopwatch.Start();
                    Socket handler = listenSocket.Accept();

                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; 
                    byte[] data = new byte[256]; 

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);
                    float readBytes = (data.Length * 8);                    
                    actualReadBytes = readBytes / stopwatch.ElapsedMilliseconds;               
                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    if(start == false)
                    {
                        actualPakets++;
                        countPakets--;

                        stopwatch.Stop();
                        Console.WriteLine("Скорость передачи {0} kb/s", actualReadBytes);

                        string message = "ваше сообщение доставлено";
                        data = Encoding.Unicode.GetBytes(message);
                        handler.Send(data);
                    }
                      

                    if (start == true)
                    {
                        start = false;
                        countPakets = Int32.Parse(builder.ToString());
                        ccountPakets = countPakets;
                        Console.WriteLine("Всего пакетов = "+ countPakets);
                        
                    }                  

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                   
                }

                Console.WriteLine("Ожидалось пакетов "+ ccountPakets + " Пришло пакетов " + actualPakets);
                UdpClient sender = new UdpClient();
               


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
