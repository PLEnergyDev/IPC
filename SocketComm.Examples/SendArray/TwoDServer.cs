using static SocketComm.Cmd;
namespace SocketComm.Examples.SendArray;

public class TwoDServer
{
    public static void Main()
    {
        FPipe p = new FPipe("/tmp/TwoDArray.pipe");
        p.Connect();
        try
        {
            p.ExpectCmd(Ready);
            int[,] i = {{0,1},{2,3},{4,5},{6,7},{8,9}};
            Console.Write("Sending [");
            for (int a = 0; a < i.GetLength(0); a++)
            {
                Console.Write("[");
                for (int b = 0; b < i.GetLength(1); b++)
                {
                    Console.Write(i[a, b] + ", ");
                }

                Console.Write("], ");
            }

            Console.WriteLine("]");
            p.SendValue(i, (val) => SimpleConversion.ArrayToBytes<int>(val, SimpleConversion.NumberToBytes));
            p.ExpectCmd(Ready);
            p.WriteCmd(Go);
            p.ExpectCmd(Done);
            p.ExpectCmd(Receive);
            var res = p.ReceiveValue(SimpleConversion.BytesToNumber<int>);
            Console.WriteLine($"Received {res}");
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("An error occured with the pipe: " + e);
        }
        finally
        {
            p.WriteCmd(Exit);
            p.Dispose();
        }
        
    }
}