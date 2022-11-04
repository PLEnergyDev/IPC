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
int[] i = {1,2,3,4};
while(true)
{
    p.SendValue(i, (x)=> SimpleConversion.ArrayToBytes<int>(x,SimpleConversion.NumberToBytes));
    if (p.ReadCmd() == Receive)
    {
        i = (int[])p.ReceiveValue((x)=> SimpleConversion.BytesToArray(x,SimpleConversion.BytesToNumber<int>));
        foreach (var e in i)
        {
            Console.WriteLine(e);
        }
        
    }
    else
    {
        break;
    }
}

p.WriteCmd(Exit);
p.Dispose();
