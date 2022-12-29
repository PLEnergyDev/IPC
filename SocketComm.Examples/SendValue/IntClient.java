import java.nio.ByteBuffer;
public class IntClient {
    public static void main(String[] args) throws Exception {
        FPipe p = new FPipe("/tmp/intvalue.pipe");
        p.WriteCmd(Cmd.Ready);
        Cmd c;
        c = p.ReadCmd();
        while (c == Cmd.Receive)
        {
            int i = p.ReceiveValue(ByteBuffer::getInt);
            System.out.printf("Received %d\n", i);
            p.WriteCmd(Cmd.Ready);
            p.ExpectCmd(Cmd.Go);
            i = Bench(i);
            p.WriteCmd(Cmd.Done);
            System.out.printf("Sending %d\n", i);
            p.SendValue(i, (val)-> ByteBuffer.allocate(4).putInt(val));
            c = p.ReadCmd();
        }
        p.close();
    }

    public static int Bench(int i){
        return i * 2;
    }
}
