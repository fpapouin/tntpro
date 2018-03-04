using System;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace Tools_GraphDrawer
{
    public class GraphDrawer : IDisposable
    {

        private Chart chart;
        public ChartArea ca;
        private Title title;

        public GraphDrawer(out bool errorOccurred, out int errorCode, out string errorMessage, string title = "Title1", int width = 1920, int height = 1080)
        {
            ResetParamsErrors(out errorOccurred, out errorCode, out errorMessage);
            try
            {
                //Create Graph representive the curve
                chart = new Chart();
                chart.Size = new Size(width, height);

                //Title
                chart.Titles.Clear();
                this.title = chart.Titles.Add(title);

                //Zone
                chart.ChartAreas.Clear();
                ca = chart.ChartAreas.Add("ChartArea1");

                //Series
                chart.Series.Clear();
            }
            catch (Exception ex)
            {
                errorOccurred = true;
                errorCode = ex.HResult;
                errorMessage = ex.Message;
            }

        }

        public Series AddSeries(string serieName, out bool errorOccurred, out int errorCode, out string errorMessage, SeriesChartType charType = SeriesChartType.Line)
        {
            ResetParamsErrors(out errorOccurred, out errorCode, out errorMessage);
            Series s = chart.Series.Add(serieName);
            try
            {
                s.ChartType = charType;
                s.ChartArea = ca.Name;
            }
            catch (Exception ex)
            {
                errorOccurred = true;
                errorCode = ex.HResult;
                errorMessage = ex.Message;
            }
            return s;
        }

        public DataPoint AddPointXY(Series serie, double x, double y, out bool errorOccurred, out int errorCode, out string errorMessage)
        {
            ResetParamsErrors(out errorOccurred, out errorCode, out errorMessage);
            return serie.Points[serie.Points.AddXY(x, y)];
        }

        public DataPoint[] AddPointXY(Series serie, double[] x, double[] y, out bool errorOccurred, out int errorCode, out string errorMessage, double sampleRate = 1)
        {
            ResetParamsErrors(out errorOccurred, out errorCode, out errorMessage);

            DataPoint[] result = null;

            try
            {
                //If y is OK
                if (y != null && y.Length != 0)
                {
                    //If x is KO create it with samplerate
                    if ((x == null) || (x.Length == 0) || x.Length != y.Length)
                    {
                        x = new double[y.Length];
                        for (int i = 0; i < x.Length; i++)
                        {
                            x[i] = i * sampleRate;
                        }
                    }

                    result = new DataPoint[x.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        result[i] = AddPointXY(serie, x[i], y[i], out errorOccurred, out errorCode, out errorMessage);
                    }

                }
            }
            catch (Exception ex)
            {
                errorOccurred = true;
                errorCode = ex.HResult;
                errorMessage = ex.Message;
            }

            return result;
        }

        public void SaveImageAndData(string sPathFileImageData, out bool errorOccurred, out int errorCode, out string errorMessage)
        {
            ResetParamsErrors(out errorOccurred, out errorCode, out errorMessage);

            try
            {
                //Save chart in png file
                chart.SaveImage(sPathFileImageData + ".png", System.Drawing.Imaging.ImageFormat.Png);

                //Save chart in data file
                StreamWriter sw = new StreamWriter(sPathFileImageData + ".txt", false);
                sw.WriteLine("Title " + title.Name);
                foreach (Series s in chart.Series)
                {
                    sw.WriteLine("Serie " + s.Name);
                    foreach (DataPoint p in s.Points)
                    {
                        sw.WriteLine(p.XValue + "\t" + p.YValues[0]);
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                errorOccurred = true;
                errorCode = ex.HResult;
                errorMessage = ex.Message;
            }
        }

        private static void ResetParamsErrors(out bool errorOccurred, out int errorCode, out string errorMessage)
        {
            errorCode = 0;
            errorMessage = "";
            errorOccurred = false;
        }

        public void Dispose()
        {

        }
    }
}
