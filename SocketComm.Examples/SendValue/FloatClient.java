import java.nio.ByteBuffer;

public class FloatClient {
    public static void main(String[] args) throws Exception {
        FPipe p = new FPipe("/tmp/floatvalue.pipe");
        p.WriteCmd(Cmd.Ready);
        Cmd c;
        c = p.ReadCmd();
        while (c == Cmd.Receive)
        {
            float i = p.ReceiveValue(ByteBuffer::getFloat);
            System.out.printf("Received %f\n", i);
            p.WriteCmd(Cmd.Ready);
            p.ExpectCmd(Cmd.Go);
            i = Bench(i);
            p.WriteCmd(Cmd.Done);
            System.out.printf("Sending %f\n", i);
            p.SendValue(i, (val)-> ByteBuffer.allocate(4).putFloat(val));
            c = p.ReadCmd();
        }
        p.close();
    }

    public static float Bench(float i){
        return i * 2;
    }
}
