using SocketComm;
using static SocketComm.Cmd;

if (args.Length < 1)
{
    Console.Error.WriteLine("usage: [socket]");
    return;
}
FPipe p = new FPipe(args[0]);
p.Connect();
p.ExpectCmd(Ready);
// Always communicate big endian
var reverse = BitConverter.IsLittleEndian;
int i = short.MinValue;
while (i < short.MaxValue)
{
    p.SendValue(++i, (val)=>
    {
        var result =  BitConverter.GetBytes(val);
        if (reverse)
        {
            result = result.Reverse().ToArray();
        }
        return result;
    });
    Console.WriteLine($"Server sending {i}");
    if (p.ReadCmd() == Receive)
    {
        i = p.ReceiveValue((buf) =>
        {
            if (reverse)
                buf = buf.Reverse().ToArray();
            return BitConverter.ToInt32(buf, 0);
        });
        Console.WriteLine($"Server received {i}");
    }
    else
    {
        break;
    }
}

p.WriteCmd(Exit);
p.Dispose();
