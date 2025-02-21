using System.Runtime.InteropServices;

namespace NativeSharpLz4;

internal sealed partial class Lz4Native
{
    private const string Library = "liblz4";

#if NET8_0_OR_GREATER
    [LibraryImport(Library)]
    public static partial IntPtr LZ4_createStreamDecode();
#else
    [DllImport(Library)]
    public static extern IntPtr LZ4_createStreamDecode();
#endif

#if NET8_0_OR_GREATER
    [LibraryImport(Library)]
    public static partial int LZ4_freeStreamDecode(IntPtr stream);
#else
    [DllImport(Library)]
    public static extern int LZ4_freeStreamDecode(IntPtr stream);
#endif

#if NET8_0_OR_GREATER
    [LibraryImport(Library)]
    public static partial int LZ4_setStreamDecode(IntPtr stream, byte[] dictionary, int dictSize);
#else
    [DllImport(Library)]
    public static extern int LZ4_setStreamDecode(IntPtr stream, byte[] dictionary, int dictSize);
#endif

#if NET8_0_OR_GREATER
    [LibraryImport(Library)]
    public static partial int LZ4_decompress_safe_continue(IntPtr stream, byte[] src, byte[] dst, int compressedSize, int maxDecompressedSize);
#else
    [DllImport(Library)]
    public static extern int LZ4_decompress_safe_continue(IntPtr stream, byte[] src, byte[] dst, int compressedSize, int maxDecompressedSize);
#endif

#if NET8_0_OR_GREATER
    [LibraryImport(Library)]
    public static partial IntPtr LZ4_createStream();
#else
    [DllImport(Library)]
    public static extern IntPtr LZ4_createStream();
#endif

#if NET8_0_OR_GREATER
    [LibraryImport(Library)]
    public static partial int LZ4_compress_fast_continue(IntPtr stream, byte[] src, byte[] dst, int srcSize, int dstCapacity, int acceleration);
#else
    [DllImport(Library)]
    public static extern int LZ4_compress_fast_continue(IntPtr stream, byte[] src, byte[] dst, int srcSize, int dstCapacity, int acceleration);
#endif
}
