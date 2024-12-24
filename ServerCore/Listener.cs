using System.Net;
using System.Net.Sockets;

namespace ServerCore;

public class Listener
{
    private Socket? _socket;
    Func<Session> _sessionFactory;
    
    public void Init(IPEndPoint endPoint, Func<Session> sessionFactory)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _sessionFactory = sessionFactory;
        
        _socket.Bind(endPoint);
        
        _socket.Listen(10);

        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.Completed += new EventHandler<SocketAsyncEventArgs>(this.OnAcceptedCompleted);
        RegisterAccept(args);
    }

    private void OnAcceptedCompleted(object? sender, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            Session session = _sessionFactory.Invoke(); 
            session.Start(args.AcceptSocket);
            session.OnConnected(args.AcceptSocket.RemoteEndPoint);
        }
        else
        {
            Console.WriteLine("Socket Error: " + args.SocketError.ToString());
        }
        
        this.RegisterAccept(args);
    }

    private void RegisterAccept(SocketAsyncEventArgs args)
    {
        args.AcceptSocket = null;
        
        bool pending = _socket.AcceptAsync(args);
        if (!pending) this.OnAcceptedCompleted(null, args);
    }
}