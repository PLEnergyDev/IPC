import java.nio.ByteBuffer;

public class IntServer {
    public static void main(String[] args) throws Exception {
        FPipe p = new FPipe("/tmp/intvalue.pipe");
        p.ExpectCmd(Cmd.Ready);
        int i = 2;
        while (i < Integer.MAX_VALUE && i > 1)
        {
            System.out.printf("Sending %d\n", i);
            p.SendValue(i, (val) -> ByteBuffer.allocate(4).putInt(val));
            p.ExpectCmd(Cmd.Ready);
            p.WriteCmd(Cmd.Go);
            p.ExpectCmd(Cmd.Done);
            p.ExpectCmd(Cmd.Receive);
            i = p.ReceiveValue(ByteBuffer::getInt);
            System.out.printf("Received %d\n", i);
        }
        p.WriteCmd(Cmd.Exit);
        p.close();
    }
}
