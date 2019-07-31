using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utility;

namespace PnGen
{
    class PnGen
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string dumpFilepath = @"dump.dmp";
            string palletFilepath = @"volcano_def.RES.vpf";
            string qualifname = "received_power";
            string qualifunit = "db_mw";
            string outPngPath = @"out.png";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    switch (args[i])
                    {
                        case "-d":
                            dumpFilepath = args[i + 1];
                            break;
                        case "-p":
                            palletFilepath = args[i + 1];
                            break;
                        case "-qn":
                            qualifname = args[i + 1];
                            break;
                        case "-qu":
                            qualifunit = args[i + 1];
                            break;
                        case "-o":
                            outPngPath = args[i + 1];
                            break;
                        case "-h":
                            Help();
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Unknown argument " + args[i]);
                            break;
                    }
                }
            }

            if (!File.Exists(dumpFilepath) && dumpFilepath != "dump.dmp")
            {
                Console.WriteLine("ERROR : File not found dumpFilepath " + dumpFilepath);
                Help();
                Environment.Exit(-1);
            }
            else if (!File.Exists(palletFilepath))
            {
                Console.WriteLine("ERROR : File not found palletFilepath " + palletFilepath);
                Help();
                Environment.Exit(-1);
            }

            //Read dump
            double[,] dumpArray = ReadDump(dumpFilepath);

            //Read Palette
            Pallet pallet = ReadPal(palletFilepath, qualifname, qualifunit);

            //Gen Png
            GenPng(dumpArray, pallet, outPngPath);
        }

        static void Help()
        {
            Console.WriteLine("-h this help");
            Console.WriteLine("-d dumpFilepath  [default = dump.dmp] (use random data if default)");
            Console.WriteLine("-p palletFilepath [default = volcano_def.RES.vpf]");
            Console.WriteLine("-qn qualifname [default = received_power]");
            Console.WriteLine("-qu qualifunit [default = db_mw]");
            Console.WriteLine("-o outPngPath [default = out.png]");
        }

        static void GenPng(double[,] dumpArray, Pallet pallet, string outPngPath)
        {
            Bitmap outPng = new Bitmap(dumpArray.GetLength(0), dumpArray.GetLength(1));
            for (int x = 0; x < outPng.Width; x++)
            {
                for (int y = 0; y < outPng.Height; y++)
                {
                    double dumpVal = dumpArray[x, y];
                    outPng.SetPixel(x, y, pallet.undefinedvalue.color);

                    if (dumpVal == pallet.invalidvalue.value)
                    {
                        outPng.SetPixel(x, y, pallet.invalidvalue.color);
                    }
                    else if(dumpVal == pallet.notrequiredvalue.value)
                    {
                        outPng.SetPixel(x, y, pallet.notrequiredvalue.color);
                    }
                    else if (dumpVal == pallet.underthresholdvalue.value)
                    {
                        outPng.SetPixel(x, y, pallet.underthresholdvalue.color);
                    }
                    else if (dumpVal < pallet.seqfloatassociation.Min(fa => fa.lowerbound) || dumpVal >= pallet.seqfloatassociation.Max(fa => fa.upperbound))
                    {
                        outPng.SetPixel(x, y, pallet.notrequiredvalue.color);
                    }
                    else
                    {
                        foreach (ColorFloatIntervalAssociation cfia in pallet.seqfloatassociation)
                        {
                            if (dumpVal >= cfia.lowerbound && dumpVal < cfia.upperbound)
                            {
                                outPng.SetPixel(x, y, cfia.color);
                            }
                        }
                    }
                }
            }
            outPng = ImageUtils.CreateIndexedPng(outPng);
            outPng.Save(outPngPath, System.Drawing.Imaging.ImageFormat.Png);
        }

        static public Bitmap CreateIndexedImage(Image src)
        {
            Bitmap newBmp = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            using (Graphics gfx = Graphics.FromImage(newBmp))
            {
                gfx.DrawImage(src, 0, 0);
            }

            return newBmp;
        }

        static double[,] ReadDump(string filepath)
        {
            double[,] dumpArray = null;
            if (filepath == "dump.dmp")
            {
                //Fake dump
                int height = 10;
                int width = 10;
                dumpArray = new double[width, height];
                Random rnd = new Random();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        dumpArray[x, y] = -rnd.NextDouble() * 250;
                        File.AppendAllText(filepath, dumpArray[x, y] + " ");
                    }
                    File.AppendAllText(filepath, Environment.NewLine);
                }
            }
            else
            {
                List<string> dumpLines = System.IO.File.ReadLines(filepath).ToList();
                int height = dumpLines.Count;
                if (string.IsNullOrWhiteSpace(dumpLines.Last()))
                {
                    height = height - 1;
                }
                int width = dumpLines[0].Split(' ').Count();
                if (string.IsNullOrWhiteSpace(dumpLines[0].Split(' ').Last()))
                {
                    width = width - 1;
                }
                dumpArray = new double[width, height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        dumpArray[x, y] = Double.Parse(dumpLines[y].Split(' ')[x]);
                    }
                }
            }
            return dumpArray;
        }

     

        static Pallet ReadPal(string filepath, string qualifname, string qualifunit)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            XmlNode node = doc.DocumentElement.SelectSingleNode(string.Format("/root/pallet[qualifname=\"{0}\"][qualifunit=\"{1}\"]", qualifname, qualifunit));

            Pallet pallet = new Pallet();
            pallet.qualifname = node.SelectSingleNode("qualifname").InnerText;
            pallet.qualifunit = node.SelectSingleNode("qualifunit").InnerText;
            pallet.undefinedvalue = ParseNodeToColorIntAssociation(node.SelectSingleNode("undefinedvalue"));
            pallet.invalidvalue = ParseNodeToColorIntAssociation(node.SelectSingleNode("invalidvalue"));
            pallet.notrequiredvalue = ParseNodeToColorIntAssociation(node.SelectSingleNode("notrequiredvalue"));
            pallet.underthresholdvalue = ParseNodeToColorIntAssociation(node.SelectSingleNode("underthresholdvalue"));

            foreach (XmlNode n in node.SelectNodes("seqfloatassociation/floatassociation"))
            {
                pallet.seqfloatassociation.Add(ParseNodeToColorFloatIntervalAssociation(n));
            }

            return pallet;
        }

        static ColorIntAssociation ParseNodeToColorIntAssociation(XmlNode node)
        {
            ColorIntAssociation cia = new ColorIntAssociation();
            cia.value = int.Parse(node.SelectSingleNode("value").InnerText);
            cia.color = ParseNodeToColor(node.SelectSingleNode("color"));
            return cia;
        }

        static ColorFloatIntervalAssociation ParseNodeToColorFloatIntervalAssociation(XmlNode node)
        {
            ColorFloatIntervalAssociation cfia = new ColorFloatIntervalAssociation();
            cfia.lowerbound = double.Parse(node.SelectSingleNode("lowerbound").InnerText);
            cfia.upperbound = double.Parse(node.SelectSingleNode("upperbound").InnerText);
            cfia.color = ParseNodeToColor(node.SelectSingleNode("color"));
            return cfia;
        }

        static Color ParseNodeToColor(XmlNode node)
        {
            int r = int.Parse(node.SelectSingleNode("r").InnerText);
            int g = int.Parse(node.SelectSingleNode("g").InnerText);
            int b = int.Parse(node.SelectSingleNode("b").InnerText);
            int a = int.Parse(node.SelectSingleNode("t").InnerText);
            Color c = Color.FromArgb(a, r, g, b);
            return c;
        }
    }

    public class Pallet
    {
        public string qualifname;
        public string qualifunit;
        public ColorIntAssociation undefinedvalue = new ColorIntAssociation();
        public ColorIntAssociation invalidvalue = new ColorIntAssociation();
        public ColorIntAssociation notrequiredvalue = new ColorIntAssociation();
        public ColorIntAssociation underthresholdvalue = new ColorIntAssociation();
        public List<ColorFloatIntervalAssociation> seqfloatassociation = new List<ColorFloatIntervalAssociation>();
    }

    public class ColorIntAssociation
    {
        public int value;
        public Color color = new Color();
    }

    public class ColorFloatIntervalAssociation
    {
        public double lowerbound;
        public double upperbound;
        public Color color = new Color();
    }
}
