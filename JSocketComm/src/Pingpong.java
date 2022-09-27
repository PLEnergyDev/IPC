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
            var i = (Object[])p.ReceiveValue((buf)-> SimpleConversion.BytesToArray(buf, ByteBuffer::getDouble));
            System.out.print("Client received: [");
            for(int j = 0; j < i.length; j++){
                System.out.print("[");
                var r = (Object[])i[j];
                for(int k = 0; k < r.length; k++){
                    System.out.print(r[k] + ",");
                }
                System.out.print("],");
            }
            System.out.println("]");

            c = p.ReadCmd();
        }
        p.close();

    }

}
