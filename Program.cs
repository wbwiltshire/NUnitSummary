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
        public static void Main(string[] args)
        {
            const string fileName = "TestResult.xml";
            XDocument doc;
            XElement testFixture;
            string results = String.Empty;

            Console.WriteLine("NUnit Test Summary");
            try
            {
                doc = XDocument.Load(fileName);
                testFixture = doc.XPathSelectElement("descendant::test-suite[@type='TestFixture']");
                if (testFixture != null)
                {
                    foreach (XElement element in testFixture.DescendantNodes())
                    {
                        Console.Write($"\t{element.Attribute("name").Value} : ");
                        if (element.Attribute("result").Value.ToUpper() == "PASSED")
                            Console.ForegroundColor = ConsoleColor.Green;
                        else
                            Console.ForegroundColor = ConsoleColor.Red;

                        Console.Write($"{element.Attribute("result").Value}");
                        Console.ResetColor();
                        Console.WriteLine($", Duration={element.Attribute("duration").Value}(sec)");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Press <enter> to end.");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exception: {ex.Message}");
            }
            Console.ReadLine();
        }
    }
}
