using static SocketComm.Cmd;
namespace SocketComm.Examples.SendValue;

public class FloatServer
{
    public static void Main()
    {
        FPipe p = new FPipe("/tmp/floatvalue.pipe");
        p.Connect();
        try
        {
            p.ExpectCmd(Ready);
            float i = 1.1f;
            while (i is < float.MaxValue and > 1)
            {
                Console.WriteLine($"Sending {i}");
                p.SendValue(i, SimpleConversion.NumberToBytes);
                p.ExpectCmd(Ready);
                p.WriteCmd(Go);
                p.ExpectCmd(Done);
                p.ExpectCmd(Receive);
                i = p.ReceiveValue(SimpleConversion.BytesToNumber<float>);
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