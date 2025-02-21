using System.IO.Compression;

namespace NativeSharpLz4;

public class NativeLz4Stream : Stream
{
    private readonly Stream stream;
    private readonly CompressionMode mode;
    private readonly bool leaveOpen;

    private IntPtr lz4Stream;

    public NativeLz4Stream(Stream stream, CompressionMode mode, NativeLz4Options options)
    {
        this.stream = stream;
        this.mode = mode;

        leaveOpen = options.LeaveOpen;

        lz4Stream = mode switch
        {
            CompressionMode.Compress => Lz4Native.LZ4_createStream(),
            CompressionMode.Decompress => Lz4Native.LZ4_createStreamDecode(),
            _ => throw new NotSupportedException()
        };

        if (options.Dictionary is not null)
        {
            Lz4Native.LZ4_setStreamDecode(lz4Stream, options.Dictionary, options.Dictionary.Length);
        }
    }

    public NativeLz4Stream(Stream stream, CompressionMode mode, bool leaveOpen = false)
        : this(stream, mode, new NativeLz4Options { LeaveOpen = leaveOpen }) { }

    public override bool CanRead => mode == CompressionMode.Decompress;

    public override bool CanSeek => false;

    public override bool CanWrite => mode == CompressionMode.Compress;

    public override long Length => throw new NotImplementedException();

    public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override void Flush()
    {
        stream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (mode != CompressionMode.Decompress)
            throw new NotSupportedException();

        // read from stream and decompress

        var compressedBuffer = new byte[count];
        var read = stream.Read(compressedBuffer, 0, count);

        if (read == 0)
            return 0;

        var decompressedBuffer = new byte[count * 4];

        var decompressedSize = Lz4Native.LZ4_decompress_safe_continue(lz4Stream, compressedBuffer, decompressedBuffer, read, decompressedBuffer.Length);

        if (decompressedSize < 0)
            throw new InvalidDataException();

        Array.Copy(decompressedBuffer, 0, buffer, offset, decompressedSize);

        return decompressedSize;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (mode != CompressionMode.Compress)
            throw new NotSupportedException();

        // compress and write to stream
        var compressedBuffer = new byte[count * 4];

        var compressedSize = Lz4Native.LZ4_compress_fast_continue(lz4Stream, buffer, compressedBuffer, count, compressedBuffer.Length, 1);

        if (compressedSize < 0)
            throw new InvalidDataException();

        stream.Write(compressedBuffer, 0, compressedSize);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (lz4Stream != IntPtr.Zero)
            {
                Lz4Native.LZ4_freeStreamDecode(lz4Stream);
                lz4Stream = IntPtr.Zero;
            }
        }
        base.Dispose(disposing);
    }
}
