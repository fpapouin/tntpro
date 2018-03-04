using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace renLight
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            GoSubFolder(di);
        }

        private static void GoSubFolder(DirectoryInfo di)
        {
            foreach (DirectoryInfo ddi in di.GetDirectories())
            {
                GoSubFolder(ddi);
            }
            foreach (FileInfo fi in di.GetFiles("no_light*.png"))
            {
                Ren(fi);
            }
        }

        static void Ren(FileInfo fileInfo)
        {

            string suffix = "";

            if (fileInfo.Name.Length > "no_lightxxx.png".Length) //ex no_light_texture110.png
            {
                suffix = fileInfo.Name.Substring("no_light".Length); //remove no_light
                suffix = suffix.Substring(0, suffix.Length - 7); //remove 110.png
                //suffix = _texture
            }


            if (fileInfo.Name != "no_light.png")
            {
                string s = "";

                if (fileInfo.Name.Contains("110"))
                    s = "lightingAllOn" + suffix + "_layerOff";

                if (fileInfo.Name.Contains("111"))
                    s = "lightingAllOn" + suffix + "_layerOn";

                if (fileInfo.Name.Contains("100"))
                    s = "lightingAllOff" + suffix + "_layerOff";

                if (fileInfo.Name.Contains("101"))
                    s = "lightingAllOff" + suffix + "_layerOn";

                if (fileInfo.Name.Contains("010"))
                    s = "lightingIndividual" + suffix + "_layerOff";

                if (fileInfo.Name.Contains("011"))
                    s = "lightingIndividual" + suffix + "_layerOn";

                try
                {
                    fileInfo.MoveTo(fileInfo.DirectoryName + "/" + s + ".png");
                }
                catch (Exception)
                {

                }
            }




        }
    }
}
