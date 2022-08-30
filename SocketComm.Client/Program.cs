using SocketComm;

if (args.Length < 1)
{
    Console.Error.WriteLine("usage: [socket]");
    return;
}
FPipe p = new FPipe(args[0]);
// Always communicate big endian
var reverse = BitConverter.IsLittleEndian;
p.Connect();
p.WriteCmd(Cmd.Ready);
Cmd c = p.ReadCmd();
while (c == Cmd.Receive)
{
    var i = p.ReceiveValue((buf) =>
    {
        if (reverse)
            buf = buf.Reverse().ToArray();
        return BitConverter.ToInt32(buf, 0);
    });
    Console.WriteLine($"Client received {i}");
    p.SendValue(++i, (val) =>
    {
        var result = BitConverter.GetBytes(val);
        if (reverse)
        {
            result = result.Reverse().ToArray();
        }

        return result;
    });
    Console.WriteLine($"Client sending {i}");
    c = p.ReadCmd();
}
p.Dispose();
