using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DeleteEmptyFolder
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            DeleteEmptySubFolder(di);
  
        }

        private static void DeleteEmptySubFolder(DirectoryInfo di)
        {
            foreach (DirectoryInfo ddi in di.GetDirectories())
            {
                DeleteEmptySubFolder(ddi);
                if (ddi.GetFiles("*.*", SearchOption.AllDirectories).Count() == 0)
                    ddi.Delete(true);
            }
        }
        
        private void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            }
            foreach (FileInfo file in source.GetFiles())
            {
                // Ignore readonly files
                if ((File.GetAttributes(file.FullName) & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
                {
                    file.CopyTo(Path.Combine(target.FullName, file.Name));
                }
            }
        }
    }
}
