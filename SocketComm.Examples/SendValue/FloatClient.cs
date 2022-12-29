using static SocketComm.Cmd;
namespace SocketComm.Examples.SendValue;

public class FloatClient
{
    public static void Main()
    {
        FPipe p = new FPipe("/tmp/floatvalue.pipe");
        p.Connect();
        p.WriteCmd(Ready);
        Cmd c;
        c = p.ReadCmd();
        while (c == Receive)
        {
            float i = p.ReceiveValue(SimpleConversion.BytesToNumber<float>);
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

    public static float Bench(float i)
    {
        return i * 2;
    }
}