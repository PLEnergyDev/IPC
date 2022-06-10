import java.io.IOException;

public class Pingpong {
    public static void main(String[] args) throws IOException, InterruptedException {
        System.out.println("Hello world");
        FPipe p = new FPipe("/tmp/jsock");
        while(true){
            p.WriteCmd(Cmd.Go);
            Cmd c = p.ReadCmd();
            System.out.println("received: "+c);
            Thread.sleep(2000);
        }
//        for(Cmd c : p){
//            System.out.println(c.toString());
//            p.WriteCmd(Cmd.Go);
//            Thread.sleep(2000);
//        }
    }
}
