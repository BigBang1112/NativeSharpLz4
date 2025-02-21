namespace NativeSharpLz4;

public sealed class NativeLz4Options
{
    public byte[]? Dictionary { get; set; }
    public bool LeaveOpen { get; set; }
}