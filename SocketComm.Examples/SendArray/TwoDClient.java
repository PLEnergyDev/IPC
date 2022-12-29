import java.nio.ByteBuffer;

public class TwoDClient {
    public static void main(String[] args) throws Exception {
        FPipe p = new FPipe("/tmp/TwoDArray.pipe");
        p.WriteCmd(Cmd.Ready);
        Cmd c;
        c = p.ReadCmd();
        while (c == Cmd.Receive)
        {
            var i = (Integer[][])p.ReceiveValue((buf)-> SimpleConversion.BytesToArray(buf, ByteBuffer::getInt));
            System.out.print("Received [");
            for (var e : i) {
                System.out.print("[");
                for(var f: e){
                    System.out.print(f +", ");
                }
                System.out.print("], ");
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

    public static int Bench(Integer[][] i){

        var sum = 0;
        for (var e : i){
            for(var f: e){
                sum += f;
            }
        }
        return sum;
    }
}
