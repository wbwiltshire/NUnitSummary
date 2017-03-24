using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NUnitSummary
{
    public class Program
    {
        const string Version = "0.9.1";
        public static void Main(string[] args)
        {
            XDocument doc;
            IEnumerable<XElement> testCases;
            string results = String.Empty;
            CommandLineParameters clp = new CommandLineParameters() { Arguments = 0, FileName = String.Empty, Version = false };

            try
            {
                if (ProcessCommandLine(args, clp))
                {
                    Console.WriteLine("NUnit Test Summary");
                    doc = XDocument.Load(clp.FileName);
                    testCases = doc.XPathSelectElements("descendant::test-case");
                    if (testCases != null)
                    {
                        foreach (XElement testCase in testCases)
                        {
                            Console.Write($"\t{testCase.Attribute("name").Value} : ");
                            if (testCase.Attribute("result").Value.ToUpper() == "PASSED")
                                Console.ForegroundColor = ConsoleColor.Green;
                            else
                                Console.ForegroundColor = ConsoleColor.Red;

                            Console.Write($"{testCase.Attribute("result").Value}");
                            Console.ResetColor();
                            Console.WriteLine($", Duration={testCase.Attribute("duration").Value}(sec)");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private static bool ProcessCommandLine(string[] args, CommandLineParameters clp)
        {
            bool fileExists = false;

            //Capture number of arguments
            clp.Arguments = args.Count();

            foreach (string arg in args)
            {
                if (arg.ToUpper().StartsWith("-V"))
                    clp.Version = true;
                else
                {
                    clp.FileName = arg;
                    if (File.Exists(arg))
                    {
                        fileExists = true;
                    }
                }
            }

            //If no filename was provided, but version was requested then print version
            //Else, If a filename was provided, but doesn't exist then print usage
            //Otherwise (i.e. a valid filename was provided), skip printing usage or version and just process the filename provided
            if (clp.Version  && (clp.FileName == String.Empty))
                Console.WriteLine($"NUnitSummary v.{Version}");
            else if (!fileExists)
                    Console.WriteLine("Usage: NUnitSummary filename.xml <-v>");

            return fileExists;
        }
    }
}
