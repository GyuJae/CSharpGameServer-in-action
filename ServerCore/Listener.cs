using System.Net;
using System.Net.Sockets;

namespace ServerCore;

public class Listener
{
    private Socket? _socket;
    private Action<Socket> _onAcceptHandler;

    public void Init(IPEndPoint endPoint, Action<Socket> onAcceptHandler)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _onAcceptHandler = onAcceptHandler;
        
        _socket.Bind(endPoint);
        
        _socket.Listen(10);

        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.Completed += new EventHandler<SocketAsyncEventArgs>(this.onAccepCompleted);
        RegisterAccept(args);
    }

    private void onAccepCompleted(object? sender, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            if (args.AcceptSocket != null) this._onAcceptHandler?.Invoke(args.AcceptSocket);
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
        if (!pending) this.onAccepCompleted(null, args);
    }
}