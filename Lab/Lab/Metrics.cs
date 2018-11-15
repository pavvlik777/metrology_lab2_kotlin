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
            while (RemoveElseFromIfOperators(ref filecodeText)) ;
            while (RemoveDoWhileOperators(ref filecodeText));
            RemoveOneLineOperators(ref filecodeText);
            SolveTask(ref filecodeText, -1);
            if (maxLevel == -1) maxLevel = 0;
            return filecodeText;
        }
        
        void RemoveOneLineOperators(ref string filecodeText)
        {

        }

        bool RemoveElseFromIfOperators(ref string filecodeText)
        {
            Regex elsePattern = new Regex(@"\belse(\s)+[{]{1}");
            Match elseMatch = elsePattern.Match(filecodeText);
            int i;
            if (elseMatch.Success)
            {
                string temp = elseMatch.Value;
                filecodeText = filecodeText.Remove(elseMatch.Index, temp.Length);
                filecodeText = filecodeText.Insert(elseMatch.Index, "if( )\r\n{");
            }
            return elseMatch.Success;
        }

        bool RemoveDoWhileOperators(ref string filecodeText)
        {
            Regex doPattern = new Regex(@"\bdo(\s)*[{]{1}");
            Match doMatch = doPattern.Match(filecodeText);
            int i;
            if(doMatch.Success)
            {
                string temp1 = doMatch.Value;
                int doLeftParentess = 1;
                int doRightParentess = 0;
                i = doMatch.Index + temp1.Length;
                while (doLeftParentess != doRightParentess)
                {
                    if (filecodeText[i] == '{') doLeftParentess++;
                    if (filecodeText[i] == '}') doRightParentess++;
                    i++;
                }
                int doBlockStart = i;

                Regex whilePattern = new Regex(@"\bwhile(\s)*[(]{1}");
                Match whileMatch = whilePattern.Match(filecodeText, doBlockStart);
                string temp2 = whileMatch.Value;
                int whileLeftParentess = 1;
                int whileRightParentess = 0;
                i = whileMatch.Index + temp2.Length;
                while (whileLeftParentess != whileRightParentess)
                {
                    if (filecodeText[i] == '(') whileLeftParentess++;
                    if (filecodeText[i] == ')') whileRightParentess++;
                    i++;
                }
                string whilePart = filecodeText.Substring(whileMatch.Index, i - whileMatch.Index);
                whilePart = whilePart.Remove(0, 1);
                whilePart = whilePart.Insert(0, "W");
                filecodeText = filecodeText.Remove(whileMatch.Index, i - whileMatch.Index);
                filecodeText = filecodeText.Insert(doMatch.Index + 2, whilePart);
            }
            return doMatch.Success;
        }

        public void SolveTask(ref string filecodeText, int level)
        {
            if (level > maxLevel) maxLevel = level;
            int i;
            string closestPattern = ClosestPattern(filecodeText);
            if (closestPattern == "None") return;
            if(closestPattern == @"\bwhen(\s)*[(]{1}")
            {
                SolveTaskForSwitch(ref filecodeText, level);
                return;
            }
            //if (closestPattern == @"\bif(\s)*[(]{1}")
            //{
            //    SolveTaskForIf(ref filecodeText, level);
            //    return;
            //}
            Regex IfPattern = new Regex(closestPattern);
            Match ifMatch = IfPattern.Match(filecodeText);

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

        void SolveTaskForIf(ref string filecodeText, int level)
        {
            Regex ifPattern = new Regex(@"\bif(\s)*[(]{1}");
            Match ifMatch = ifPattern.Match(filecodeText);
            int i;

            string temp = ifMatch.Value;
            i = ifMatch.Index + temp.Length;
            while (filecodeText[i] != '{') i++;
            i++;

            int ifBlockStart = i;
            int figureParentIfLeft = 1;
            int figureParentIfRight = 0;
            while (figureParentIfLeft != figureParentIfRight)
            {
                if (filecodeText[i] == '{') figureParentIfLeft++;
                if (filecodeText[i] == '}') figureParentIfRight++;
                i++;
            }
            int ifBlockEnd = i - 2;
            string ifBlock = filecodeText.Substring(ifBlockStart, ifBlockEnd - ifBlockStart - 1);
            SolveTask(ref ifBlock, level + 1);

            string notIfPart = filecodeText.Substring(i);
            SolveTask(ref notIfPart, level);
        }

        void SolveTaskForSwitch(ref string filecodeText, int level)
        {
            Regex whenPattern = new Regex(@"\bwhen(\s)*[(]{1}");
            Match whenMatch = whenPattern.Match(filecodeText);
            int i;

            string temp = whenMatch.Value;
            i = whenMatch.Index + temp.Length;
            while (filecodeText[i] != '{') i++;
            i++;

            int whenBlockStart = i;
            int figureParentWhenLeft = 1;
            int figureParentWhenRight = 0;
            while (figureParentWhenLeft != figureParentWhenRight)
            {
                if (filecodeText[i] == '{') figureParentWhenLeft++;
                if (filecodeText[i] == '}') figureParentWhenRight++;
                i++;
            }
            int whenBlockEnd = i - 2;

            Regex casePattern = new Regex(@"->(\s)*[{]{1}");
            string whenBlock = filecodeText.Substring(whenBlockStart,whenBlockEnd - whenBlockStart - 1);
            MatchCollection matches = casePattern.Matches(whenBlock);
            int j = 1;
            int amountOfCases = matches.Count;
            Regex elseCase = new Regex(@"\belse(\s)+->(\s)*[{]{1}");
            if (elseCase.Match(whenBlock).Success) amountOfCases--;//последний else не считается
            foreach (Match cur in matches) 
            {
                string caseTemp = cur.Value;
                i = cur.Index + caseTemp.Length;
                int caseBlockStart = i;
                int figureParentCaseLeft = 1;
                int figureParentCaseRight = 0;
                while (figureParentCaseLeft != figureParentCaseRight)
                {
                    if (whenBlock[i] == '{') figureParentCaseLeft++;
                    if (whenBlock[i] == '}') figureParentCaseRight++;
                    i++;
                }
                int caseBlockEnd = i - 2;
                string casePart = whenBlock.Substring(caseBlockStart, caseBlockEnd - caseBlockStart);
                SolveTask(ref casePart, level + j);
                if(j + 1 <= amountOfCases) j++;
            }

            string notWhenPart = filecodeText.Substring(whenBlockEnd + 2);
            SolveTask(ref notWhenPart, level);
        }

        string ClosestPattern(string filecodeText)
        {
            int i = filecodeText.Length + 1;
            string output = "None";
            string[] patterns = { @"\bif(\s)*[(]{1}", @"\bfor(\s)*[(]{1}", @"\bwhile(\s)*[(]{1}", @"\bdoWhile(\s)*[(]{1}", @"\bwhen(\s)*[(]{1}" };
            for(int j = 0; j < patterns.Length; j++)
            {
                Regex pattern = new Regex(patterns[j]);
                Match match = pattern.Match(filecodeText);
                if (match.Success)
                {
                    if (i > match.Index)
                    {
                        i = match.Index;
                        output = patterns[j];
                    }
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
