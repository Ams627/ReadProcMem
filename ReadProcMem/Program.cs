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
    class Program
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



                var processes = Process.GetProcesses();
                var dprocs = processes.Where(x=>x.ProcessName.IndexOf("devenv", StringComparison.OrdinalIgnoreCase) != -1);
                var r1 = IsProcessWow64(dprocs.First());

                var procDict = processes.ToDictionary(x=>x.Id, x=>x.ProcessName);
                var processes64 = processes.Where(x=>!IsProcessWow64(x));
                var ids32 = processes.Where(IsProcessWow64).Select(x => x.Id).ToHashSet();
                var ids64 = processes64.Select(x => x.Id).ToHashSet();
                var fail = false;

                var pids = new List<int>();
                foreach (var arg in args)
                {
                    if (arg.All(char.IsDigit))
                    {
                        var procId = Int32.Parse(arg);
                        if (ids32.Contains(procId))
                        {
                            Console.Error.WriteLine($"error: {procId} is a 32 bit process");
                        }
                        else if (!ids64.Contains(procId))
                        {
                            fail = true;
                            Console.Error.WriteLine($"error: {procId} does not correspond to a running process Id.");
                        }
                        else
                        {
                            pids.Add(procId);
                        }
                    }
                    else
                    {
                        var morePids = processes.Where(x=>x.ProcessName.Contains(arg)).Select(x=>x.Id);
                        pids.AddRange(morePids);
                    }
                }

                if (fail)
                {
                    Console.Error.WriteLine($"Exiting. Please correct the errors and try again");
                    Environment.Exit(1);
                }


                foreach (var pid in pids)
                {
                    Console.WriteLine($"pid: {pid} name: {procDict[pid]}");
                    var env = GetProcessEnvironment(pid);
                    foreach (var variable in env)
                    {
                        Console.WriteLine($"    {variable}");
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

        private unsafe static IEnumerable<string> GetProcessEnvironment(int pid)
        {
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

            Span<byte> ebuf = new byte[100];
            // stackalloc byte[(int)rtl.EnvironmentSize];


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
            return list;
        }

        private static bool IsProcessWow64(Process p)
        {
            var handle = NativeMethods.OpenProcess(NativeMethods.ProcessPermissions.PROCESS_QUERY_INFORMATION, false, p.Id);
            NativeMethods.IsWow64Process(handle, out var result);
            return result;
        }
    }
}
