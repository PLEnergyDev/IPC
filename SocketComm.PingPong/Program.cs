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

int i = short.MinValue;
var v = new double[,] {{1.1,2.2},{3.3,4.4},{5.5,6.6},{7.7,8.8},{9.9,0.987654321}};
while(i < short.MaxValue)
{
    p.SendValue(v, (val)=> SimpleConversion.ArrayToBytes<double>(val, SimpleConversion.NumberToBytes));
    i = short.MaxValue;
    break;
    if (p.ReadCmd() == Receive)
    {
        i = p.ReceiveValue(SimpleConversion.BytesToNumber<int>);
    }
    else
    {
        break;
    }
}

p.WriteCmd(Exit);
p.Dispose();
