using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient;

class Program
{
    static void Main(string[] args)
    { 
        // DNS
        IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 7777);
        
        // Socket 설정

        while (true)
        {
            try
            {
                Socket socket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(localEndPoint);
                Console.WriteLine("Socket connected to {0}:{1}", localEndPoint.Address, socket.RemoteEndPoint?.ToString());


                // Send
                byte[] sendBuffer = Encoding.UTF8.GetBytes("Hello World");
                int sendBytes = socket.Send(sendBuffer);

                // Receive
                byte[] receiveBuffer = new byte[1024];
                int receiveBytes = socket.Receive(receiveBuffer);
                string message = Encoding.UTF8.GetString(receiveBuffer, 0, receiveBytes);
                Console.WriteLine($"[Server]: {message}");

                // 나간다
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            Thread.Sleep(2000);
        }
    }
}

