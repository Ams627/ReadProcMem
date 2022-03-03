using System;
using System.Runtime.InteropServices;

namespace ReadProcMem
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct UnicodeString
    {
        // The length in bytes of the string pointed to by buffer, not including the null-terminator.
        private ushort length;
        // The total allocated size in memory pointed to by buffer.
        private ushort maximumLength;
        // A pointer to the buffer containing the string data.
        private IntPtr buffer;

        public ushort Length { get { return length; } }
        public ushort MaximumLength { get { return maximumLength; } }
        public IntPtr Buffer { get { return buffer; } }
    }
}
