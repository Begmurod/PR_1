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
        // точка подключения порта и ип адреса
        private IPEndPoint IpPoint { get; set; }
        //Работает цикл или нет
        public bool Work { get; set; }
        //Сокет класс через которую работает соединения 
        private Socket Socket { get; set; }
        //Метод клиент конструктор заедает точку подключения порт и ип адрес
        public Client(int port, string address)
        {
            IpPoint = new IPEndPoint(IPAddress.Parse(address), port);          
        }
        //Метод запускающий клиент и два потока обработки данных приема и отправки
        public void Start()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Connect(IpPoint);
            Console.WriteLine(DateTime.Now +  " Соединение установленно.");
            Work = true;
            //поток для оправки сообщений
            Task processingSend = new Task(ProcessingSend);
            processingSend.Start();
            //поток для приема сообщений
            Task processingReceive = new Task(ProcessingReceive);
            processingReceive.Start();
        }
        //метод отключащий клиент
        public void Stop()
        {
            Socket.Close();
            Work = false;
        }
        //функция для отправки сообщений
        private void ProcessingSend()
        {// обработка ощибок
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
        //фукция для потока четения сообщений
        private void ProcessingReceive()
        {//Обработка ощибок
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
