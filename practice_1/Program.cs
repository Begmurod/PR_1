using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Collections.Specialized;

namespace practice_1
{
    class Program
    {//Входная фукция для запуска сервера
        static void Main(string[] args)
        {//Создается 2 переменные которые ссылаются в конфиг на адреса и порта 
            string sAttr;                             
            sAttr = ConfigurationManager.AppSettings.Get("Key0");
            string sAttr2;                             
            sAttr2 = ConfigurationManager.AppSettings.Get("Key1");
            NameValueCollection sAll;
            sAll = ConfigurationManager.AppSettings;
            Server server = new Server(Convert.ToInt32(sAttr),sAttr2 );
            server.Start();
         //
         //если цикл while "правда" то выполняется действие, под 
            while (true)
            {
                string command;
                command = Console.ReadLine();
                if(command == "Выход")
                {
                    server.Stop();
                    break;
                }

                if(command == "Перезапуск")
                {
                    server.Stop();
                    server.Start();
                }
            }         
        }
    }
}
