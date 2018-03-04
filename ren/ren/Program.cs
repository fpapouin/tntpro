using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ren
{
    class Program
    {
        static void Main(string[] args)
        {

            foreach (FileInfo fi in new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("*ref.png"))
            {
                Ren(fi);
            }

            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            GoSubFolder(di);
        }

        private static void GoSubFolder(DirectoryInfo di)
        {
            foreach (DirectoryInfo ddi in di.GetDirectories())
            {
                GoSubFolder(ddi);
            }
            foreach (FileInfo fi in di.GetFiles("*ref.png"))
            {
                Ren(fi);
            }
        }

        static void Ren(FileInfo fileInfo)
        {
            //ex not_visible_58_ref.png => not_visible.png
            if (fileInfo.Name.Contains("_"))
            {
                List<string> ls = new List<string>(fileInfo.Name.Split('_'));
                ls.Reverse(); //reverse to be sure to remove the last 
                ls.Remove(ls.First()); //ref.png
                ls.Remove(ls.First()); //58
                ls.Reverse();
                try
                {
                    fileInfo.MoveTo(fileInfo.DirectoryName + "/" + string.Join("_", ls) + ".png");
                }
                catch (Exception)
                {

                }
            }
        }

    }
}
