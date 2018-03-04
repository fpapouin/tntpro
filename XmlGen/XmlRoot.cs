using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Generator
{
    public class Root
    {
        public Header Header { get; set; }
        public List<Record> Records { get; set; }
        public Root()
        {
            Header = new Header();
            Records = new List<Record>();
        }
    }

    public class Header
    {
        public string ItemReference { get; set; }
        public string ItemNumber { get; set; }
        public string ItemSerialNumber { get; set; }
        public string ItemDescription { get; set; }
        public string BankNumber { get; set; }
        public string BankSerialNumber { get; set; }
        public string PvcNumber { get; set; }
        public string PvcVersion { get; set; }
        public string Nos_specif { get; set; }
        public string Nos_Cp { get; set; } //Not in template Nos_Cp
        public string GlobalSanction { get; set; }
        public string GeneratedDate { get; set; }
        public string Checksum { get; set; }
        public List<Model> Models { get; set; }

        public Header()
        {
            Models = new List<Model>();
        }
    }

    public class Model
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class Record
    {
        public string BenchReference { get; set; }
        public string BenchSerialNumber { get; set; }
        public string BenchLocation { get; set; }
        public string BenchDescription { get; set; }
        public string BenchMCMCode { get; set; }
        public string BenchSoftwareNumber { get; set; }
        public string BenchSoftwareVersion { get; set; }
        public string SoftwareNumberVersion { get; set; }
        public string SequenceNumberVersion { get; set; }
        public string SequenceDescription { get; set; }
        public string Operator { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SupportLocation { get; set; }
        public string Comment { get; set; }
        public string RunState { get; set; }
        public string Sanction { get; set; }

        public List<MCM> MCMs { get; set; }
        public List<Mesure> Mesures { get; set; }
        public Record()
        {
            MCMs = new List<MCM>();
            Mesures = new List<Mesure>();
        }
    }

    public class MCM
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Serial { get; set; }
    }

    public class Mesure
    {
        public string Id { get; set; }
        public string Identification { get; set; }
        public string Index { get; set; }
        public string ResultId { get; set; }
        public string IsCritical { get; set; }
        public string Requirement { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsPVC { get; set; }
        public string IsPVAI { get; set; }
        public string Unit { get; set; }
        public string Type { get; set; }
        public string DecimalNumber { get; set; }
        public string IsArray { get; set; }
        public string Value { get; set; }
        public List<ValueTab> Values { get; set; }
        public List<Tolerance> Tolerances { get; set; }
        public List<Tolerance> DynamicTolerances { get; set; }
        public string Sanction { get; set; }
        public Mesure()
        {
            Values = new List<ValueTab>();
            Tolerances = new List<Tolerance>();
            DynamicTolerances = new List<Tolerance>();
        }
    }

    public class ValueTab
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Sanction { get; set; }
        public string Label { get; set; }
    }

    public class Tolerance
    {
        public string Id { get; set; }
        public string Identification { get; set; }
        public string ComparaisonType { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string Expected { get; set; }
    }

    public static class Serializer
    {
        public static string SerializeRoot(Root root)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Root));
            string s;

            XmlWriterSettings set = new XmlWriterSettings();
            set.Indent = true;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (StringWriter textWriter = new StringWriter())
            {
                x.Serialize(textWriter, root, ns);
                s = textWriter.ToString();
            }

            return s;
        }
    }
}
