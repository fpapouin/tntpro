using BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Generator
{
    public static class PvBuilder
    {
        ///----------------------------------------------------------------------------------------
        /// <signature>public static string CreateXml()</signature>
        ///
        /// <summary>Creates simple XML data</summary>
        ///
        /// <returns>The new XML</returns>
        ///----------------------------------------------------------------------------------------
        public static string CreateXml()
        {

            Root root = new Root();
            SetPropsValuesForString(root.Header);

            root.Records.Add(new Record());
            root.Records.Add(new Record());
            root.Records.Add(new Record());

            foreach (Record record in root.Records)
            {
                SetPropsValuesForString(record);

                for (int i = 0; i < 5; i++)
                {
                    record.MCMs.Add(new MCM());
                    SetPropsValuesForString(record.MCMs.Last());
                }

                for (int i = 0; i < 20; i++)
                {
                    record.Mesures.Add(new Mesure());
                    for (int j = 0; j < 3; j++)
                    {
                        record.Mesures.Last().Values.Add(new ValueTab());
                        SetPropsValuesForString(record.Mesures.Last().Values.Last());
                    }

                    for (int j = 0; j < 2; j++)
                    {
                        record.Mesures.Last().Tolerances.Add(new Tolerance());
                        SetPropsValuesForString(record.Mesures.Last().Tolerances.Last());

                        record.Mesures.Last().DynamicTolerances.Add(new Tolerance());
                        SetPropsValuesForString(record.Mesures.Last().DynamicTolerances.Last());
                    }

                    SetPropsValuesForString(record.Mesures.Last());

                }
            }


            return Serializer.SerializeRoot(root);
        }

        private static void SetPropsValuesForString(object o)
        {
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                if (p.PropertyType == typeof(string))
                    p.SetValue(o, p.Name);
            }
        }

    }
}
