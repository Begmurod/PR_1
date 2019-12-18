using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace practice_1
{
    class Server
    {
        //Ип адрес и порт сервера 
        private IPEndPoint IpPoint { get; set; }
        //Работает цикл или нет
        public bool Work { get; set; }
        //Ид подключаемого клиента
        private int Id { get; set; }
        //Конструктор в котором мы задаем адрес и порт сервера 
        public Server(int port, string address)
        {
            IpPoint = new IPEndPoint(IPAddress.Parse(address), port);
        }
        //Запускает сервер
        public void Start()
        {
            Work = true;
            //Создает поток в котором принимается новые подключеных клиентов
            Task listener = new Task(Listener);
            listener.Start();
        }
        //Останавлмвает работу сервера
        public void Stop()
        {
            Work = false;
        }
        //Фукция для потока в котором принимается новые подключеных клиентов
        private void Listener()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(IpPoint);
            listener.Listen(40);
            //Цикл обработки новых  подключений к серверу
            while (Work)
            {   
                Id++;
                User user = new User(this, listener.Accept(), Id);
                //Создаета поток обработки новых клиентов
                Task userTask = new Task(user.Processing);
                userTask.Start();
            }
            //Закрывает сокет приема клиентов
            listener.Close();
        }
    }

}
