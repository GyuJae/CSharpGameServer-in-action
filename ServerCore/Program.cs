using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore;

internal abstract class Program
{
    private static readonly Listener _listener = new Listener();

    static void Main(string[] args)
    {
        // DNS
        IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 7777);
        
        try
        {
            _listener.Init(localEndPoint, () => new GameSession());
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