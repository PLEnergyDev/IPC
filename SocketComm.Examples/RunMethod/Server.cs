using SocketComm;
using static SocketComm.Cmd;

static class Server
{
    public static void Main()
    {
        FPipe p = new FPipe("/tmp/hello.pipe");
        p.Connect();
        p.ExpectCmd(Ready);
        while(true)
        {
            p.ExpectCmd(Ready);
            p.WriteCmd(Go);
            p.ExpectCmd(Done);
            break;
        }
        p.WriteCmd(Exit);
        p.Dispose();
    }
}



