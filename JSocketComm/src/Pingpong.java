import java.io.IOException;
import java.nio.ByteBuffer;

public class Pingpong {
    public static void main(String[] args) throws Exception {
        if(args.length < 1){
            System.err.println("usage: [socket]");
            System.exit(1);
        }
        FPipe p = new FPipe(args[0]);
        p.WriteCmd(Cmd.Ready);
        Cmd c = p.ReadCmd();
        while(c == Cmd.Receive){
            var i = p.ReceiveValue(ByteBuffer::getInt);
            System.out.println("Client received: " + i);

            p.SendValue(++i,(val)-> ByteBuffer.allocate(4).putInt(val));
            System.out.println("Client sent: " + i);
            c = p.ReadCmd();
        }
        p.close();

    }

}
