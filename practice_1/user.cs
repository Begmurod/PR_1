using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace practice_1
{
    class User
    {
        //Сокет подключения клиенту
        private Socket Socket { get; set; }
        //Ссылка на класс сервера
        private Server Server;
        //Ид клиента
        private int Id { get; set; }
        //Количество принятых содинений во время обработки 
        private int Tasks = 0;
        //Конструктор в котором мы задаем сокет клиента его ид и сервер обработки 
        public User(Server Server, Socket Socket, int Id)
        {
            this.Socket = Socket;
            this.Server = Server;
            this.Id = Id;
        }
        //Фукция для потока в которой обрабатываются запросы клиентов 
        public void Processing()
        {
            try
            {
                while (Server.Work)
                {
                    double number = Reading();

                    if (Tasks == 0)
                    {
                        Tasks = 1;
                        Task task = new Task(() => Working(number));
                        task.Start();
                    }
                    else
                    {
                        Tasks++;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(DateTime.Now + " Пользователь: " + Id +" Ошибка: " + e.Message);
            }

            Exit();
        }
        //Фукция кторой читает запрос клиента 
        private double Reading()
        {
            byte[] receiveBuffer = new byte[8];
            Socket.Receive(receiveBuffer, receiveBuffer.Length, SocketFlags.None);
            Console.WriteLine(DateTime.Now +  " Пользователь: " + Id + " Запрос получен."+ BitConverter.ToDouble(receiveBuffer, 0));
            return Math.Round(BitConverter.ToDouble(receiveBuffer, 0));
        }
        //Фукция обработки запроса клиентов 
        private void Working(double number)
        {
            Thread.Sleep(5000);

            Answer(BitConverter.GetBytes((int)number));
        }
        //Фукция для оправки ответа клиенту 
        private void Answer(byte[] answer)
        {
            for (int i = 0; i < Tasks; i++)
            {
                Socket.Send(answer);
                Console.WriteLine(DateTime.Now+ " Пользователь " + Id + " отправлен ответ на запрос.");
            }
            Tasks = 0;
        }
        //Фукция которое закрывает соединение с клиентом 
        private void Exit()
        {
            Socket.Close();
        }
        
    }
}
