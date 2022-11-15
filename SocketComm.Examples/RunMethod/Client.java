import java.nio.ByteBuffer;

public class Client {
    public static void main(String[] args) throws Exception {
        FPipe p = new FPipe("/tmp/hello.pipe");
        p.WriteCmd(Cmd.Ready);
        Cmd c;
        do
        {
            p.WriteCmd(Cmd.Ready);
            p.ExpectCmd(Cmd.Go);
            System.out.println("Hello World!");
            p.WriteCmd(Cmd.Done);
            c = p.ReadCmd();
        } while (c == Cmd.Ready);

        p.close();
    }
}