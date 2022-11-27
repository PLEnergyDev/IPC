using static SocketComm.Cmd;

namespace SocketComm.Examples.SendArray;

public class OneDClient
{
    public static void Main()
    {
        FPipe p = new FPipe("/tmp/OneDArray.pipe");
        p.Connect();
        p.WriteCmd(Ready);
        Cmd c;
        c = p.ReadCmd();
        while (c == Receive)
        {
            int[] i =(int[]) p.ReceiveValue((val) => SimpleConversion.BytesToArray(val, SimpleConversion.BytesToNumber<int>));
            Console.Write("Received [");
            foreach (var e in i)
            {
                Console.Write(e + ", ");
            }
            Console.WriteLine("]");
            p.WriteCmd(Ready);
            p.ExpectCmd(Go);
            var res = Bench(i);
            p.WriteCmd(Done);
            Console.WriteLine($"Sending {res}");
            p.SendValue(res ,SimpleConversion.NumberToBytes);
            c = p.ReadCmd();
        }
        p.Dispose();
    }

    public static int Bench(int[] i)
    {
        var sum = 0;
        foreach (var e in i)
        {
            sum += e;
        }

        return sum;
    }
}