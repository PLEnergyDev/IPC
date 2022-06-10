using SocketComm;
using static SocketComm.Cmd;

namespace SocketComm.PingPong
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }
            FPipe p = new FPipe(args[0]);
            await foreach (var cmd in p)
            {
                Console.WriteLine($"Received: {cmd}");
                Console.Write($"Replying [{cmd}]..");
                await p.WriteCmdAsync(cmd);
                Console.WriteLine(".OK");
            }
        }
    }
}