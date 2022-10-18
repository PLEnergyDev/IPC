using System.Text;
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
Console.WriteLine("Starting loop");
double i = 0.0 ;
while(i < 10.0)
{
    p.SendValue(i + 1, SimpleConversion.NumberToBytes);
    if (p.ReadCmd() == Receive)
    {
        i = p.ReceiveValue(SimpleConversion.BytesToNumber<double>);
    }
    else
    {
        break;
    }
}

p.WriteCmd(Exit);
p.Dispose();
