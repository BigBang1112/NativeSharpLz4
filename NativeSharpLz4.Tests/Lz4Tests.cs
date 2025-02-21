using System.IO.Compression;
using System.Text;

namespace NativeSharpLz4.Tests;

public class Lz4Tests
{
    [Fact]
    public void Test()
    {
        using var stream = new MemoryStream();
        var data = Encoding.UTF8.GetBytes("Hello, World!");
        using (var lz4Stream = new NativeLz4Stream(stream, CompressionMode.Compress, leaveOpen: true))
        {
            lz4Stream.Write(data, 0, data.Length);
        }
        stream.Position = 0;
        using var lz4Stream2 = new NativeLz4Stream(stream, CompressionMode.Decompress);
        var buffer = new byte[256];
        var read = lz4Stream2.Read(buffer, 0, buffer.Length);
        Assert.Equal(data, buffer.Take(read));
    }

    [Fact]
    public async Task TestAsync()
    {
        using var stream = new MemoryStream();
        var data = Encoding.UTF8.GetBytes("Hello, World!");
        using (var lz4Stream = new NativeLz4Stream(stream, CompressionMode.Compress, leaveOpen: true))
        {
            await lz4Stream.WriteAsync(data, 0, data.Length);
        }
        stream.Position = 0;
        using var lz4Stream2 = new NativeLz4Stream(stream, CompressionMode.Decompress);
        var buffer = new byte[256];
        var read = await lz4Stream2.ReadAsync(buffer, 0, buffer.Length);
        Assert.Equal(data, buffer.Take(read));
    }

#if NET6_0_OR_GREATER
    [Fact]
    public async Task TestModernAsync()
    {
        await using var stream = new MemoryStream();
        var data = Encoding.UTF8.GetBytes("Hello, World!");
        await using (var lz4Stream = new NativeLz4Stream(stream, CompressionMode.Compress, leaveOpen: true))
        {
            await lz4Stream.WriteAsync(data);
        }
        stream.Position = 0;
        await using var lz4Stream2 = new NativeLz4Stream(stream, CompressionMode.Decompress);
        var buffer = new byte[256];
        var read = await lz4Stream2.ReadAsync(buffer);
        Assert.Equal(data, buffer.Take(read));
    }
#endif

    [Fact]
    public void TestFlush()
    {
        using var stream = new MemoryStream();
        var data = Encoding.UTF8.GetBytes("Hello, World!");
        using var lz4Stream = new NativeLz4Stream(stream, CompressionMode.Compress, leaveOpen: true);
        lz4Stream.Write(data, 0, data.Length);
        lz4Stream.Flush();
        stream.Position = 0;
        using var lz4Stream2 = new NativeLz4Stream(stream, CompressionMode.Decompress, leaveOpen: true);
        var buffer = new byte[256];
        var read = lz4Stream2.Read(buffer, 0, buffer.Length);
        Assert.Equal(data, buffer.Take(read));
    }

    [Fact]
    public async Task TestFlushAsync()
    {
        using var stream = new MemoryStream();
        var data = Encoding.UTF8.GetBytes("Hello, World!");
        using var lz4Stream = new NativeLz4Stream(stream, CompressionMode.Compress, leaveOpen: true);
        await lz4Stream.WriteAsync(data, 0, data.Length);
        await lz4Stream.FlushAsync();
        stream.Position = 0;
        using var lz4Stream2 = new NativeLz4Stream(stream, CompressionMode.Decompress, leaveOpen: true);
        var buffer = new byte[256];
        var read = await lz4Stream2.ReadAsync(buffer, 0, buffer.Length);
        Assert.Equal(data, buffer.Take(read));
    }

#if NET6_0_OR_GREATER
    [Fact]
    public async Task TestModernFlushAsync()
    {
        await using var stream = new MemoryStream();
        var data = Encoding.UTF8.GetBytes("Hello, World!");
        await using var lz4Stream = new NativeLz4Stream(stream, CompressionMode.Compress, leaveOpen: true);
        await lz4Stream.WriteAsync(data);
        await lz4Stream.FlushAsync();
        stream.Position = 0;
        await using var lz4Stream2 = new NativeLz4Stream(stream, CompressionMode.Decompress, leaveOpen: true);
        var buffer = new byte[256];
        var read = await lz4Stream2.ReadAsync(buffer);
        Assert.Equal(data, buffer.Take(read));
    }
#endif
}