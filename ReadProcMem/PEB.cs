using System;
using System.Runtime.InteropServices;

namespace ReadProcMem
{

    [StructLayout(LayoutKind.Explicit, Size = 0x20C)]
    public struct PEB
    {
        [FieldOffset(0x000)]
        public byte InheritedAddressSpace;
        [FieldOffset(0x001)]
        public byte ReadImageFileExecOptions;
        [FieldOffset(0x002)]
        public byte BeingDebugged;
        [FieldOffset(0x003)]
        public byte Spare;
        [FieldOffset(0x004)]
        public IntPtr Mutant;
#if WIN64
        [FieldOffset(0x00C)]
        public IntPtr ImageBaseAddress; // (PVOID) 
        [FieldOffset(0x014)]
        public IntPtr LoaderData; // (PPEB_LDR_DATA)
        [FieldOffset(0x01C)]
        public IntPtr ProcessParameters; // (PRTL_USER_PROCESS_PARAMETERS)
        [FieldOffset(0x024)]
        public IntPtr SubSystemData;  // (PVOID) 
        [FieldOffset(0x02c)]
        public IntPtr ProcessHeap;  // (PVOID) 
        [FieldOffset(0x034)]
        public IntPtr FastPebLock; // (PRTL_CRITICAL_SECTION)  
        [FieldOffset(0x03c)]
        public IntPtr FastPebLockRoutine; // (PPEBLOCKROUTINE)
        [FieldOffset(0x044)]
        public IntPtr FastPebUnlockRoutine;  // (PPEBLOCKROUTINE)
        [FieldOffset(0x04c)]
        public UInt32 EnvironmentUpdateCount; // (ULONG)
        [FieldOffset(0x050)]
        public IntPtr KernelCallbackTable;  // (PPVOID)  
        [FieldOffset(0x058)]
        public IntPtr SystemReserved;  // (PVOID)  
        [FieldOffset(0x060)]
        public IntPtr AtlThunkSListPtr32;  // (PVOID)  
        [FieldOffset(0x068)]
        public IntPtr FreeList;  // (PPEB_FREE_BLOCK) 
        [FieldOffset(0x070)]
        public UInt32 TlsExpansionCounter;  // (ULONG) 
        [FieldOffset(0x074)]
        public IntPtr TlsBitmap;  // (PVOID)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        [FieldOffset(0x07c)]
        public UInt32[] TlsBitmapBits;  // (ULONG) 
        [FieldOffset(0x084)]
        public IntPtr ReadOnlySharedMemoryBase;  // (PVOID) 
        [FieldOffset(0x08c)]
        public IntPtr ReadOnlySharedMemoryHeap;  // (PVOID) 
        [FieldOffset(0x094)]
        public IntPtr ReadOnlyStaticServerData;  // (PPVOID) 
        [FieldOffset(0x09c)]
        public IntPtr AnsiCodePageData;  // (PVOID) 
        [FieldOffset(0x0A4)]
        public IntPtr OemCodePageData;  // (PVOID) 
        [FieldOffset(0x0AC)]
        public IntPtr UnicodeCaseTableData;  // (PVOID) 
        [FieldOffset(0x0B4)]
#else
        [FieldOffset(0x008)]
        public IntPtr ImageBaseAddress; // (PVOID) 
        [FieldOffset(0x00c)]
        public IntPtr LoaderData; // (PPEB_LDR_DATA)
        [FieldOffset(0x010)]
        public IntPtr ProcessParameters; // (PRTL_USER_PROCESS_PARAMETERS)
        [FieldOffset(0x014)]
        public IntPtr SubSystemData;  // (PVOID) 
        [FieldOffset(0x018)]
        public IntPtr ProcessHeap;  // (PVOID) 
        [FieldOffset(0x01c)]
        public IntPtr FastPebLock; // (PRTL_CRITICAL_SECTION)  
        [FieldOffset(0x020)]
        public IntPtr FastPebLockRoutine; // (PPEBLOCKROUTINE)
        [FieldOffset(0x024)]
        public IntPtr FastPebUnlockRoutine;  // (PPEBLOCKROUTINE)
        [FieldOffset(0x028)]
        public UInt32 EnvironmentUpdateCount; // (ULONG)
        [FieldOffset(0x02c)]
        public IntPtr KernelCallbackTable;  // (PPVOID)  
        [FieldOffset(0x030)]
        public IntPtr SystemReserved;  // (PVOID)  
        [FieldOffset(0x034)]
        public IntPtr AtlThunkSListPtr32;  // (PVOID)  
        [FieldOffset(0x038)]
        public IntPtr FreeList;  // (PPEB_FREE_BLOCK) 
        [FieldOffset(0x03c)]
        public UInt32 TlsExpansionCounter;  // (ULONG) 
        [FieldOffset(0x040)]
        public IntPtr TlsBitmap;  // (PVOID)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        [FieldOffset(0x044)]
        public UInt32[] TlsBitmapBits;  // (ULONG) 
        [FieldOffset(0x04c)]
        public IntPtr ReadOnlySharedMemoryBase;  // (PVOID) 
        [FieldOffset(0x050)]
        public IntPtr ReadOnlySharedMemoryHeap;  // (PVOID) 
        [FieldOffset(0x054)]
        public IntPtr ReadOnlyStaticServerData;  // (PPVOID) 
        [FieldOffset(0x058)]
        public IntPtr AnsiCodePageData;  // (PVOID) 
        [FieldOffset(0x05c)]
        public IntPtr OemCodePageData;  // (PVOID) 
        [FieldOffset(0x060)]
        public IntPtr UnicodeCaseTableData;  // (PVOID) 
        [FieldOffset(0x064)]
#endif
        public UInt32 NumberOfProcessors;  // (ULONG) 
        [FieldOffset(0x068)]
        public UInt32 NtGlobalFlag;  // (ULONG) 
        [FieldOffset(0x070)]
        public Int64 CriticalSectionTimeout;  // (LARGE_INTEGER) 
        [FieldOffset(0x078)]
        public UInt32 HeapSegmentReserve;  // (ULONG) 
        [FieldOffset(0x07c)]
        public UInt32 HeapSegmentCommit;  // (ULONG) 
        [FieldOffset(0x080)]
        public UInt32 HeapDeCommitTotalFreeThreshold;  // (ULONG) 
        [FieldOffset(0x084)]
        public UInt32 HeapDeCommitFreeBlockThreshold;  // (ULONG) 
        [FieldOffset(0x088)]
        public UInt32 NumberOfHeaps;  // (ULONG) 
        [FieldOffset(0x08c)]
        public UInt32 MaximumNumberOfHeaps;  // (ULONG) 
        [FieldOffset(0x090)]
        public IntPtr ProcessHeaps;  // (PHEAP*) 
        [FieldOffset(0x094)]
        public IntPtr GdiSharedHandleTable;  // (PVOID) 
        [FieldOffset(0x098)]
        public IntPtr ProcessStarterHelper;  // (PVOID) 
        [FieldOffset(0x09c)]
        public IntPtr GdiDCAttributeList;  // (PVOID) 
        [FieldOffset(0x0a0)]
        public IntPtr LoaderLock;  // (PVOID) 
        [FieldOffset(0x0a4)]
        public UInt32 OSMajorVersion;  // (ULONG) 
        [FieldOffset(0x0a8)]
        public UInt32 OSMinorVersion;  // (ULONG) 
        [FieldOffset(0x0ac)]
        public UInt16 OSBuildNumber;  // (ULONG) 
        [FieldOffset(0x0ae)]
        public UInt16 OSCSDVersion;  // (ULONG) 
        [FieldOffset(0x0b0)]
        public UInt32 OSPlatformId;  // (ULONG) 
        [FieldOffset(0x0b4)]
        public UInt32 ImageSubSystem;   // (ULONG)
        [FieldOffset(0x0b8)]
        public UInt32 ImageSubSystemMajorVersion;   // (ULONG)
        [FieldOffset(0x0bc)]
        public UInt32 ImageSubSystemMinorVersion;  // (ULONG)  
        [FieldOffset(0x0c0)]
        public UInt32 ImageProcessAffinityMask;  // (ULONG)  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x22)]
        [FieldOffset(0x0c4)]
        public UInt32[] GdiHandleBuffer;  // (ULONG)   
        [FieldOffset(0x14c)]
        public UInt32 PostProcessInitRoutine;  // (ULONG) 
        [FieldOffset(0x150)]
        public IntPtr TlsExpansionBitmap;  // (ULONG)       
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
        [FieldOffset(0x154)]
        public byte[] TlsExpansionBitmapBits;  // (BYTE) 
        [FieldOffset(0x1d4)]
        public UInt32 SessionId;  // (ULONG)
        [FieldOffset(0x1d8)]
        public UInt64 AppCompatFlags;
        [FieldOffset(0x1e0)]
        public UInt64 AppCompatFlagsUser;
        [FieldOffset(0x1e8)]
        public IntPtr pShimData;
        [FieldOffset(0x1ec)]
        public IntPtr AppCompatInfo;
        [FieldOffset(0x1f0)]
        public UnicodeString CSDVersion;
        [FieldOffset(0x1f8)]
        public IntPtr ActivationContextData;
        [FieldOffset(0x1fc)]
        public IntPtr ProcessAssemblyStorageMap;
        [FieldOffset(0x200)]
        public IntPtr SystemDefaultActivationContextData;
        [FieldOffset(0x204)]
        public IntPtr SystemAssemblyStorageMap;
        [FieldOffset(0x208)]
        public UInt32 MinimumStackCommit;
    };
}
