using System;
using System.Diagnostics;
using System.IO;

namespace vltream
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    throw new Exception(GetBadLenghtArgs());
                }
                else
                {
                    string filename = args[0];
                    string width = "1280";
                    string height = "720";
                    string dst = "8081";
                    string cmd = "\"" + @"C:\Program Files\VideoLAN\VLC\vlc.exe" + "\"";
                    if (!File.Exists(cmd))
                        cmd = "\"" + @"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe" + "\"";

                    if (Process.GetCurrentProcess().ProcessName.Contains("low"))
                    {
                        width = "640";
                        height = "360";
                    }
                    if (Process.GetCurrentProcess().ProcessName.Contains("ulow"))
                    {
                        width = "320";
                        height = "200";
                    }

                    if (args.Length > 1) width = args[1];
                    if (args.Length > 2) height = args[2];
                    if (args.Length > 3) dst = args[3];
                    if (args.Length > 4) cmd = args[4];

                    string cmdargs = "\"{0}\" ";
                    cmdargs = cmdargs + "--sout=\"#transcode{vcodec=h264,vb=2000,venc=x264{profile=baseline},scale=Auto,";
                    cmdargs = cmdargs + "width={1},height={2},acodec=mp3,ab=192,channels=2,samplerate=44100,scodec=dvbs,soverlay}";
                    cmdargs = cmdargs + ":http{mux=ts,dst=:{3}/}\" --sout-keep";

                    //cmdargs = String.Format(cmdargs, filename, width, height, dst);
                    cmdargs = cmdargs.Replace("{0}", filename);
                    cmdargs = cmdargs.Replace("{1}", width);
                    cmdargs = cmdargs.Replace("{2}", height);
                    cmdargs = cmdargs.Replace("{3}", dst);
                    Console.WriteLine(cmd);
                    Console.WriteLine(cmdargs);
                    Process.Start(cmd, cmdargs);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey(true);
            }
        }

        static string GetBadLenghtArgs()
        {
            string result = "Erreur dans les arguments";
            result = result + "\n" + "args0=Filename.avi";
            result = result + "\n" + "args1=1280 width (opt)";
            result = result + "\n" + "args2=720 height (opt)";
            result = result + "\n" + "args3=8081 dst (opt)";

            return result;
        }
    }
}
