#define debug

using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;
using System.Windows.Interop;

namespace DDD_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        const int SWP_NOZORDER = 0x04;
        const int SWP_NOACTIVATE = 0x10;

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int cx, int cy, int flags);



        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if debug
            AllocConsole();

            SetWindowPosition(0, 0, 600, 400);
#endif
            DDD.Program.Sequence.FirstExecute firstExecute = new DDD.Program.Sequence.FirstExecute();
#if debug
            firstExecute.TestFile();
#endif
            firstExecute.StartUp(1);
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            System.Environment.Exit(0);
        }


        /// <summary>
        /// Sets the console window location and size in pixels
        /// </summary>
        public static void SetWindowPosition(int x, int y, int width, int height)
        {
            SetWindowPos(Handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
        }

        public static IntPtr Handle
        {
            get
            {
                //Initialize();
                return GetConsoleWindow();
            }
        }


    }
}
