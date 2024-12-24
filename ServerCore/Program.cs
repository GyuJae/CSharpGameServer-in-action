using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore;

internal abstract class Program
{
    private static readonly Listener _listener = new Listener();

    static void OnAcceptHandler(Socket clientSocket)
    {
        try
        {
            // Receive
            byte[] recvBuff = new byte[1024];
            int recvByte = clientSocket.Receive(recvBuff);
            string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvByte);
            Console.WriteLine($"[Client] {recvData}");
            
            // Send
            byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to the server!");
            clientSocket.Send(sendBuff);
            
            // 쫓아낸다
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    static void Main(string[] args)
    {
        // DNS
        IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 7777);
        
        try
        {
            _listener.Init(localEndPoint, OnAcceptHandler);
            Console.WriteLine("Waiting for a connection...");
 
            while (true)
            {
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}