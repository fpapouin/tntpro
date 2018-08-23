using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunitReport
{
    enum TestStatus
    {
        passed,
        failed,
        error
    }

    class TestSuite
    {
        public string Name { get; set; }
        public List<TestCase> Cases { get; set; } = new List<TestCase>();
    }

    class TestCase
    {
        public string Name { get; set; }
        public List<TestResult> Results { get; set; } = new List<TestResult>();
    }

    class TestResult
    {
        public string Name { get; set; }
        public TestStatus Status { get; set; }
        public string FailureMessage { get; set; }
    }

    class Program
    {
        static List<TestSuite> Suites = new List<TestSuite>();
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            if (args.Length >= 1)
            {
                if (Directory.Exists(args[0]))
                {
                    path = args[0];
                }
            }

            string output = Directory.GetCurrentDirectory();
            if (args.Length >= 2)
            {
                if (Directory.Exists(args[1]))
                {
                    output = args[1];
                }
            }

            DirectoryInfo cdi = new DirectoryInfo(path);

            FileInfo[] files = cdi.GetFiles("*.png", SearchOption.AllDirectories);

            foreach (FileInfo fi in files)
            {
                if (fi.Directory.Name == "vrt_references")
                {
                    TestSuite ts = Suites.FirstOrDefault(s => s.Name == fi.Directory.Parent.Name);
                    if (ts == null)
                    {
                        ts = new TestSuite();
                        ts.Name = fi.Directory.Parent.Name;
                        Suites.Add(ts);
                    }

                    TestCase tc = ts.Cases.FirstOrDefault(c => c.Name == fi.Directory.Name);
                    if (tc == null)
                    {
                        tc = new TestCase();
                        tc.Name = fi.Directory.Name;
                        ts.Cases.Add(tc);
                    }

                    TestResult tr = tc.Results.FirstOrDefault(r => r.Name == fi.Name);
                    if (tr == null)
                    {
                        tr = new TestResult();
                        tr.Name = fi.Name;
                        tc.Results.Add(tr);
                    }
                }
            }

            foreach (TestSuite ts in Suites)
            {
                updateResultStatus(ts, path);
                Console.WriteLine("Writing junit Report");
                File.WriteAllText(Path.Combine(output, ts.Name + ".xml"), writeReport(ts));
                Console.WriteLine(Path.Combine(output, ts.Name + ".xml"));
            }
        }

        private static void updateResultStatus(TestSuite ts, string path)
        {
            foreach (TestCase tc in ts.Cases)
            {
                foreach (TestResult tr in tc.Results)
                {
                    if (!File.Exists(Path.Combine(path, ts.Name, "vrt_references", tr.Name)))
                    {
                        tr.Status = TestStatus.error;
                        tr.FailureMessage = Path.Combine(ts.Name, "vrt_references", tr.Name) + Environment.NewLine + "does not exist";
                    }
                    else if (!File.Exists(Path.Combine(path, ts.Name, "vrt_results", tr.Name)))
                    {
                        tr.Status = TestStatus.error;
                        tr.FailureMessage = Path.Combine(ts.Name, "vrt_results", tr.Name) + Environment.NewLine + "does not exist";
                    }
                    else if (File.Exists(Path.Combine(path, ts.Name, "vrt_diffs", tr.Name)))
                    {
                        tr.Status = TestStatus.failed;
                        tr.FailureMessage = Path.Combine(ts.Name, "vrt_diffs", tr.Name) + Environment.NewLine + "diff detected";
                    }
                    else
                    {
                        tr.Status = TestStatus.passed;
                    }
                }
            }
        }

        private static string writeReport(TestSuite ts)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            sb.Append(Environment.NewLine);

            sb.Append("<!--Voclano VRT results in JUnit format-->");
            sb.Append(Environment.NewLine);

            sb.Append("<testsuites>");
            sb.Append(Environment.NewLine);

            int tests = ts.Cases[0].Results.Count;
            int errors = ts.Cases[0].Results.Count(r => r.Status == TestStatus.error);
            int failures = ts.Cases[0].Results.Count(r => r.Status == TestStatus.failed);

            sb.Append(string.Format("  <testsuite name={0} tests={1} errors={2} failures={3}>", toLiteral(ts.Name), toLiteral(tests), toLiteral(errors), toLiteral(failures)));
            sb.Append(Environment.NewLine);

            foreach (TestResult tr in ts.Cases[0].Results)
            {
                sb.Append(string.Format("    <testcase name={0}>", toLiteral(tr.Name)));
                sb.Append(Environment.NewLine);

                if (tr.Status != TestStatus.passed)
                {
                    sb.Append("      <failure>");
                    sb.Append(Environment.NewLine);
                    sb.Append(tr.FailureMessage);
                    sb.Append(Environment.NewLine);
                    sb.Append("      </failure>");
                    sb.Append(Environment.NewLine);
                }

                sb.Append(string.Format("    </testcase>", toLiteral(tr.Name)));
                sb.Append(Environment.NewLine);
            }

            sb.Append("  </testsuite>");
            sb.Append(Environment.NewLine);
            sb.Append("</testsuites>");

            return sb.ToString();
        }

        private static string toLiteral(string input)
        {
            return "\"" + input + "\"";
        }

        private static string toLiteral(int input)
        {
            return "\"" + input + "\"";
        }
    }
}
