using System.Net;
using System.Text;

namespace ServerCore;

class GameSession: Session
{
    public override void OnConnected(EndPoint endPoint)
    {
        Console.WriteLine($"Connected to server {endPoint}");
        
        byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to the server!");
        this.Send(sendBuff);
            
        Thread.Sleep(1000);
            
        this.Disconnect();
    }

    public override void OnReceive(ArraySegment<byte> buffer)
    {
        string message = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
        Console.WriteLine("[Client] " + message);        }

    protected override void OnSend(int numOfBytes)
    {
        Console.WriteLine($"Transferred Bytes: {numOfBytes}");
    }

    protected override void OnDisconnected(EndPoint endPoint)
    {
        Console.WriteLine($"Disconnected from {endPoint}");
    }
}