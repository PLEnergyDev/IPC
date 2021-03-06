using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketComm;

using static Cmd;

public class FPipe : IDisposable, IAsyncDisposable, IEnumerable<Cmd>, IAsyncEnumerable<Cmd>
{
    ILogger<FPipe> Logger { get; }
    Socket S { get; }

    public FPipe(string file, ILogger<FPipe>? logger = null)
    {

        this.Logger = logger ?? (LoggerFactory.Create(b => {
            b.AddConsole();
        }).CreateLogger<FPipe>());
        Logger.LogInformation("Creating socket endpoint");
        UnixDomainSocketEndPoint ep = new UnixDomainSocketEndPoint(file);
        Logger.LogInformation("Creating socket");
        if (File.Exists(file))
        {
            S = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            Logger.LogInformation("Connecting...");
            S.Connect(ep);
            ShakeHands(S);
        }
        else
        {
            Socket FileSocket;
            Logger.LogInformation("Serving...");
            FileSocket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            FileSocket.Bind(ep);
            FileSocket.Listen();
            Logger.LogInformation("Listening");
            S = FileSocket.Accept();
            FileSocket.Disconnect(false);
            FileSocket.Dispose();
            ReceiveHandshake(S);
        }
        Logger.LogInformation("Connected..");
        

    }
    const Int32 MagicHandshakeValue = 25;
    Cmd CmdFromBB(byte[] buffer)
    {
        sbyte result = (sbyte)buffer[0];
        return (Cmd)result;
    }
    byte[] BBFromCmd(Cmd c)
    {
        var b = new byte[] { (byte)c };
        Logger.LogTrace($"{nameof(BBFromCmd)}: {c} ==> {Convert.ToHexString(b)} ({b.Length}b)");
        return b;
    }
    void ValidateHandshake(byte[] data)
    {
        Int32 result = BitConverter.ToInt32(data);
        //L.LogInformation($"[{nameof(ShakeHands)}]: {MagicHandshakeValue}/{result}");
        byte[] rev = new byte[data.Length];
        Array.Copy(data, rev, rev.Length);
        Array.Reverse(rev);
        if (result == MagicHandshakeValue)
        {
            Logger.LogInformation($"Handshake OK {MagicHandshakeValue}/{result}");
            return;
        }

        int revResult = BitConverter.ToInt32(rev);
        if (revResult == MagicHandshakeValue)
        {
            Logger.LogError($"Other side uses {(BitConverter.IsLittleEndian ? "BigEndian" : "LittleEndian")} -- {MagicHandshakeValue}/{result}");
            return;
        }
        throw new InvalidDataException("Bad handshake..." + result);
    }

    public async Task ShakeHandsAsync(Socket socket)
    {
        var data = BitConverter.GetBytes(MagicHandshakeValue);
        await socket.SendAsync(data, SocketFlags.None);
        await socket.ReceiveAsync(data, SocketFlags.None);
        ValidateHandshake(data);
    }

    public void ShakeHands(Socket socket)
    {
        var data = BitConverter.GetBytes(MagicHandshakeValue);
        socket.Send(data, SocketFlags.None);
        socket.Receive(data, SocketFlags.None);
        ValidateHandshake(data);
    }

    public void ReceiveHandshake(Socket socket)
    {
        byte[] data=new byte[4];
        Int32 result;
        Logger.LogInformation("Waiting for handshake");
        socket.Receive(data, SocketFlags.None);
        result = BitConverter.ToInt32(data);
        Logger.LogInformation($"[{nameof(ReceiveHandshake)}] Read: {Convert.ToHexString(data)}/{result}");
        BitConverter.GetBytes(MagicHandshakeValue).CopyTo(data.AsMemory());
        socket.Send(data);
        ValidateHandshake(data);
    }

    public async Task<Cmd> ReadCmdAsync(CancellationToken ct = default)
    {
        try
        {
            var buffer = new byte[1];
            var count = await S.ReceiveAsync(buffer, SocketFlags.None, ct);
            return CmdFromBB(buffer);
        }
        catch(Exception e) {
            Logger.LogError(e.Message);
            return Error; 
        }
    }
    public async Task<bool> WriteCmdAsync(Cmd cmd, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogTrace($"Sending {cmd}");
            var data = BBFromCmd(cmd);
            Logger.LogTrace($"data: {Convert.ToHexString(data)}");
            var sent = await S.SendAsync(data, SocketFlags.None, cancellationToken);
            Logger.LogTrace($"Sent: {sent}");
            return true;
        }
        catch(Exception ex)
        {
            Logger.LogError(ex.Message);
            return false;
        }
    }

    public Cmd ReadCmd()
    {
        try
        {
            byte[] buffer = new byte[1];
            S.Receive(buffer);
            return CmdFromBB(buffer);
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            return Error;
        }
    }
    public bool WriteCmd(Cmd cmd)
    {
        try
        {
            S.Send(BBFromCmd(cmd));
            return true;
        }
        catch (Exception e){
            Logger.LogError(e.Message);
            return false;
        }
    }

    public void Dispose()
    {
        S?.Dispose();
        GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
        S?.Dispose();
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    public IEnumerator<Cmd> GetEnumerator()
    {
        Cmd r;
        do
        {
            r = ReadCmd();
            yield return r; 
        } while (r > Exit); 
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public async IAsyncEnumerator<Cmd> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        Cmd r;
        do
        {
            r = await ReadCmdAsync(cancellationToken);
            yield return r;
        } while(r > Exit);
    }
}
