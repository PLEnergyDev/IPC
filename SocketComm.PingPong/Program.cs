using System.ComponentModel;
using SocketComm;
using static SocketComm.Cmd;

namespace SocketComm.PingPong
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }
            FPipe p = new FPipe(args[0]);
            p.Connect();
            int i = -30;
            while (i < 100)
            {
                p.SendValue(i, BitConverter.GetBytes);
                i = p.ReceiveValue((buf)=>BitConverter.ToInt32(buf,0));
            }

            p.WriteCmd(Exit);
            p.Dispose();

        }
    }
}