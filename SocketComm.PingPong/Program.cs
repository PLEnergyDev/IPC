﻿using System.Text;
using SocketComm;
using static SocketComm.Cmd;

if (args.Length < 1)
{
    Console.Error.WriteLine("usage: [socket]");
    return;
}

FPipe p = new FPipe(args[0]);
p.Connect();
p.ExpectCmd(Ready);
Console.WriteLine("Starting loop");
int[] i = {1,2,3,4};
while(true)
{
    p.SendValue(i, (x)=> SimpleConversion.ArrayToBytes<int>(x,SimpleConversion.NumberToBytes));
    if (p.ReadCmd() == Receive)
    {
        i = (int[])p.ReceiveValue((x)=> SimpleConversion.BytesToArray(x,SimpleConversion.BytesToNumber<int>));
        Console.WriteLine(i);
    }
    else
    {
        break;
    }
}

p.WriteCmd(Exit);
p.Dispose();
