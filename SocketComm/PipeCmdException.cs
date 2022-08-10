using System.Runtime.Serialization;

namespace SocketComm
{
    class PipeCmdException : Exception
    {
        public PipeCmdException()
        {
        }

        public PipeCmdException(string? message) : base(message)
        {
        }

        public PipeCmdException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PipeCmdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}