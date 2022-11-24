using static SocketComm.Cmd;

namespace SocketComm.Examples.SendArray;

public class TwoDClient
{
    public static void Main()
    {
        FPipe p = new FPipe("/tmp/TwoDArray.pipe");
        p.Connect();
        p.WriteCmd(Ready);
        Cmd c;
        c = p.ReadCmd();
        while (c == Receive)
        {
            int[,] i = (int[,]) p.ReceiveValue((val) =>
                SimpleConversion.BytesToArray(val, SimpleConversion.BytesToNumber<int>));
            Console.Write("Received [");
            for (int a = 0; a < i.GetLength(0); a++)
            {
                Console.Write("[");
                for (int b = 0; b < i.GetLength(1); b++)
                {
                    Console.Write(i[a,b] + ", ");
                }
                Console.Write("], ");
            }
            Console.WriteLine("]");
            p.WriteCmd(Ready);
            p.ExpectCmd(Go);
            i = Bench(i);
            p.WriteCmd(Done);
            Console.Write("Sending [");
            for (int a = 0; a < i.GetLength(0); a++)
            {
                Console.Write("[");
                for (int b = 0; b < i.GetLength(1); b++)
                {
                    Console.Write(i[a,b] + ", ");
                }
                Console.Write("], ");
            }
            Console.WriteLine("]");
            p.SendValue(i, (val) => SimpleConversion.ArrayToBytes<int>(val, SimpleConversion.NumberToBytes));
            c = p.ReadCmd();
        }

        p.Dispose();
    }

    public static int[,] Bench(int[,] i)
    {
        var sum = 0;
        foreach (var e in i)
        {
            sum += e;
        }

        for (int a = 0; a < i.GetLength(0); a++)
        {
            for (int b = 0; b < i.GetLength(1); b++)
            {
                i[a, b] += sum;
            }
        }

        return i;
    }
}