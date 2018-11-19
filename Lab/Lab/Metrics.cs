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
        public Metrics()
        {

        }

        bool multilineComment = false;
        int maxLevel = -1;
        int absolutDifficulty = 0;
        int allOperators = 0;
        public int FindMaxNestingLevel(string readPath)
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
            filecodeText += "\r\n";
            ReplaceWhenElse(ref filecodeText, true);

            RemoveOneLineOperators(ref filecodeText);
            //ReplaceWhenElse(ref filecodeText, false);

            while (RemoveElseFromIfOperators(ref filecodeText)) ;
            while (RemoveDoWhileOperators(ref filecodeText));
            SolveTask(ref filecodeText, -1);
            if (maxLevel == -1) maxLevel = 0;
            return maxLevel;
        }

        bool needToCountForWhiles = false;
        public int FindAbsolutDiff(string readPath, bool countForWhiles)
        {
            needToCountForWhiles = countForWhiles;
            string filecodeText = "";
            using (StreamReader sr = File.OpenText(readPath))
            {
                string cur;
                while ((cur = sr.ReadLine()) != null)
                {
                    InputTestLine(ref filecodeText, cur);
                }
            }
            filecodeText += "\r\n";
            ReplaceWhenElse(ref filecodeText, true);

            RemoveOneLineOperators(ref filecodeText);
            //ReplaceWhenElse(ref filecodeText, false);

            while (RemoveElseFromIfOperators(ref filecodeText)) ;
            while (RemoveDoWhileOperators(ref filecodeText)) ;
            SolveTask(ref filecodeText, -1);
            return absolutDifficulty;
        }

        public double FindRelativeDiff(string readPath, bool countForWhiles)
        {
            needToCountForWhiles = countForWhiles;
            string filecodeText = "";
            using (StreamReader sr = File.OpenText(readPath))
            {
                string cur;
                while ((cur = sr.ReadLine()) != null)
                {
                    InputTestLine(ref filecodeText, cur);
                }
            }
            filecodeText += "\r\n";
            ReplaceWhenElse(ref filecodeText, true);

            RemoveOneLineOperators(ref filecodeText);
            //ReplaceWhenElse(ref filecodeText, false);

            while (RemoveElseFromIfOperators(ref filecodeText)) ;
            while (RemoveDoWhileOperators(ref filecodeText)) ;
            SolveTask(ref filecodeText, -1);
            CountOperators(ref filecodeText);
            return absolutDifficulty/(double)allOperators;
        }

        void CountOperators(ref string filecodeText)
        {
            string[] operators = { @"\bas\b", @"\bas[?]{1}\b", @"\bbreak\b", @"\bclass\b", @"\bcontinue\b", @"\bfun\b", @"\bin\b", @"\b[!]{1}in\b", @"\binterface\b", @"\bis\b", @"\b[!]{1}is\b",
                @"\breturn\b", @"\bvar\b", @"\bval\b",
            @"([\+]{1}[=]{1})|([-]{1}[=]{1})|([\*]{1}[=]{1})|([/]{1}[=]{1})|([%]{1}[=]{1})",
            @"(===)|(!==)",
            @"(&&)|([\|]{2})",
            @"(==)|(!=)|(>=)|(<=)|(>)|(<)",
            @"([.]{2})",
            @"([\:]{1})",
            @"([\+]{2})|([-]{2})",
            @"[.]{1}",
            @"([\+]{1})|([-]{1})|([\*]{1})|([/]{1})|([%]{1)}|([=]{1})|([!]{1})",
            @"[;]{1}" };
            for(int i = 0; i < operators.Length; i++)
            {
                Regex pattern = new Regex(operators[i]);
                allOperators += pattern.Matches(filecodeText).Count;
                filecodeText = pattern.Replace(filecodeText, "");
            }
        }

        void ReplaceWhenElse(ref string filecodeText, bool mode)
        {
            if(mode)
            {
                Regex pattern = new Regex(@"\belse(\s)+->");
                filecodeText = pattern.Replace(filecodeText, "felse ->");
                pattern = new Regex(@"}(\s)+else(\s)+{");
                filecodeText = pattern.Replace(filecodeText, "☼");
                pattern = new Regex(@"}(\s)+else(\s)+");
                filecodeText = pattern.Replace(filecodeText, "☼");
                pattern = new Regex(@"(\s)+else(\s)+");
                filecodeText = pattern.Replace(filecodeText, "☼");
            }
            else
            {
                Regex pattern = new Regex(@"\bfelse(\s)+->");
                filecodeText = pattern.Replace(filecodeText, "else ->");
                pattern = new Regex(@"☼");
                filecodeText = pattern.Replace(filecodeText, "\r\n}\r\nelse\r\n{\r\n");
            }
        }

        void FixOneLineOperator(ref string line)
        {
            line = line.Insert(0, "{\r\n");
            line = line.Insert(line.Length, "}\r\n");
        }

        (bool, int) IsThisOperatorOneLine(ref string line, int operatorEnd)
        {
            int i = operatorEnd;
            while(line[i] == ' ' || line[i] == '\t' || line[i] == '\r' || line[i] == '\n' || line[i] == '{')
            {
                if (line[i] == '{')
                {
                    return (false, i);
                }
                i++;
            }
            return (true , i);
        }

        (string, bool) GetClosestPatternForOneLine(string filecodeText)
        {
            string output = "None";
            bool IsWithParentess = false;
            int i = filecodeText.Length;
            string[] patterns1 = { @"\bif(\s)*[(]{1}", @"\bfor(\s)*[(]{1}", @"\bwhile(\s)*[(]{1}", @"\bwhen(\s)*[(]{1}" };
            string[] patterns2 = { @"->", @"\bdo\b", @"\belse\b"};
            for (int j = 0; j < patterns1.Length; j++)
            {
                Regex pattern = new Regex(patterns1[j]);
                Match match = pattern.Match(filecodeText);
                if (match.Success)
                {
                    if (i > match.Index)
                    {
                        i = match.Index;
                        output = patterns1[j];
                        IsWithParentess = true;
                    }
                }
            }
            for (int j = 0; j < patterns2.Length; j++)
            {
                Regex pattern = new Regex(patterns2[j]);
                Match match = pattern.Match(filecodeText);
                if (match.Success)
                {
                    if (i > match.Index)
                    {
                        i = match.Index;
                        output = patterns2[j];
                        IsWithParentess = false;
                    }
                }
            }
            return (output, IsWithParentess);
        }
        
        void RemoveOneLineOperators(ref string filecodeText) //if() else ситуацию рассмотреть отдельно
        {
            (string pattern, bool isNeedParentess) data = GetClosestPatternForOneLine(filecodeText);
            Regex pattern = new Regex(data.pattern);
            Match cur = pattern.Match(filecodeText);

            if(cur.Success)
            {
                string temp = cur.Value;
                int i = cur.Index + temp.Length;
                if(data.isNeedParentess)
                {
                    int leftParentess = 1;
                    int rightParentess = 0;
                    while (leftParentess != rightParentess)
                    {
                        if (filecodeText[i] == '(') leftParentess++;
                        if (filecodeText[i] == ')') rightParentess++;
                        i++;
                    }
                }
                int operatorEnd = i;
                int posForClose = i;
                (bool, int) dataOneline = IsThisOperatorOneLine(ref filecodeText, i);
                if (dataOneline.Item1)
                {
                    i = dataOneline.Item2;
                    while (true)
                    {
                        int startLine = i;
                        while (filecodeText[i] != '\n') i++;
                        i++;
                        int endLine = i;
                        string line = filecodeText.Substring(startLine, endLine - startLine);
                        (string pattern, bool isNeedParentess) onelineData = GetClosestPatternForOneLine(line);
                        if (onelineData.pattern == "None")
                        {
                            posForClose = endLine;
                            break;
                        }
                        else
                        {
                            //проверка на то однолинейный ли оператор
                            Regex onelinePattern = new Regex(onelineData.pattern);
                            Match matchOneline = onelinePattern.Match(filecodeText, startLine);
                            string multiLineOperator = matchOneline.Value;
                            int k = multiLineOperator.Length + matchOneline.Index;
                            if(onelineData.isNeedParentess)
                            {
                                int leftParentess = 1;
                                int rightParentess = 0;
                                while (leftParentess != rightParentess)
                                {
                                    if (filecodeText[k] == '(') leftParentess++;
                                    if (filecodeText[k] == ')') rightParentess++;
                                    k++;
                                }
                            }
                            (bool, int) dataMultiLine = IsThisOperatorOneLine(ref filecodeText, k);
                            if(!dataMultiLine.Item1)
                            {
                                k = dataMultiLine.Item2;
                                k++;
                                int leftParentess = 1;
                                int rightParentess = 0;
                                while (leftParentess != rightParentess)
                                {
                                    if (filecodeText[k] == '{') leftParentess++;
                                    if (filecodeText[k] == '}') rightParentess++;
                                    k++;
                                }
                                i = k;
                            }
                            //else i++;
                        }
                    }
                    filecodeText = filecodeText.Insert(operatorEnd, "\r\n{");
                    operatorEnd += 3;
                    filecodeText = filecodeText.Insert(posForClose + 1, "\r\n}");

                    while (filecodeText[operatorEnd] == '\r' || filecodeText[operatorEnd] == '\n' || filecodeText[operatorEnd] == ' ' || filecodeText[operatorEnd] == '\t') operatorEnd++;

                    string otherLine = filecodeText.Substring(operatorEnd, posForClose + 3 - operatorEnd);
                    int tempLength = otherLine.Length;
                    RemoveOneLineOperators(ref otherLine);
                    filecodeText = filecodeText.Remove(operatorEnd, tempLength);
                    filecodeText = filecodeText.Insert(operatorEnd, otherLine);

                    operatorEnd += otherLine.Length;
                    otherLine = filecodeText.Substring(operatorEnd);
                    tempLength = otherLine.Length;
                    RemoveOneLineOperators(ref otherLine);
                    filecodeText = filecodeText.Remove(operatorEnd, tempLength);
                    filecodeText = filecodeText.Insert(operatorEnd, otherLine);
                }
                else
                {
                    string otherLine = filecodeText.Substring(operatorEnd);
                    RemoveOneLineOperators(ref otherLine);
                    filecodeText = filecodeText.Remove(operatorEnd);
                    filecodeText = filecodeText.Insert(operatorEnd, otherLine);
                }

                //RemoveOneLineOperators();
            }
        }

        bool RemoveElseFromIfOperators(ref string filecodeText)
        {
            Regex elsePattern = new Regex(@"\belse(\s)+[{]{1}");
            Match elseMatch = elsePattern.Match(filecodeText);
            if (elseMatch.Success)
            {
                string temp = elseMatch.Value;
                filecodeText = filecodeText.Remove(elseMatch.Index, temp.Length);
                filecodeText = filecodeText.Insert(elseMatch.Index, "fif( )\r\n{");
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
            Regex IfPattern = new Regex(closestPattern);
            Match ifMatch = IfPattern.Match(filecodeText);
            if (!needToCountForWhiles)
            {
                if (closestPattern != @"\bfor(\s)*[(]{1}" && closestPattern != @"\bwhile(\s)*[(]{1}" && closestPattern != @"\bdoWhile(\s)*[(]{1}")
                    absolutDifficulty++;
            }
            else absolutDifficulty++;
            allOperators++;

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

        void SolveTaskForSwitch(ref string filecodeText, int level)
        {
            Regex whenPattern = new Regex(@"\bwhen(\s)*[(]{1}");
            Match whenMatch = whenPattern.Match(filecodeText);
            int i;

            string temp = whenMatch.Value;
            i = whenMatch.Index + temp.Length;
            while (filecodeText[i] != '{') i++;
            i++;

            allOperators++;

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

            Regex casePattern = new Regex(@"->(\s)+[{]{1}");
            string whenBlock = filecodeText.Substring(whenBlockStart,whenBlockEnd - whenBlockStart - 1);
            MatchCollection matches = casePattern.Matches(whenBlock);
            int j = 1;
            int amountOfCases = matches.Count;
            Regex elseCase = new Regex(@"\bfelse(\s)+->(\s)*[{]{1}");
            if (elseCase.Match(whenBlock).Success) amountOfCases--;//последний else не считается
            absolutDifficulty += amountOfCases;
            //allOperators += amountOfCases;
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
            string[] patterns = { @"\bif(\s)*[(]{1}", @"\bfif(\s)*[(]{1}", @"\bfor(\s)*[(]{1}", @"\bwhile(\s)*[(]{1}", @"\bdoWhile(\s)*[(]{1}", @"\bwhen(\s)*[(]{1}" };
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
