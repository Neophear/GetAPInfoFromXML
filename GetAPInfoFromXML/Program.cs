using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GetAPInfoFromXML
{
    class Program
    {
        static void Main(string[] args)
        {
            List<AP> list = new List<AP>();
            Regex findAP = new Regex(@"<AP>.*?</AP>", RegexOptions.Singleline);
            Regex findSerialNumber = new Regex(@"<SerialNumber>(\w*)</SerialNumber>");
            Regex findSystemName = new Regex(@"<SystemName>(.*)</SystemName>");

            try
            {
                string text = File.ReadAllText("ConfiguredAPs.xml");
                foreach (Match m in findAP.Matches(text))
                {
                    string serial = findSerialNumber.Match(m.Value).Groups[1].Value;
                    string name = findSystemName.Match(m.Value).Groups[1].Value;

                    list.Add(new AP { SerialNumber = serial, SystemName = name });
                }

                string result = "";

                foreach (AP ap in list)
                    result += ap + "\n";

                File.WriteAllText("ConfiguredAPs.csv", result);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file ConfiguredAPs.xml could not be found!");
                Console.ReadKey();
            }
        }
    }

    class AP
    {
        public string SerialNumber { get; set; }
        public string SystemName { get; set; }

        public override string ToString()
        {
            return String.Format("{0};{1}", SystemName, SerialNumber);
        }
    }
}
