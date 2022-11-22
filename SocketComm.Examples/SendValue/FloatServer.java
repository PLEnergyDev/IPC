import java.nio.ByteBuffer;

public class FloatServer {
    public static void main(String[] args) throws Exception {
        FPipe p = new FPipe("/tmp/floatvalue.pipe");
        p.ExpectCmd(Cmd.Ready);
        float i = 1.1f;
        while (i < Float.MAX_VALUE && i > 1)
        {
            System.out.printf("Sending %f\n", i);
            p.SendValue(i, (val)-> ByteBuffer.allocate(4).putFloat(val));
            p.ExpectCmd(Cmd.Ready);
            p.WriteCmd(Cmd.Go);
            p.ExpectCmd(Cmd.Done);
            p.ExpectCmd(Cmd.Receive);
            i = p.ReceiveValue(ByteBuffer::getFloat);
            System.out.printf("Received %f\n", i);
        }
        p.WriteCmd(Cmd.Exit);
        p.close();
    }
}
