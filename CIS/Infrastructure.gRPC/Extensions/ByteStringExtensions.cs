using Google.Protobuf;
using System.Runtime.InteropServices;

namespace CIS.Infrastructure.gRPC;
public static class ByteStringExtensions
{
    /// <summary>
    /// This copy method is memory efficient, there is no data copy (ToArray do copy of data).  
    /// Byte array instance should only be passed to methods which treat the array contents as read-only.
    /// </summary>
    public static byte[] ToArrayUnsafe(this ByteString bytesString)
    {
        return MemoryMarshal.TryGetArray(bytesString.Memory, out var arraySegment)
                       ? arraySegment.Array!
                       : throw new InvalidOperationException("Failed to get memory of document buffer");
    }
}
