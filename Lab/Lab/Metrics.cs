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
            SolveTask(filecodeText);
            return filecodeText;
        }

        public void SolveTask(string filecodeText)
        {
            string temp;
            int i;
            //if( )
            Regex ifPattern = new Regex(@"\bif(\s)*[(]{1}");
            Match ifMatch = ifPattern.Match(filecodeText);
            if (ifMatch.Success)
            {
                temp = ifMatch.Value;
                int leftParentess = 1;
                int rightParentess = 0;
                i = ifMatch.Index + temp.Length;
                bool parentessMatch = true;
                while(leftParentess != rightParentess)
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

                if (parentessMatch)
                {
                    int findStart = i;
                    bool figureParentessIfStart = false;
                    while(filecodeText[i] == '\r' || filecodeText[i] == '\n' || filecodeText[i] == ' ' || filecodeText[i] == '\t' || filecodeText[i] == '{')
                    {
                        if (filecodeText[i] == '{')
                            figureParentessIfStart = true;
                        i++;
                    }
                    //if( ) { }
                    string IfLine = "";
                    int IfStart = i;
                    if (figureParentessIfStart)
                    {
                        int figureParentIfLeft = 1;
                        int figureParentIfRight = 0;
                        while(figureParentIfLeft != figureParentIfRight)
                        {
                            if (filecodeText[i] == '{') figureParentIfLeft++;
                            if (filecodeText[i] == '}') figureParentIfRight++;
                            i++;
                        }
                        i--;
                        IfLine = filecodeText.Substring(IfStart, i - IfStart);
                    }
                    //if( )
                    else
                    {
                        while (filecodeText[i] != '\r')
                            i++;
                        i += 2;
                        IfLine = filecodeText.Substring(IfStart, i - IfStart);
                    }
                }
                //else
                int findStartElse = i;
                Regex elsePattern = new Regex(@"\belse\b");
                Match elseMatch = elsePattern.Match(filecodeText, findStartElse);
                if (elseMatch.Success)
                {
                    temp = elseMatch.Value;
                    i = elseMatch.Index + temp.Length;
                    bool figureParentessElseStart = false;
                    while (filecodeText[i] == '\r' || filecodeText[i] == '\n' || filecodeText[i] == ' ' || filecodeText[i] == '\t' || filecodeText[i] == '{')
                    {
                        if (filecodeText[i] == '{')
                            figureParentessElseStart = true;
                        i++;
                    }
                    //else { }
                    string ElseLine = "";
                    int ElseStart = i;
                    if (figureParentessElseStart)
                    {
                        int figureParenElseLeft = 1;
                        int figureParentElseRight = 0;
                        while (figureParenElseLeft != figureParentElseRight)
                        {
                            if (filecodeText[i] == '{') figureParenElseLeft++;
                            if (filecodeText[i] == '}') figureParentElseRight++;
                            i++;
                        }
                        i--;
                        ElseLine = filecodeText.Substring(ElseStart, i - ElseStart);
                    }
                    //else
                    else
                    {
                        while (filecodeText[i] != '\r')
                            i++;
                        i += 2;
                        ElseLine = filecodeText.Substring(ElseStart, i - ElseStart);
                    }
                }
            }
            int a = 5;
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
