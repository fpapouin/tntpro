using System;
using XmlDataKeeper;

namespace DataKeeperConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DataKeeper.SetConfigFile("Calibration.xml");

            string teststring = string.Empty;
            double testnumber = 0;
            //  DataKeeper.Read("/CONFIG/DEVICE/INIT_ORDER/", out teststring);
            //  DataKeeper.Read("/CONFIG/DEVICE/INIT_ORDER/", out testnumber);
            //  DataKeeper.Write("/aa/bb/cc///", "coucou");
            //  DataKeeper.Write("/aa/bb/dd///", 1234567890);
            //  DataKeeper.Write("/aa/bb/ee///", false);
            DataKeeper.Read("CONFIG/CALIBRATION/PERTEL1", out testnumber);
            DataKeeper.Write("CONFIG/CALIBRATION/PERTEL1", testnumber * 2);
            Console.WriteLine(teststring);
            Console.WriteLine(testnumber);
            Console.ReadKey();
        }
    }
}
