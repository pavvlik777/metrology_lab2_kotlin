using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Lab
{
    public class Metrics
    {
        List<string> fileCode;
        string FilecodeText;

        public Metrics()
        {
            fileCode = new List<string>() { };
            FilecodeText = "";
        }

        bool multilineComment = false;
        int maxLevel = -1;
        public string FixFileCode(string readPath)
        {
            string filecodeText = "";
            using (StreamReader sr = File.OpenText(readPath))
            {
                string cur;
                while ((cur = sr.ReadLine()) != null)
                {
                    InputTestLine(ref filecodeText, cur);
                }
            }
            CreateFilecodeList(filecodeText);
            FilecodeText = filecodeText;
            RemoveDoWhileOperators(ref filecodeText);
            RemoveOneLineOperators(ref filecodeText);
            //SolveTask(ref filecodeText, -1);
            if (maxLevel == -1) maxLevel = 0;
            return filecodeText;
        }
        
        void RemoveOneLineOperators(ref string filecodeText)
        {

        }

        void RemoveDoWhileOperators(ref string filecodeText)
        {

        }

        public void SolveTask(ref string filecodeText, int level)
        {
            if (level > maxLevel) maxLevel = level;
            int i;
            string closestPattern = ClosestPattern(filecodeText);
            if (closestPattern == "None") return;
            Regex IfPattern = new Regex(closestPattern);
            Match ifMatch = IfPattern.Match(filecodeText);
            if (ifMatch.Success)
            {
                string temp = ifMatch.Value;
                int leftParentess = 1;
                int rightParentess = 0;
                i = ifMatch.Index + temp.Length;
                bool parentessMatch = true;
                while (leftParentess != rightParentess)
                {
                    if (filecodeText[i] == '\r')
                    {
                        parentessMatch = false;
                        break;
                    }
                    if (filecodeText[i] == '(') leftParentess++;
                    if (filecodeText[i] == ')') rightParentess++;
                    i++;
                }

                int ifBlockStart = i;
                int ifBlockEnd = i;
                if (parentessMatch)
                {
                    while (filecodeText[i] == '\r' || filecodeText[i] == '\n' || filecodeText[i] == ' ' || filecodeText[i] == '\t' || filecodeText[i] == '{')
                    {
                        if (filecodeText[i] == '{')
                            break;
                        i++;
                    }
                    i++;
                    ifBlockStart = i;
                    int figureParentIfLeft = 1;
                    int figureParentIfRight = 0;
                    while (figureParentIfLeft != figureParentIfRight)
                    {
                        if (filecodeText[i] == '{') figureParentIfLeft++;
                        if (filecodeText[i] == '}') figureParentIfRight++;
                        i++;
                    }
                    i--;
                    ifBlockEnd = i;
                    string IfLine = filecodeText.Substring(ifBlockStart, ifBlockEnd - ifBlockStart);
                    SolveTask(ref IfLine, level + 1);
                }
                string notIfPart = filecodeText.Substring(ifBlockEnd + 1);
                SolveTask(ref notIfPart, level);
            }


            int a = 5;
        }

        string ClosestPattern(string filecodeText)
        {
            int i = filecodeText.Length + 1;
            string output = "None";
            Regex ifPattern = new Regex(@"\bif(\s)*[(]{1}");
            Match match = ifPattern.Match(filecodeText);
            if (match.Success)
            {
                if(i > match.Index)
                {
                    i = match.Index;
                    output = @"\bif(\s)*[(]{1}";
                }
            }
            Regex forPattern = new Regex(@"\bfor(\s)*[(]{1}");
            match = forPattern.Match(filecodeText);
            if (match.Success)
            {
                if (i > match.Index)
                {
                    i = match.Index;
                    output = @"\bfor(\s)*[(]{1}";
                }
            }
            Regex whilePattern = new Regex(@"\bwhile(\s)*[(]{1}");
            match = whilePattern.Match(filecodeText);
            if (match.Success)
            {
                if (i > match.Index)
                {
                    i = match.Index;
                    output = @"\bwhile(\s)*[(]{1}";
                }
            }
            return output;
        }

        void CreateFilecodeList(string filecodeText)
        {
            int index = filecodeText.IndexOf("\r\n");
            if (index != -1)
            {
                fileCode.Add(filecodeText.Substring(0, index));
                CreateFilecodeList(filecodeText.Substring(index + 2));
            }
            else
            {
                fileCode.Add(filecodeText);
            }
        }

        void InputTestLine(ref string filecodeText, string cur)
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
                InputTestLine(ref filecodeText, line.Substring(0, separatorPos + 1));
                InputTestLine(ref filecodeText, line.Substring(separatorPos + 1));
                return;
            }
            int singlelineComment = line.IndexOf("//");
            if(singlelineComment != -1)
            {
                line = line.Substring(0, singlelineComment);
            }
            line += "\r\n";
            if (!multilineComment)
            {
                int multilineCommentStart = line.IndexOf("/*");
                if (multilineCommentStart != -1)
                {
                    int multilineCommentEnd = line.LastIndexOf("*/");
                    if (multilineCommentEnd != -1 && multilineCommentEnd != multilineCommentStart + 1)
                    {
                        line = line.Remove(multilineCommentStart, multilineCommentEnd - multilineCommentStart + 2);
                    }
                    else if(multilineCommentEnd != multilineCommentStart + 1)
                    {
                        multilineComment = true;
                        line = line.Remove(multilineCommentStart);
                    }
                }
            }
            else
            {
                int endMultilineComment = line.LastIndexOf("*/");
                if (endMultilineComment != -1)
                {
                    line = line.Substring(endMultilineComment + 2);
                    multilineComment = false;
                }
                else return;
            }

            if (!string.IsNullOrWhiteSpace(line))
            {
                filecodeText += line;
            }
        }
    }
}
