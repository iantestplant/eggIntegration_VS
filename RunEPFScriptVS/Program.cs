using System;
using System.Diagnostics;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace eggIntegration_VS
{
    internal class Options
    {
        [Option('h', "host",
            HelpText =
                "The host:port of eggPlant Functional running in drivemode e.g. 192.168.0.99:5400 or localhost:5400")]
        public string host_port { get; set; }

        [Option('t', "suite", Required = true,
            HelpText = "path to eggPlant Functional suite on the host")]
        public string suite { get; set; }

        [Option('s', "script", Required = true,
            HelpText = "the eggPlant script name in the suite")]
        public string script { get; set; }

        [Option('p', "parameters",
            HelpText = "optional parameters to local RunScript.bat command")]
        public string parameters { get; set; }

        [Option('v', "verbose", DefaultValue = false,
            HelpText = "Prints all messages to standard output")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    internal class Program
    {
        private string TestOutputDirectory
        {
            get
            {
                string s = Environment.GetEnvironmentVariable("TestOutputDirectory");
                if (String.IsNullOrEmpty(s))
                    s = Environment.GetEnvironmentVariable("TEMP");
                return s;
            }
        }

        [STAThread]
        private static int Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options) == false)
            {
                Utils.ReportError(options.GetUsage());
                return (int) ExitCodes.InvalidArg;
            }
            return String.IsNullOrEmpty(options.host_port) ? new RunScriptVS().Execute(options) : new DriveScript().Execute(options);

        }
    }
}