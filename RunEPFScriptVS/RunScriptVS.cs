using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace eggIntegration_VS
{
    // Run a script from the command line using RunScript.bat
    class RunScriptVS
    {
        string _ePFlogFile = "";
        private const string _GlobalResultsFolder = "Results"; // May need to be parameterised
        string _resultFolder = "";
        string _runHistory = "";
        DateTime _lastRunHistoryTime;
        bool _tracing = true;
        string _parameters = "";

        private string TestOutputDirectory
        {
            get
            {
                string s = Environment.GetEnvironmentVariable("TestOutputDirectory"); // This 
                if (String.IsNullOrEmpty(s))
                    s = Environment.GetEnvironmentVariable("TEMP");
                return s;
            }
        }

        public int Execute(Options options)
        {
            _tracing = options.Verbose;

            Utils.Trace("TestOutputDirectory=" + Environment.GetEnvironmentVariable("TestOutputDirectory"));

            string suite = options.suite;
            string script = options.script;
            //_parameters = options.p

            if (!string.IsNullOrEmpty(suite))
            {
                if (!Path.IsPathRooted(script))
                {
                    script = Path.Combine(suite, "Scripts", script);
                }
            }

            if (!script.EndsWith(".script", StringComparison.OrdinalIgnoreCase))
            {
                script += ".script";
            }

            var fi = new FileInfo(script);

            if (String.IsNullOrEmpty(_ePFlogFile))
            {
                var runScriptPath = GetRunScriptPath(); // return full path to runscript.bat
                if (runScriptPath == string.Empty)
                {
                    return (int)ExitCodes.FileDoesNotExist;
                }

                if (fi.Exists == false)
                {
                    Utils.ReportError(String.Format("The file {0} does not exist", script));
                    return (int)ExitCodes.FileDoesNotExist;
                }

                if (!GetRunHistoryFilePath(fi, false)) // get the ePF RunHistory path (if it exists) and record _lastRunHistoryTime
                    _lastRunHistoryTime = DateTime.Now; // RunHistory does not exist (first time) get current time

                ExecuteSript(runScriptPath, fi.FullName);
            }

            // Run returned success, find logFile
            string logFile = GetLogFilePath(fi); // Also checks if RunHistory has been updated since _lastRunHistoryTime
            if (String.IsNullOrEmpty(logFile))
                return (int)ExitCodes.FileDoesNotExist;

            // Transpose logFile.xml to Visual Studio Summary.xml format
            TransposeLogfile(logFile);

            return (int)ExitCodes.Success;
        }

        /// <summary>
        ///  Execute RunScript.bat for the script
        /// </summary>
        /// <param name="runScriptPath"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        private void ExecuteSript(string runScriptPath, string script)
        {
            // Failed to get cmd.exe to work with quotes around both runpath and scriptpath which is needed if both paths can contain spaces.
            // i.e. ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe", String.Format("/C \"{0}\" {1} -GlobalResultsFolder Results", runScriptPath, script));
            // This is a common problem as /C only allow for one pair of quotes!!
            // Instead changed to use runpath.bat as the executable - works on Win 8.1


            var startInfo = new ProcessStartInfo(runScriptPath, String.Format("\"{0}\" -GlobalResultsFolder {1} {2}", script, _GlobalResultsFolder, this._parameters));
            Utils.Trace(startInfo.FileName + " " + startInfo.Arguments);
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            try
            {
                // Call WaitForExit and then the using statement will close.
                using (var process = Process.Start(startInfo))
                {
                    if (process == null) return;  // an exception should have been raised.
                    process.WaitForExit();
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();
                    int exitCode = process.ExitCode;

                    if (!String.IsNullOrEmpty(stdout)) Utils.Trace(stdout);
                    if (!String.IsNullOrEmpty(stderr)) Utils.Trace(stderr);
                    Utils.Trace("RunScript.bat ExitCode: " + exitCode);

                    //process.Close();
                    if (exitCode > 20)
                    {
                        Utils.ReportError(String.Format("Failed to execute the script\n {0}\n{1}", stdout, stderr));
                    }
                }
            }
            catch (Exception e)
            {
                Utils.ReportError(String.Format("Exception: Failed to execute {0} {1}\nError: {2}", runScriptPath, startInfo.Arguments, e.Message));
            }
        }

        /// <summary>
        /// return the full path to runScript.bat
        /// </summary>
        /// <returns></returns>
        public string GetRunScriptPath()
        {
            string runscriptPath;
            var folders = new List<string>
            {
                Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),
                Environment.CurrentDirectory,
                Environment.GetEnvironmentVariable("ProgramFiles"),
                Environment.GetEnvironmentVariable("ProgramFiles(x86)")
            };
            foreach (string f in folders.Where(f => !String.IsNullOrEmpty(f)))
            {
                runscriptPath = Path.Combine(f, "eggPlant\\runscript.bat");
                if (File.Exists(runscriptPath))
                    return runscriptPath;
            }
            var epf = Environment.GetEnvironmentVariable("EGGPLANT");
            if (!String.IsNullOrEmpty(epf))
            {
                runscriptPath = Path.Combine(epf, "runscript.bat");
                if (File.Exists(runscriptPath))
                    return runscriptPath;
            }

            Utils.ReportError(String.Format("Failed to find runscript.bat in:\n\n" +
            "%ProgramFiles%\\eggPlant, %ProgramFiles (x86)%\\eggPlant, %EGGPLANT%"
            ));

            return string.Empty;
        }

        /// <summary>
        /// Return the RunHistory path for the script run logfile.xml under the "Results" folde
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="reportError"></param>
        /// <returns></returns>

        bool GetRunHistoryFilePath(FileInfo fi, bool reportError)
        {
            var d = fi.Directory;
            while (d.Parent != null && d.Parent.Extension.Equals(".suite", StringComparison.OrdinalIgnoreCase) == false)
                d = d.Parent;
            if (d.Parent == null)
            {
                Utils.ReportError("Failed to find suite folder in " + fi.FullName);
                return false;
            }
            _resultFolder = Path.Combine(d.Parent.FullName, _GlobalResultsFolder);
            _resultFolder = Path.Combine(_resultFolder, fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length));
            _runHistory = Path.Combine(_resultFolder, "RunHistory.xml");
            if (!File.Exists(_runHistory))
            {
                if (reportError)
                    Utils.ReportError("Failed to find RunHistory.xml folder in " + _resultFolder);
                _runHistory = String.Empty;
                return false;
            }
            if (reportError == false) // first time round, get the file time
                _lastRunHistoryTime = File.GetLastWriteTime(_runHistory);
            return true;
        }


        /// <summary> Return the path to the script run logfile.xml under the "Results" folder
        /// </summary>
        /// <param name="scriptPath"></param>
        /// <returns></returns>
        string GetLogFilePath(FileInfo scriptPath)
        {
            string logFilePath;
            if (String.IsNullOrEmpty(_runHistory) && GetRunHistoryFilePath(scriptPath, true) == false)
            {
                return null;  // Failed to find RunHistory path
            }
            DateTime updated = File.GetLastWriteTime(_runHistory);
            if (updated < _lastRunHistoryTime)
            {
                Utils.ReportError(String.Format("The RunHistory.xml has not been updated in {0}\nThe run results cannot be read", _resultFolder));
                return null;
            }

            // Extract final entry from RunHistory
            var text = File.ReadAllText(_runHistory);
            int pos1 = text.LastIndexOf("<LogFile>", StringComparison.Ordinal);
            int pos2 = text.LastIndexOf(".txt</LogFile>", StringComparison.Ordinal);
            logFilePath = Path.Combine(_resultFolder, text.Substring(pos1 + 9, (pos2 - (pos1 + 9)))) + ".xml";
            logFilePath = Path.GetFullPath(logFilePath);
            if (!File.Exists(logFilePath))
            {
                Utils.ReportError("Failed to find " + logFilePath);
                return "";
            }

            return logFilePath;
        }

        /// <summary>
        /// Transpose ePF Logfile.xml to Visual Studio ResultSummary.xml
        /// </summary>
        /// <param name="logFilePath"></param>
        public void TransposeLogfile(string logFilePath)
        {
            var xelement = XElement.Load(logFilePath);
            var testsuites = xelement.Elements();
            foreach (var testsuite in testsuites)
            {
                var vsResult = new SummaryResult();

                //if (_tracing)
                //{
                //    Console.WriteLine(testsuite.Attribute("name"));
                //    Console.WriteLine(testsuite.Attribute("package"));
                //    Console.WriteLine(testsuite.Attribute("timestamp"));
                //    Console.WriteLine(testsuite.Attribute("tests"));
                //    Console.WriteLine(testsuite.Attribute("errors"));
                //}

                vsResult.TestName = testsuite.Attribute("name").Value;
                vsResult.DetailedResultsFile = logFilePath;
                var testProperties = testsuite.Elements("properties");

                foreach (var propSet in testProperties)
                {
                    var properties = propSet.Elements("property");

                    foreach (var prop in properties)
                    {
                        //if (_tracing)
                        //    Console.WriteLine("propname={0} value={1}", prop.Attribute("name").Value, prop.Attribute("value").Value);
                        if (prop.Attribute("name").Value == "Status")
                            vsResult.TestResult = Utils.VsResult(prop.Attribute("value").Value);
                    }
                }
                /*
                var testcases = testsuite.Elements("testcase");
                var innerTestsL = new List<SummaryResultInnerTest>();
                foreach (var testcase in testcases)
                {
                    var innerTest = new SummaryResultInnerTest();

                    //if (_tracing)
                    //    Console.WriteLine("case={0} assertions={1} successes={2} errors={3} time={4}", testcase.Attribute("name").Value, testcase.Attribute("assertions").Value,
                    //        testcase.Attribute("successes").Value, testcase.Attribute("errors").Value, testcase.Attribute("time").Value);
                    innerTest.TestName = testcase.Attribute("name").Value;
                    // Use zero errors to indicate inner test has passed
                    innerTest.TestResult = int.Parse(testcase.Attribute("errors").Value) == 0 ? testResultType.Passed : testResultType.Failed;
                    innerTestsL.Add(innerTest);

                }
                vsResult.InnerTests = innerTestsL.ToArray();
                */

                string testOutputDirectory = Utils.GetTestOutputDirectory();
                Utils.CopyePFfilesToTestOutputDirectory(logFilePath);

                var summary = Path.Combine(testOutputDirectory, "Summary.xml");
                using (var writer = new StreamWriter(summary))
                {
                    var serializer = new XmlSerializer(vsResult.GetType());
                    serializer.Serialize(writer, vsResult);
                    writer.Flush();
                }
                //if (_tracing)
                //    Console.WriteLine("Coverted {0} to {1}", logFilePath, summary);
            }
        }
    }
}
