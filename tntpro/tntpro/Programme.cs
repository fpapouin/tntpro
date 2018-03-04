using System;

namespace tntpro
{
    class Programme
    {
        private string name;
        private int canal;

        public string Name { get { return name; } set { name = value; ReBuild(); } }
        public int Frequency { get; set; }
        public int Canal { get { return canal; } set { canal = value; ReBuild(); } }
        public string Filename { get; private set; }
        public string Destination { get; private set; }

        void ReBuild()
        {
            Filename = DateTime.Now.ToString("ddd-dd-MMM HH-mm ") + Helper.MakeValidFileName(name) + ".ts";
            Destination = string.Format("dst=std{{dst={0},access=file,mux=ts}},select='program={1}'", Filename, canal);
        }

    }
}
