using static SocketComm.Cmd;

namespace SocketComm.Examples.SendValue;

public class IntClient
{
    public static void Main()
    {
        FPipe p = new FPipe("/tmp/hello.pipe");
        p.Connect();
        p.WriteCmd(Ready);
        Cmd c;
        c = p.ReadCmd();
        while (c == Receive)
        {
            int i = p.ReceiveValue(SimpleConversion.BytesToNumber<int>);
            Console.WriteLine($"Received {i}");
            p.WriteCmd(Ready);
            p.ExpectCmd(Go);
            i = Bench(i);
            p.WriteCmd(Done);
            Console.WriteLine($"Sending {i}");
            p.SendValue(i, SimpleConversion.NumberToBytes);
            c = p.ReadCmd();
        }
        p.Dispose();
    }

    public static int Bench(int i)
    {
        return i * 2;
    }
}