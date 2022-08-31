using System.Text;
using SocketComm;

if (args.Length < 1)
{
    Console.Error.WriteLine("usage: [socket]");
    return;
}
FPipe p = new FPipe(args[0]);

p.Connect();
p.WriteCmd(Cmd.Ready);
Cmd c = p.ReadCmd();
while (c == Cmd.Receive)
{
    var i = p.ReceiveValue((buf)=> SimpleConversion.BytesToArray(buf, SimpleConversion.BytesToNumber<int>));
    Console.Write($"Received [");
    foreach (var n in i)
    {
        Console.Write($"{n}, ");
    }
    Console.WriteLine("]");

    c = p.ReadCmd();
}
p.Dispose();
