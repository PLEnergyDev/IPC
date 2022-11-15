using static SocketComm.Cmd;
namespace SocketComm.Examples.RunMethod;

public static class Client
{
    public static void Main()
    {
        FPipe p = new FPipe("/tmp/hello.pipe");
        p.Connect();
        p.WriteCmd(Ready);
        Cmd c;
        do
        {
            p.WriteCmd(Ready);
            p.ExpectCmd(Go);
            Console.WriteLine("Hello World!");
            p.WriteCmd(Done);
            c = p.ReadCmd();
        } while (c == Ready);
        
        p.Dispose();
    }
}