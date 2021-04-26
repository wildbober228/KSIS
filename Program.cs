using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UDP
{
    class Program
    {
        static string remoteAddress; 
        static int remotePort;
        static int localPort;

        static int countDiagrams = 0;

        static bool SERVER = false;

        static void Main(string[] args)
        {
            try
            {

                int index = Int32.Parse(Console.ReadLine());

                if(index == 1)
                {
                    remoteAddress = "127.0.0.1";
                    remotePort = 8002;
                    localPort = 8001;
                    SERVER = true;

                }
                else
                {
                    remoteAddress = "127.0.0.1";
                    remotePort = 8001;
                    localPort = 8002;
                }

                

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            
                receiveThread.Start();
                Console.WriteLine("Write");
                string message = Console.ReadLine(); // сообщение для отправки
                if (!SERVER)
                {
                    SendMessage(message); // отправляем сообщение
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void SendMessage(string message)
        {
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки сообщений
            try
            {
                while (countDiagrams != 10)
                {
                   
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    byte[] newData = new byte[data.Length + 1];
                    newData[0] = (byte) countDiagrams;
                    Console.WriteLine(newData[0]);
                    data.CopyTo(newData, 1);
                    int numberOfSentBytes = sender.Send(newData, newData.Length, remoteAddress, remotePort); // отправка
                    Console.WriteLine("Отправлено байт " + numberOfSentBytes);
                    countDiagrams++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }

        private static void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(localPort); // UdpClient для получения данных
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    int indexDigram = data[0];
                    byte[] newData = new byte[data.Length + 1];
                    data.CopyTo(newData, 1);
                    Console.WriteLine("......................");
                    Console.WriteLine("Получен пакет "+ indexDigram);                    
                    string message = Encoding.Unicode.GetString(newData);
                    Console.WriteLine("Данные пакета: {0}", message);
                    Console.WriteLine("Принято байт " + newData.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}
