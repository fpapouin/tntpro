//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;

using System;
using System.IO;
using System.Xml.Linq;
namespace XmlDataKeeper
{
    public static class DataKeeper
    {

        static XDocument xml = new XDocument();
        static string xmlFile;

        public static void SetConfigFile(string xmlFilePath)
        {
            if (File.Exists(xmlFilePath))
            {
                try
                {
                    xml = XDocument.Load(xmlFilePath);
                }
                catch (Exception)
                {
                    xml = new XDocument();
                }

            }
            else
            {
                xml = new XDocument();
            }

            xmlFile = xmlFilePath;
        }

        public static void Read(string path, out string value)
        {
            value = GetPathValue(path);
        }

        public static void Read(string path, out bool value)
        {
            if (string.IsNullOrEmpty(GetPathValue(path)))
            {
                value = false;
            }
            else
            {
                value = Convert.ToBoolean(GetPathValue(path));
            }
        }

        public static void Read(string path, out double value)
        {
            if (string.IsNullOrEmpty(GetPathValue(path)))
            {
                value = 0;
            }
            else
            {
                value = Convert.ToDouble(GetPathValue(path));
            }
        }

        public static void Write(string path, string value)
        {
            SetPathValue(path, value);
        }

        public static void Write(string path, bool value)
        {
            SetPathValue(path, Convert.ToString(value));
        }

        public static void Write(string path, double value)
        {
            SetPathValue(path, Convert.ToString(value));
        }

        private static string GetPathValue(string path)
        {

            var route = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            XElement item = null;
            string result = string.Empty;
            foreach (string node in route)
            {
                if (item == null)
                {
                    item = xml.Element(node);
                }
                else
                {
                    item = item.Element(node);
                }
            }

            if (item != null)
                result = item.Value;

            return result;
        }

        private static void SetPathValue(string path, string value)
        {

            var route = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);


            XElement item = xml.Root;

            if (item == null)
            {

                item = new XElement("root");
                xml.Add(item);
            }

            foreach (string node in route)
            {


                if (item.Element(node) == null)
                {
                    item = xml.Element(node);

                    if (item == null)
                    {
                        item.Add(new XElement(node));
                        item = item.Element(node);
                    }
                }
                else
                {
                    item = item.Element(node);
                }

            }

            item.Value = value;

            xml.Save(xmlFile);

        }

    }
}
