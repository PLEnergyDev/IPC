using static SocketComm.Cmd;
namespace SocketComm.Examples.SendValue;

public class IntServer
{
    public static void Main()
    {
        FPipe p = new FPipe("/tmp/intvalue.pipe");
        p.Connect();
        try
        {
            p.ExpectCmd(Ready);
            int i = 2;
            while (i is < Int32.MaxValue and > 1)
            {
                Console.WriteLine($"Sending {i}");
                p.SendValue(i, SimpleConversion.NumberToBytes);
                p.ExpectCmd(Ready);
                p.WriteCmd(Go);
                p.ExpectCmd(Done);
                p.ExpectCmd(Receive);
                i = p.ReceiveValue(SimpleConversion.BytesToNumber<int>);
                Console.WriteLine($"Received {i}");
            }
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