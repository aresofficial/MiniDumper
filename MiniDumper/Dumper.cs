using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace MiniDumper
{
    public static class Dumper
    {
        [Flags]
        public enum Type
        {
            Normal = 0x00000000,
            WithDataSegs = 0x00000001,
            WithFullMemory = 0x00000002,
            WithHandleData = 0x00000004,
            FilterMemory = 0x00000008,
            ScanMemory = 0x00000010,
            WithUnloadedModules = 0x00000020,
            WithIndirectlyReferencedMemory = 0x00000040,
            FilterModulePaths = 0x00000080,
            WithProcessThreadData = 0x00000100,
            WithPrivateReadWriteMemory = 0x00000200,
            WithoutOptionalData = 0x00000400,
            WithFullMemoryInfo = 0x00000800,
            WithThreadInfo = 0x00001000,
            WithCodeSegs = 0x00002000,
            WithoutAuxiliaryState = 0x00004000,
            WithFullAuxiliaryState = 0x00008000,
            WithPrivateWriteCopyMemory = 0x00010000,
            IgnoreInaccessibleMemory = 0x00020000,
            ValidTypeFlags = 0x0003ffff
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct ExceptionInfo
        {
            public uint ThreadId;
            public IntPtr ExceptionPointers;
            [MarshalAs(UnmanagedType.Bool)]
            public bool ClientPointers;
        }

        [DllImport("dbghelp.dll")]
        static extern bool MiniDumpWriteDump(IntPtr hProcess,
                                                       int ProcessId,
                                                       IntPtr hFile,
                                                       int DumpType,
                                                       ref ExceptionInfo ExceptionParam,
                                                       IntPtr UserStreamParam,
                                                       IntPtr CallackParam);

        public static void Create(string fileName, ExceptionInfo exceptionInfo, Type? type = null)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                using (Process process = Process.GetCurrentProcess())
                {
                    var flags = Type.WithFullMemory | Type.WithFullMemoryInfo | Type.WithHandleData | Type.WithUnloadedModules | Type.WithThreadInfo;
                    MiniDumpWriteDump(process.Handle,
                                                     process.Id,
                                                     fs.SafeFileHandle.DangerousGetHandle(),
                                                     (type != null) ? (int)type : (int)flags,
                                                     ref exceptionInfo,
                                                     IntPtr.Zero,
                                                     IntPtr.Zero);
                }
            }
        }
    }
}
