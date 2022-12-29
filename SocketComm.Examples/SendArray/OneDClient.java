import java.io.Console;
import java.nio.ByteBuffer;

public class OneDClient {
    public static void main(String[] args) throws Exception {
        FPipe p = new FPipe("/tmp/OneDArray.pipe");
        p.WriteCmd(Cmd.Ready);
        Cmd c;
        c = p.ReadCmd();
        while (c == Cmd.Receive)
        {
            var i = (Integer[])p.ReceiveValue((buf)-> SimpleConversion.BytesToArray(buf, ByteBuffer::getInt));
            System.out.print("Received [");
            for (var e : i) {
                System.out.print(e +", ");
            }
            System.out.println("]");
            p.WriteCmd(Cmd.Ready);
            p.ExpectCmd(Cmd.Go);
            var res = Bench(i);
            p.WriteCmd(Cmd.Done);
            System.out.printf("Sending %d\n", res);
            p.SendValue(res, (val)-> ByteBuffer.allocate(4).putInt(val));
            c = p.ReadCmd();
        }
        p.close();
    }

    public static int Bench(Integer[] i){

        var sum = 0;
        for (var e : i){
            sum += e;
        }
        return sum;
    }
}
