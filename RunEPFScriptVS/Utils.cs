using System;
using System.IO;
using System.Windows.Forms;

namespace eggIntegration_VS
{
    [Flags]
    enum ExitCodes : int
    {
        Success = 0,
        Failed = 1,
        InvalidArg = 2,
        FileDoesNotExist = 4,
        UnknownError = 32
    };

    public class Utils
    {
        static bool _tracing = true;
        public static string TestOutputDirectory;

        public bool tracing
        {
            set { _tracing = value;}
            get { return _tracing; }
        }

        public static string ProgramName()
        {
            string myFullPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            return System.IO.Path.GetFileNameWithoutExtension(myFullPath);
        }

        public static void Trace(string msg)
        {
            if (_tracing)
                Console.WriteLine(msg);
        }

        public static void ReportError(string message)
        {
            if (_tracing)
            {
                Trace(message);
                MessageBox.Show(message, ProgramName());
            }
        }

        public static testResultType VsResult(string status)
        {
            if (status == "Success") return testResultType.Passed;
            if (status == "Failure") return testResultType.Failed;
            if (status == "Running") return testResultType.InProgress;
            return testResultType.Inconclusive;
        }

        public static string GetTestOutputDirectory()
        {
            TestOutputDirectory = Environment.GetEnvironmentVariable("TestOutputDirectory");
            if (String.IsNullOrEmpty(TestOutputDirectory))
                TestOutputDirectory = Environment.GetEnvironmentVariable("TEMP");
            return TestOutputDirectory;
        }

        public static void CopyePFfilesToTestOutputDirectory(string epFlogFile)
        {
            foreach (string source in Directory.EnumerateFiles(Path.GetDirectoryName(epFlogFile)))
            {
                try
                {
                    string dest;
                    if (source.EndsWith(".failure") || source.EndsWith(".success"))
                        dest = Path.Combine(TestOutputDirectory, "ResultCsv.txt"); // useful to have this as a text file.
                    else if (source.EndsWith("logfile.xml", StringComparison.OrdinalIgnoreCase))
                        continue;
                    else
                        dest = Path.Combine(TestOutputDirectory, Path.GetFileName(source));
                    if (File.Exists(dest))
                        File.Delete(dest);
                    File.Copy(source, dest);
                }
                catch (Exception e)
                {
                    Utils.ReportError(e.Message);
                }
            }
        }

    }
}