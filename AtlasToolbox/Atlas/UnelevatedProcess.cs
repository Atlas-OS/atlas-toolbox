using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AtlasToolbox.Atlas
{
    /// <summary>
    /// Starts a process as the signed-in user without the Toolbox's administrator rights,
    /// as if the user had opened it from the desktop. The process is created with the
    /// shell (Explorer) as its parent, so it takes the shell's token rather than the
    /// Toolbox's elevated one, and the environment Windows builds for that user.
    /// </summary>
    internal static class UnelevatedProcess
    {
        private const int ERROR_ACCESS_DENIED = 5;

        public static SafeProcessHandle Start(string application, string commandLine, string workingDirectory, bool hidden)
        {
            IntPtr shellWindow = GetShellWindow();
            if (shellWindow == IntPtr.Zero)
            {
                throw new InvalidOperationException("Windows has no desktop shell to start this as the signed-in user.");
            }
            if (GetWindowThreadProcessId(shellWindow, out uint shellProcessId) == 0) throw new Win32Exception();

            using SafeProcessHandle shell = OpenProcess(PROCESS_CREATE_PROCESS | PROCESS_QUERY_LIMITED_INFORMATION, false, shellProcessId);
            if (shell.IsInvalid) throw ShellAccessFailure();
            if (!OpenProcessToken(shell, TOKEN_QUERY | TOKEN_DUPLICATE, out SafeAccessTokenHandle shellToken)) throw ShellAccessFailure();

            IntPtr environment = IntPtr.Zero;
            IntPtr attributeListSize = IntPtr.Zero;
            InitializeProcThreadAttributeList(IntPtr.Zero, 1, 0, ref attributeListSize);
            IntPtr attributeList = Marshal.AllocHGlobal(attributeListSize);
            IntPtr parent = Marshal.AllocHGlobal(IntPtr.Size);
            try
            {
                if (!CreateEnvironmentBlock(out environment, shellToken, false)) throw new Win32Exception();
                if (!InitializeProcThreadAttributeList(attributeList, 1, 0, ref attributeListSize)) throw new Win32Exception();
                try
                {
                    Marshal.WriteIntPtr(parent, shell.DangerousGetHandle());
                    if (!UpdateProcThreadAttribute(attributeList, 0, PROC_THREAD_ATTRIBUTE_PARENT_PROCESS, parent, IntPtr.Size, IntPtr.Zero, IntPtr.Zero))
                    {
                        throw new Win32Exception();
                    }

                    STARTUPINFOEX startup = new();
                    startup.StartupInfo.cb = Marshal.SizeOf<STARTUPINFOEX>();
                    startup.StartupInfo.dwFlags = STARTF_USESHOWWINDOW;
                    startup.StartupInfo.wShowWindow = hidden ? SW_HIDE : SW_SHOWNORMAL;
                    startup.lpAttributeList = attributeList;

                    // A hidden run gets no console at all, so no terminal can show one.
                    uint flags = EXTENDED_STARTUPINFO_PRESENT | CREATE_UNICODE_ENVIRONMENT | (hidden ? CREATE_NO_WINDOW : CREATE_NEW_CONSOLE);
                    // The console's parent is Explorer, so it doesn't inherit the Toolbox's
                    // right to come to the front; the Toolbox, in the foreground, passes it on.
                    if (!hidden) AllowSetForegroundWindow(ASFW_ANY);
                    StringBuilder mutableCommandLine = new(commandLine);
                    if (!CreateProcess(application, mutableCommandLine, IntPtr.Zero, IntPtr.Zero, false,
                        flags, environment, workingDirectory, ref startup, out PROCESS_INFORMATION info))
                    {
                        throw new Win32Exception();
                    }
                    CloseHandle(info.hThread);
                    return new SafeProcessHandle(info.hProcess, true);
                }
                finally
                {
                    DeleteProcThreadAttributeList(attributeList);
                }
            }
            finally
            {
                if (environment != IntPtr.Zero) DestroyEnvironmentBlock(environment);
                shellToken.Dispose();
                Marshal.FreeHGlobal(parent);
                Marshal.FreeHGlobal(attributeList);
            }
        }

        /// <summary>
        /// The shell belongs to another account when the Toolbox was opened with a different
        /// administrator's credentials; the user's own settings can't be changed from there.
        /// </summary>
        private static Exception ShellAccessFailure()
        {
            int error = Marshal.GetLastWin32Error();
            return error == ERROR_ACCESS_DENIED
                ? new InvalidOperationException("The Toolbox can't change the signed-in user's own settings because it was opened with another account's administrator rights. Use this setting's launcher in the Atlas folder instead.")
                : new Win32Exception(error);
        }

        /// <summary>Waits for a process started here to end and returns its exit code.</summary>
        public static async Task<int> WaitForExitAsync(SafeProcessHandle process)
        {
            using ProcessWaitHandle wait = new(process);
            TaskCompletionSource ended = new(TaskCreationOptions.RunContinuationsAsynchronously);
            RegisteredWaitHandle registration = ThreadPool.RegisterWaitForSingleObject(wait, (_, _) => ended.TrySetResult(), null, Timeout.Infinite, true);
            try
            {
                await ended.Task.ConfigureAwait(false);
            }
            finally
            {
                registration.Unregister(null);
            }
            if (!GetExitCodeProcess(process, out uint exitCode)) throw new Win32Exception();
            return unchecked((int)exitCode);
        }

        private sealed class ProcessWaitHandle : WaitHandle
        {
            public ProcessWaitHandle(SafeProcessHandle process)
            {
                SafeWaitHandle = new SafeWaitHandle(process.DangerousGetHandle(), false);
            }
        }

        private const uint ASFW_ANY = unchecked((uint)-1);
        private const uint PROCESS_CREATE_PROCESS = 0x0080;
        private const uint PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;
        private const uint TOKEN_DUPLICATE = 0x0002;
        private const uint TOKEN_QUERY = 0x0008;
        private const uint EXTENDED_STARTUPINFO_PRESENT = 0x00080000;
        private const uint CREATE_UNICODE_ENVIRONMENT = 0x00000400;
        private const uint CREATE_NEW_CONSOLE = 0x00000010;
        private const uint CREATE_NO_WINDOW = 0x08000000;
        private const int STARTF_USESHOWWINDOW = 0x00000001;
        private const short SW_HIDE = 0;
        private const short SW_SHOWNORMAL = 1;
        private static readonly IntPtr PROC_THREAD_ATTRIBUTE_PARENT_PROCESS = new(0x00020000);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct STARTUPINFOEX
        {
            public STARTUPINFO StartupInfo;
            public IntPtr lpAttributeList;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllowSetForegroundWindow(uint processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeProcessHandle OpenProcess(uint desiredAccess, bool inheritHandle, uint processId);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenProcessToken(SafeProcessHandle process, uint desiredAccess, out SafeAccessTokenHandle token);

        [DllImport("userenv.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CreateEnvironmentBlock(out IntPtr environment, SafeAccessTokenHandle token, bool inherit);

        [DllImport("userenv.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyEnvironmentBlock(IntPtr environment);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool InitializeProcThreadAttributeList(IntPtr attributeList, int attributeCount, int flags, ref IntPtr size);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UpdateProcThreadAttribute(IntPtr attributeList, uint flags, IntPtr attribute, IntPtr value, IntPtr size, IntPtr previousValue, IntPtr returnSize);

        [DllImport("kernel32.dll")]
        private static extern void DeleteProcThreadAttributeList(IntPtr attributeList);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateProcessW")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CreateProcess(string applicationName, StringBuilder commandLine, IntPtr processAttributes, IntPtr threadAttributes,
            bool inheritHandles, uint creationFlags, IntPtr environment, string currentDirectory, ref STARTUPINFOEX startupInfo, out PROCESS_INFORMATION processInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetExitCodeProcess(SafeProcessHandle process, out uint exitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr handle);
    }
}
