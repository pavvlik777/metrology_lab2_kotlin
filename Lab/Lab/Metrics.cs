using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab
{
    public class Metrics
    {
        List<string> fileCode;

        public Metrics()
        {
            fileCode = new List<string>() { };
        }

        public string FixFileCode(string readPath)
        {
            string test = "";
            using (StreamReader sr = File.OpenText(readPath))
            {
                string cur;
                while ((cur = sr.ReadLine()) != null)
                {
                    InputTestLine(ref test, cur);
                }
            }
            return test;
        }

        void InputTestLine(ref string test, string cur)
        {
            string line = cur;
            while (line.Length != 0)
            {
                if (line[0] == ' ' || line[0] == '\t')
                    line = line.Remove(0, 1);
                else
                    break;
            }

            int separatorPos = line.IndexOf(';');
            if (separatorPos != -1 && separatorPos != line.Length - 1)
            {
                InputTestLine(ref test, line.Substring(0, separatorPos + 1));
                InputTestLine(ref test, line.Substring(separatorPos + 1));
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                fileCode.Add(line);
                test += $"{line}\r\n";
            }
        }
    }
}
