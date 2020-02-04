using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Bean;
using UnityEngine;


public class ClientControl
{
    private Socket clientSocket;

    public ClientControl()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void Connect(string ip, int port)
    {
        clientSocket.Connect(ip, port);
        Console.WriteLine("Connect success");

        Thread threadReceive = new Thread(Receive);
        threadReceive.IsBackground = true;
        threadReceive.Start();
    }

    private void Receive()
    {
        while (true)
        {
            byte[] msg = new byte[1024];
            int msgLen = clientSocket.Receive(msg);
            // Debug.Log("server says: " + Encoding.UTF8.GetString(msg, 0, msgLen));
            InputQueue.parse(Encoding.UTF8.GetString(msg, 0, msgLen));
        }
    }

    public void Send(string msg)
    {
        clientSocket.Send(Encoding.UTF8.GetBytes(msg));
    }
}