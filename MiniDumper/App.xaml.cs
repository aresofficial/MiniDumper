using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace MiniDumper
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            GenerateDump();
            Environment.Exit(-1);
        }

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        public static extern uint GetCurrentThreadId();

        private void GenerateDump()
        {
            var exceptionInfo = new Dumper.ExceptionInfo
            {
                ThreadId = GetCurrentThreadId(),
                ExceptionPointers = Marshal.GetExceptionPointers(),
                ClientPointers = false
            };

            Dumper.Create("dump.dmp", exceptionInfo);
        }
    }
}
