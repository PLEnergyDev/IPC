namespace SocketComm
{
    public enum Cmd
    {
        Unknown = -3,
        Error = -2,
        Stopped = -1,
        Exit = 0,
        Go = 1,
        Done = 2,
        Ready = 3,
        Ok=4,
        Receive=5
    }
}