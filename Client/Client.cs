using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        private IPEndPoint IpPoint { get; set; }
        public bool Work { get; set; }
        private Socket Socket { get; set; }

        public Client(int port, string address)
        {
            IpPoint = new IPEndPoint(IPAddress.Parse(address), port);          
        }

        public void Start()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Connect(IpPoint);
            Console.WriteLine(DateTime.Now +  " Соединение установленно.");
            Work = true;

            Task processingSend = new Task(ProcessingSend);
            processingSend.Start();

            Task processingReceive = new Task(ProcessingReceive);
            processingReceive.Start();
        }

        public void Stop()
        {
            Socket.Close();
            Work = false;
        }
        
        private void ProcessingSend()
        {
            try
            {
               // Отправка: дробного значения(из диапазона 20 - 30).
                while (Work)
                {
                    Socket.Send(BitConverter.GetBytes(Convert.ToDouble(new Random().Next(20, 30)*0.99)));
                    Console.WriteLine(DateTime.Now +  " Запрос отправлен.");
                    Thread.Sleep(2700);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Work = false;
            }

        }

        private void ProcessingReceive()
        {
            try
            {
                while (Work)
                {
                    byte[] answer = new byte[100];
                    Socket.Receive(answer, answer.Length, SocketFlags.None);

                    Console.WriteLine(DateTime.Now +  " Полученное число: " + BitConverter.ToInt32(answer,0));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Work = false;
            }
        }
    }
}
