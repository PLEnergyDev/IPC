import java.io.IOException;
import java.net.StandardProtocolFamily;
import java.net.UnixDomainSocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.Iterator;
import java.util.concurrent.Callable;
import java.util.function.Function;

public class FPipe implements AutoCloseable, Iterable<Cmd> {
    final int MagicHandshakeValue = 25;
    SocketChannel channel;

    public FPipe(String file) throws IOException {

        UnixDomainSocketAddress address = UnixDomainSocketAddress.of(file);
        if (Files.exists(Path.of(file)))
        {
            channel = SocketChannel.open(StandardProtocolFamily.UNIX);
            channel.connect(address);
            System.out.println("handshake...");
            ShakeHands(channel);
            System.out.println("done");
        } else{
            ServerSocketChannel ss = ServerSocketChannel.open(StandardProtocolFamily.UNIX);
            ss.bind(address);
            channel = ss.accept();
            ss.close();
            System.out.println("handshake...");
            ReceiveHand(channel);
            System.out.println("done");

        }
    }

    public Cmd ReadCmd() throws IOException{
        ByteBuffer bb = ByteBuffer.allocate(1);
        channel.read(bb);
        bb.rewind();
        return Cmd.forValue(bb.get(0));
    }

    public void WriteCmd(Cmd cmd) throws IOException{
        ByteBuffer bb = ByteBuffer.allocate(1);
        channel.write(bb.put(cmd.getValue()).rewind());
    }

    public Cmd ExpectCmd(Cmd cmd) throws PipeCmdException, IOException {
        var read = ReadCmd();
        if(read == cmd){
            return read;
        }
        throw new PipeCmdException(String.format("Expected: %s - Received: %s", cmd, read));
    }

    public <T> void SendValue(T value, Function<T, ByteBuffer> converter) throws IOException, PipeCmdException {
        WriteCmd(Cmd.Receive);
        ExpectCmd(Cmd.Ready);
        var buf = converter.apply(value).rewind();
        ByteBuffer bb = ByteBuffer.allocate(4).putInt(buf.capacity()).rewind();
        channel.write(bb);
        channel.write(buf);
        ExpectCmd(Cmd.Ok);
    }

    public <T> T ReceiveValue(Function<ByteBuffer, T> converter) throws IOException {
        WriteCmd(Cmd.Ready);
        var bb = ByteBuffer.allocate(4);
        channel.read(bb);
        var length = bb.rewind().getInt();
        System.out.println("Receiving " + length + " bytes!");
        bb = ByteBuffer.allocate(length);
        var rec = channel.read(bb);
        if(rec != length){
            // TODO: throw error
        }
        WriteCmd(Cmd.Ok);
        return converter.apply(bb.rewind());
    }


    public void ShakeHands(SocketChannel channel) throws IOException
    {
        ByteBuffer bb = ByteBuffer.allocate(4);
        bb.putInt(MagicHandshakeValue);
        bb.rewind();
        channel.write(bb);
        bb.rewind();
        channel.read(bb);
        bb.rewind();
        int rec = bb.getInt();
        System.out.println("chk: received:"+rec+", expected: "+MagicHandshakeValue);
    }

    public void ReceiveHand(SocketChannel channel) throws IOException{
        ByteBuffer bb = ByteBuffer.allocate(4);
        channel.read(bb);
        int rec = bb.rewind().getInt();
        System.out.println("chk: received:"+rec+", expected: "+MagicHandshakeValue);
        bb.rewind().putInt(MagicHandshakeValue).rewind();
        channel.write(bb);
    }

    @Override
    public void close() throws Exception {
        if(channel!=null)
            channel.close();
    }

    @Override
    public Iterator<Cmd> iterator() {


        return new Iterator<Cmd>() {
            boolean reachedEnd = false;
            Cmd current = Cmd.Ok;
            @Override
            public boolean hasNext(){
                return current.getValue()>Cmd.Exit.getValue();
            }

            @Override
            public Cmd next() {
                try {

                    current = ReadCmd();
                } catch (IOException e){
                    current = Cmd.Error;
                }
                return current;
            }
        };
    }
}