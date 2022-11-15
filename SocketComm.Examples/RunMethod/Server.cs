using static SocketComm.Cmd;

namespace SocketComm.Examples.RunMethod;

static class Server
{
    public static void Main()
    {
        FPipe p = new FPipe("/tmp/hello.pipe");
        p.Connect();
        try
        {
            p.ExpectCmd(Ready);
            while (true)
            {
                p.ExpectCmd(Ready);
                p.WriteCmd(Go);
                p.ExpectCmd(Done);
                break;
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