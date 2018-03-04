using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace tnt
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CopyOldFile();

                if (args.Length < 2)
                {
                    throw new Exception(GetBadLenghtArgs());
                }
                else
                {
                    List<Programme> ProgrammeList = GetProgrammeList(args[0]);
                    List<Programme> BouquetList = new List<Programme>();

                    int frq;
                    if (int.TryParse(args[1], out frq))
                    {
                        BouquetList = ProgrammeList.FindAll(p => p.Frequency == frq);
                    }
                    else
                    {
                        foreach (string chaine in args[1].Split(','))
                        {
                            Regex regEx = new Regex(Helper.WildcardToRegex(chaine), RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
                            BouquetList.AddRange(ProgrammeList.Where(p => regEx.IsMatch(p.Name)));
                        }
                    }

                    if (BouquetList.Count == 0)
                    {
                        throw new Exception(PrintListWithMessage("Erreur: aucune chaine trouvée" + "\n" + "Voici les chaines disponibles\n", ProgrammeList));
                    }
                    else if (BouquetList.Select(p => p.Frequency).Distinct().Count() == 1)
                    {
                        string cmdargs = BuildCmd(BouquetList);
                        Console.WriteLine(cmdargs);

                        if (args.Length == 3)
                        {
                            Process.Start(@args[2], cmdargs);
                        }
                        else
                        {
                            Process.Start(@"C:\Program Files\VideoLAN\VLC\vlc.exe", cmdargs);
                        }
                    }
                    else
                    {
                        throw new Exception(PrintListWithMessage("Erreur: plus d'un bouquet trouvée\n", BouquetList));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey(true);
            }
        }


        static void CopyOldFile()
        {
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

        static string GetBadLenghtArgs()
        {
            string result = "Erreur: deux arguments minimums ex:";
            result = result + "\n" + "args0=Filename.m3u";
            result = result + "\n" + "args1=498166000";

            result = result + "\n" + "Le deuxieme argument peut etre un nom de chaine ex:";
            result = result + "\n" + "args1=nrj12";
            result = result + "\n" + "ou plusieurs noms separés par une virgule ex:";
            result = result + "\n" + "args1=\"france 2 HD, france 3\"";

            result = result + "\n" + "La case n'a pas d'importance";
            result = result + "\n" + "et le nom peut etre un morceau du nom complet ex:";
            result = result + "\n" + "args1=\"*HD, fr?nce*\"";
            return result;
        }

        static List<Programme> GetProgrammeList(string m3uFilename)
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
                            Name = Helper.GetToTheEndOfLine(s, beforeName),
                            Frequency = int.Parse(Helper.GetToTheEndOfLine(s, beforeFrequency)),
                            Canal = int.Parse(Helper.GetToTheEndOfLine(s, beforeCanal)),
                        });
            }

            return ProgrammeList;
        }

        static string BuildCmd(List<Programme> ProgrammeList)
        {
            //0 frequence du bouquet, 1canaux du bouquet, 2 destination du bouquet
            string cmd = " --dvb-adapter=0 --dvb-bandwidth=8 --intf dummy dvb-t://frequency={0}";
            cmd = cmd + " :programs={1}";
            cmd = cmd + " --sout='#duplicate{{{2}}}' vlc://quit";

            int frequency = ProgrammeList.Select(p => p.Frequency).First();
            string programs = string.Join(",", ProgrammeList.Select(p => p.Canal));
            string pgrcmd = string.Join(",", ProgrammeList.Select(p => p.Destination));

            cmd = string.Format(cmd, frequency, programs, pgrcmd);

            return cmd.Replace("'", "\"");
        }

        static string PrintListWithMessage(string message, List<Programme> ProgrammeList)
        {

            foreach (Programme p in ProgrammeList)
            {
                message = message + p.Frequency + " " + p.Name + "\n";
            }

            return message;

        }
    }




}
