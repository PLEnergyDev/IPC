import java.nio.ByteBuffer;
import java.util.function.Function;

public class TwoDServer {
    public static void main(String[] args) throws Exception {
        FPipe p = new FPipe("/tmp/TwoDArray.pipe");
        p.ExpectCmd(Cmd.Ready);
        Integer[][] i = {{0,1},{2,3},{4,5},{6,7},{8,9}};
        System.out.print("Sending [");
        for (var e : i) {
            System.out.print("[");
            for(var f: e){
                System.out.print(f +", ");
            }
            System.out.print("], ");
        }
        System.out.println("]");
        p.SendValue(i, (val) -> SimpleConversion.ArrayToBytes(val, (Function<Integer, ByteBuffer>) (a) -> ByteBuffer.allocate(4).putInt(a)));
        p.ExpectCmd(Cmd.Ready);
        p.WriteCmd(Cmd.Go);
        p.ExpectCmd(Cmd.Done);
        p.ExpectCmd(Cmd.Receive);
        var res = p.ReceiveValue(ByteBuffer::getInt);
        System.out.printf("Received %d\n", res);
        p.WriteCmd(Cmd.Exit);
        p.close();
    }
}
