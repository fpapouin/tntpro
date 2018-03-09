using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Timers;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Aging
{
    class Program
    {
        static void Main(string[] args)
        {
            int minutes = 1;
            if (args.Count() == 2)
            {
                minutes = Convert.ToInt32(args[1]);
            }

            if (args.Count() > 0)
            {
                procName = args[0];
            }
#if !DEBUG
            Timer timer = new Timer(minutes * 60 * 1000);
#endif
#if DEBUG
            Timer timer = new Timer(1000);
#endif
            timer.Elapsed += OnTimedEvent;
            timer.Start();

            Console.WriteLine("Press any key to exit... ");
            Console.ReadKey();
            timer.Stop();
            timer.Dispose();
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            (source as Timer).Stop();

            Process AnalysedProcess = Process.GetProcessesByName(procName).FirstOrDefault();

            if (AnalysedProcess != null)
            {
                int usedRam = (int)(AnalysedProcess.WorkingSet64 / (1024 * 1024));

                PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", AnalysedProcess.ProcessName);

                double usedProc = 0;
                do
                {
                    usedProc = cpuCounter.NextValue() / Environment.ProcessorCount;
                } while (usedProc == 0);

                Console.WriteLine("At {0:HH:mm:ss.fff} \t Ram={1} \t Proc={2}%", e.SignalTime, usedRam, usedProc);
#if !DEBUG
                File.AppendAllText("TimedEvent.txt", string.Format("{0:HH:mm:ss.fff}\tRam\t{1}\tProc\t{2}%\r\n", e.SignalTime, usedRam, usedProc));
                Screen(string.Format("{0}h{1}min{2}s.png", e.SignalTime.Hour, e.SignalTime.Minute, e.SignalTime.Second), AnalysedProcess.MainWindowHandle);
#endif
            }
            else
            {
                Console.WriteLine("Press any key to exit... ");
            }
            (source as Timer).Start();
        }

        private static void Screen(string filename, IntPtr MainWindowHandle)
        {
            var rect = new Rect();

            GetWindowRect(MainWindowHandle, ref rect);
            Rectangle bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                bitmap.Save(filename, ImageFormat.Png);
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
