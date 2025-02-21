namespace NativeSharpLz4;

public class Lz4Exception : Exception
{
    public Lz4Exception() { }
    public Lz4Exception(string message) : base("[LZ4] " + message) { }
    public Lz4Exception(string message, Exception innerException) : base(message, innerException) { }
}