using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace tntpro
{
    class Helper
    {
        public static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).
            Replace("\\*", ".*").
            Replace("\\?", ".") + "$";
        }

        public static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"([{0}']*\.+$)|([{0}']+)", invalidChars);
            return Regex.Replace(name, invalidReStr, "_");
        }

        public static string GetToTheEndOfLine(string value, string find)
        {
            int indexStart = value.IndexOf(find) + find.Length;
            int indexStop = value.IndexOf("\r\n", indexStart);
            return value.Substring(indexStart, indexStop - indexStart);
        }

        public static string GetBetween(string value, string tokenStart, string tokenEnd)
        {
            int indexStart = value.IndexOf(tokenStart) + tokenStart.Length;
            int indexStop = value.IndexOf(tokenEnd, indexStart);
            return value.Substring(indexStart, indexStop - indexStart);
        }
        public static List<Programme> GetProgrammeList(string m3uFilename)
        {
            string m3u = File.OpenText(m3uFilename).ReadToEnd();
            m3u = m3u.Replace("dvb-t://", "@");
            string[] m3uTab = m3u.Split('@');

            string beforeName = "#EXTINF:0, ";
            string beforeFrequency = "#EXTVLCOPT:dvb-frequency=";
            string beforeCanal = "#EXTVLCOPT:program=";
            List<Programme> ProgrammeList = new List<Programme>();

            foreach (string s in m3uTab)
            {
                if (s.Contains(beforeName))
                    ProgrammeList.Add(
                        new Programme()
                        {
                            Name = GetToTheEndOfLine(s, beforeName),
                            Frequency = int.Parse(GetToTheEndOfLine(s, beforeFrequency)),
                            Canal = int.Parse(GetToTheEndOfLine(s, beforeCanal)),
                        });
            }

            //Reset prog name
            foreach (Programme prg in ProgrammeList)
            {
                prg.Name = prg.Name.ToLower();
                prg.Name = prg.Name.Trim();
                prg.Name = prg.Name.Replace(" ", "");
            }


            return ProgrammeList;
        }

        public static string GetVlcPath()
        {
            if (File.Exists(@"C:\Program Files\VideoLAN\VLC\vlc.exe"))
                return @"C:\Program Files\VideoLAN\VLC\vlc.exe";
            else if (File.Exists(@"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe"))
                return @"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe";
            else
                return "vlc.exe";
        }

        public static void CopyOldFile()
        {
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "old\\")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "old\\"));
            }

            foreach (string sourceFilePath in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.ts"))
            {
                if (new FileInfo(sourceFilePath).Length < (1024 * 1024 * 50))
                {
                    File.Delete(sourceFilePath);
                }
                else
                {
                    string destinationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "old\\", Path.GetFileName(sourceFilePath));
                    if (File.Exists(destinationFilePath))
                    {
                        File.Delete(destinationFilePath);
                    }
                    File.Move(sourceFilePath, destinationFilePath);
                }
            }
        }

        public static string BuildCmd(List<Programme> ProgrammeList)
        {
            //0 frequence du bouquet, 1 canaux du bouquet, 2 destination du bouquet
            string cmd = " --dvb-adapter=0 --dvb-bandwidth=8 --intf dummy dvb-t://frequency={0}";
            cmd = cmd + " :programs={1}";
            cmd = cmd + " --sout='#duplicate{{{2}}}' vlc://quit";

            int frequency = ProgrammeList.Select(p => p.Frequency).First();
            string programs = string.Join(",", ProgrammeList.Select(p => p.Canal));
            string pgrcmd = string.Join(",", ProgrammeList.Select(p => p.Destination));

            cmd = string.Format(cmd, frequency, programs, pgrcmd);

            return cmd.Replace("'", "\"");
        }

    }
}
