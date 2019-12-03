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
        private Socket Socket { get; set; }
        private Server Server;
        private int Id { get; set; }
        private int Tasks = 0;

        public User(Server Server, Socket Socket, int Id)
        {
            this.Socket = Socket;
            this.Server = Server;
            this.Id = Id;
        }

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

        private double Reading()
        {
            byte[] receiveBuffer = new byte[8];
            Socket.Receive(receiveBuffer, receiveBuffer.Length, SocketFlags.None);
            Console.WriteLine(DateTime.Now +  " Пользователь: " + Id + " Запрос получен."+ BitConverter.ToDouble(receiveBuffer, 0));
            return Math.Round(BitConverter.ToDouble(receiveBuffer, 0));
        }

        private void Working(double number)
        {
            Thread.Sleep(5000);

            Answer(BitConverter.GetBytes((int)number));
        }

        private void Answer(byte[] answer)
        {
            for (int i = 0; i < Tasks; i++)
            {
                Socket.Send(answer);
                Console.WriteLine(DateTime.Now+ " Пользователь " + Id + " отправлен ответ на запрос.");
            }
            Tasks = 0;
        }

        private void Exit()
        {
            Socket.Close();
        }
        
    }
}
