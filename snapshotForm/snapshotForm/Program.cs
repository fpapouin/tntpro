using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snapshotForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Clipboard.ContainsImage())
            {
                Clipboard.GetImage().Save(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\" + DateTime.Now.Ticks + ".png");
            }
        }
    }
}
