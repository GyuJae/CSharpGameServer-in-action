using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore;

internal abstract class Program
{
    static void Main(string[] args)
    {
        // DNS
        IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 7777);
        
        // Socket
        Socket listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        
        try
        {
            // 교육
            listenSocket.Bind(localEndPoint);
        
            // backlog : 최대 대기수
            listenSocket.Listen(10);
            
            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
            
                // Client 입장
                Socket clientSocket = listenSocket.Accept();
            
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}