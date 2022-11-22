public class Server {
    public static void main(String[] args) throws Exception {
        FPipe p = new FPipe("/tmp/hello.pipe");
        p.ExpectCmd(Cmd.Ready);
        while(true)
        {
            p.ExpectCmd(Cmd.Ready);
            p.WriteCmd(Cmd.Go);
            p.ExpectCmd(Cmd.Done);
            break;
        }

        p.WriteCmd(Cmd.Exit);
        p.close();
    }
}
