using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ReadProcMem
{
    class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING : IDisposable
        {
            public ushort Length;
            public ushort MaximumLength;
            private IntPtr buffer;

            public UNICODE_STRING(string s)
            {
                Length = (ushort)(s.Length * 2);
                MaximumLength = (ushort)(Length + 2);
                buffer = Marshal.StringToHGlobalUni(s);
            }

            public void Dispose()
            {
                Marshal.FreeHGlobal(buffer);
                buffer = IntPtr.Zero;
            }

            public override string ToString()
            {
                return Marshal.PtrToStringUni(buffer);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CURDIR
        {
            public UNICODE_STRING DosPath;
            public IntPtr Handle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RTL_USER_PROCESS_PARAMETERS
        {
            public uint MaxLen;
            public uint Len;
            public uint Flags;
            public uint DebugFlags;
            public IntPtr ConsoleHandle;
            public uint ConsoleFlags;
            public IntPtr StandardInput;
            public IntPtr StandardOutput;
            public IntPtr StandardError;
            public CURDIR CurrentDirectory;
            public UNICODE_STRING DllPath;
            public UNICODE_STRING ImagePathName;
            public UNICODE_STRING CommandLine;
            public IntPtr Environment;
            public UInt32 StartingX;
            public UInt32 StartingY;
            public UInt32 CountX;
            public UInt32 CountY;
            public UInt32 CountCharsX;
            public UInt32 CountCharsY;
            public UInt32 FillAttribute;
            public UInt32 WindowFlags;
            public UInt32 ShowWindowFlags;
            public UNICODE_STRING WindowTitle;
            public UNICODE_STRING DesktopInfo;
            public UNICODE_STRING ShellInfo;
            public UNICODE_STRING RuntimeData;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public RTL_DRIVE_LETTER_CURDIR[] CurrentDirectories;
            public UInt32 EnvironmentSize;
            public UInt32 EnvironmentVersion;
            public IntPtr PackageDependencyData;
            public UInt32 ProcessGroupId;
            public UInt32 LoaderThreads;
        }

        public struct RTL_DRIVE_LETTER_CURDIR
        {
            short Flags;
            short Length;
            Int32 TimeStamp;
            UNICODE_STRING DosPath;
        }

        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public AllocationProtectEnum AllocationProtect;
            public IntPtr RegionSize;
            public StateEnum State;
            public AllocationProtectEnum Protect;
            public TypeEnum Type;
        }

        public enum AllocationProtectEnum : uint
        {
            PAGE_EXECUTE = 0x00000010,
            PAGE_EXECUTE_READ = 0x00000020,
            PAGE_EXECUTE_READWRITE = 0x00000040,
            PAGE_EXECUTE_WRITECOPY = 0x00000080,
            PAGE_NOACCESS = 0x00000001,
            PAGE_READONLY = 0x00000002,
            PAGE_READWRITE = 0x00000004,
            PAGE_WRITECOPY = 0x00000008,
            PAGE_GUARD = 0x00000100,
            PAGE_NOCACHE = 0x00000200,
            PAGE_WRITECOMBINE = 0x00000400
        }

        public enum StateEnum : uint
        {
            MEM_COMMIT = 0x1000,
            MEM_FREE = 0x10000,
            MEM_RESERVE = 0x2000
        }

        public enum TypeEnum : uint
        {
            MEM_IMAGE = 0x1000000,
            MEM_MAPPED = 0x40000,
            MEM_PRIVATE = 0x20000
        }

        public enum ProcessInfoClass
        {
            ProcessBasicInformation = 0,
            ProcessDebugPort = 7,
            ProcessWow64Information = 26,
            ProcessImageFileName = 27,
            ProcessBreakOnTermination = 29,
            ProcessSubsystemInformation = 75
        }

        [Flags]
        public enum ProcessPermissions
        {
            PROCESS_CREATE_PROCESS = 0x0080,
            PROCESS_CREATE_THREAD = 0x0002,
            PROCESS_DUP_HANDLE = 0x0040,
            PROCESS_QUERY_INFORMATION = 0x0400,
            PROCESS_QUERY_LIMITED_INFORMATION = 0x1000,
            PROCESS_SET_INFORMATION = 0x0200,
            PROCESS_SET_QUOTA = 0x0100,
            PROCESS_SUSPEND_RESUME = 0x0800,
            PROCESS_TERMINATE = 0x0001,
            PROCESS_VM_OPERATION = 0x0008,
            PROCESS_VM_READ = 0x0010,
            PROCESS_VM_WRITE = 0x0020,
            SYNCHRONIZE = 0x100000,
            PROCESS_ALL_ACCESS = 0x000F0000 | SYNCHRONIZE | 0xFFFF,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_BASIC_INFORMATION
        {
            public IntPtr ExitStatus;
            public IntPtr PebBaseAddress;
            public IntPtr AffinityMask;
            public IntPtr BasePriority;
            public IntPtr UniqueProcessId;
            public IntPtr InheritedFromUniqueProcessId;
        }


        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref PROCESS_BASIC_INFORMATION processInformation, uint processInformationLength, IntPtr returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
             ProcessPermissions processAccess,
             bool bInheritHandle,
             int processId
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref IntPtr lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref RTL_USER_PROCESS_PARAMETERS lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);
    }
    [StructLayout(LayoutKind.Explicit, Size = 0x20C)]
    public struct C
    {
        [FieldOffset(0x000)]
        public int i;
        [FieldOffset(0x004)]
        public IntPtr p;
    }
    unsafe class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var handle = Process.GetCurrentProcess().Handle;

                if (!Environment.Is64BitProcess)
                {
                    Console.Error.WriteLine($"This is not a 64 bit process so NtQueryInformationProcess cannot work directly.");
                }

                // first of all get the address of the PEB (Process Environment block) by calling NtQueryInformationProcess
                IntPtr hProc = NativeMethods.OpenProcess(NativeMethods.ProcessPermissions.PROCESS_QUERY_INFORMATION | NativeMethods.ProcessPermissions.PROCESS_VM_READ, false, Process.GetCurrentProcess().Id);
                var pbi = new NativeMethods.PROCESS_BASIC_INFORMATION();
                var status = NativeMethods.NtQueryInformationProcess(
                    hProc,
                    (int)NativeMethods.ProcessInfoClass.ProcessBasicInformation,
                    ref pbi,
                    (uint)Marshal.SizeOf(pbi),
                    IntPtr.Zero);
                var error = Marshal.GetLastWin32Error();

                var pebAddress = pbi.PebBaseAddress.ToInt64();

                long processParametersOffset = 0x20;
                IntPtr processParameters = new IntPtr();
                
                bool ret = NativeMethods.ReadProcessMemory(hProc, new IntPtr(pebAddress + processParametersOffset), ref processParameters, Marshal.SizeOf(processParameters), IntPtr.Zero);
                var error2 = Marshal.GetLastWin32Error();


                var rtl = new NativeMethods.RTL_USER_PROCESS_PARAMETERS();
                bool ret3 = NativeMethods.ReadProcessMemory(hProc, processParameters, ref rtl, Marshal.SizeOf(rtl), IntPtr.Zero);
                byte[] envBuffer = new byte[rtl.EnvironmentSize];
                fixed (byte* envBufferPtr = envBuffer)
                {
                    bool ret5 = NativeMethods.ReadProcessMemory(hProc, rtl.Environment, (IntPtr)envBufferPtr, envBuffer.Length, IntPtr.Zero);
                }

                char[] chars = Encoding.Unicode.GetChars(envBuffer);
                var list = new List<string>();
                var start = 0;
                bool hadOnlyNulls = true;
                for (int i = 0; i < chars.Length; i++)
                {
                    var c = chars[i];
                    if (c != 0)
                    {
                        hadOnlyNulls = false;
                    }
                    if (chars[i] == 0 || i == chars.Length - 1)
                    {
                        if (!hadOnlyNulls)
                        {
                            list.Add(new string(chars, start, i - start));
                        }
                        start = i + 1;
                        hadOnlyNulls = true;
                    }
                }

            }
            catch (Exception ex)
            {
                var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
                var progname = Path.GetFileNameWithoutExtension(fullname);
                Console.Error.WriteLine($"{progname} Error: {ex.Message}");
            }

        }
    }
}
