using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Collections.Specialized;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string sAttr;            // Read a particular key from the config file                     
            sAttr = ConfigurationManager.AppSettings.Get("Key0");        


            {
                Client client = new Client(1000, "127.0.0.1");
                client.Start();

                while (true)
                {
                    string command;
                    command = Console.ReadLine();
                    if (command == "Выход")
                    {
                        client.Stop();
                        break;
                    }
                }
            }
        }
    }
}
    

