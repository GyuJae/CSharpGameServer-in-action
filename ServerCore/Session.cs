using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore;

public abstract class Session
{
    private Socket _socket;
    int _disconnected = 0;
    
    object _lock = new();
    Queue<byte[]> _sendQueue = new();
    List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

    SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
    SocketAsyncEventArgs _receiveArgs = new SocketAsyncEventArgs();


    public abstract void OnConnected(EndPoint endPoint);
    public abstract void OnReceive(ArraySegment<byte> buffer);
    protected abstract void OnSend(int numOfBytes);
    protected abstract void OnDisconnected(EndPoint endPoint);

    public void Start(Socket socket)
    {
        _socket = socket;
        _receiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
        _receiveArgs.SetBuffer(new byte[1024], 0, 1024);
        
        _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
        
        this.RegisterRecv();
    }

    public void Send(byte[] sendBuff)
    {
        lock (_lock)
        {
            _sendQueue.Enqueue(sendBuff);
            if (_pendingList.Count == 0)
            {
                RegisterSend();
            } 
        }
    }

    public void Disconnect()
    {
        if (Interlocked.Exchange(ref _disconnected, 1) == 1) return;
        if (_socket.RemoteEndPoint != null) this.OnDisconnected(_socket.RemoteEndPoint);
        _socket.Shutdown(SocketShutdown.Both);
        _socket.Close();
    }

    private void RegisterSend()
    { 
        while (_sendQueue.Count > 0)
        {
            byte[] buffer = _sendQueue.Dequeue();
            _pendingList.Add(new ArraySegment<byte>(buffer, 0, buffer.Length));
        }

        _sendArgs.BufferList = _pendingList;
        
       bool pending = _socket.SendAsync(_sendArgs);
       if (!pending) this.OnSendCompleted(null, _sendArgs);
    }

    private void OnSendCompleted(object? sender, SocketAsyncEventArgs args)
    {
        lock (_lock)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                    _sendArgs.BufferList = null;
                    _pendingList.Clear();
                   
                    this.OnSend(args.BytesTransferred);
                     
                    if (_sendQueue.Count > 0)
                    {
                        RegisterSend();
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("OnSendCompleted" + e);
                    throw;
                }
            }
            else
            {
                this.Disconnect();
            } 
        }
    }

    private void RegisterRecv()
    {
       bool pending =  _socket.ReceiveAsync(_receiveArgs);
       if(!pending) this.OnRecvCompleted(null, _receiveArgs);
    }
    
    private void OnRecvCompleted(object? sender, SocketAsyncEventArgs args)
    {
        if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
        {
            try
            {
                this.OnReceive(new ArraySegment<byte>(args.Buffer, args.Offset, args.BytesTransferred));
                this.RegisterRecv();
            }
            catch (Exception e)
            {
                Console.WriteLine("OnRecvCompleted" + e);
                throw;
            }
        }
        else
        {
            this.Disconnect();
        }
    }
}