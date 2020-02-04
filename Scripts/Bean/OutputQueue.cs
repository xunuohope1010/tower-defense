using System;
using System.Collections.Generic;

namespace Bean
{
    public class OutputQueue
    {
        public static ClientControl client;
        public static Stack<Node> finalPath = new Stack<Node>();
        
        public static void connect()
        {
            client = new ClientControl();
            client.Connect("127.0.0.1", 8080);
            client.Send("game start!");
        }
        public static void sendData()
        {
            // client.Send();
            List<String>lists = new List<string>();
            foreach (var coord in finalPath)
            {
                lists.Add(coord.GridPosition.x+" "+coord.GridPosition.y);
            }
            client.Send(String.Join(", ", lists.ToArray()));
        }
    }
}