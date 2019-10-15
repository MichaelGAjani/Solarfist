// FileInfo
// File:"RegularExpressions.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:RegularExpressions
// 1.ExtractGroupings(string source, string matchPattern, bool wantInitialMatch)
// 2.VerifyRegEx(string testPattern)
// 3.MatchHandler(Match theMatch)
// 4.Tokenize(string equation)
// 5.GetLines(string source, string pattern, bool isFileName)
// 6.GetBeginningOfLine(string text, int startPointOfMatch)
// 7.GetEndOfLine(string text, int endPointOfMatch)
// 8.FindOccurrenceOf(string source, string pattern, int occurrence)
// 9.FindEachOccurrenceOf(string source, string pattern, int occurrence)
// 10.IsMatch(string input, string pattern)
// 11.IsMatch(string input, string pattern, RegexOptions options)
//
// File Lines:278

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Jund.NETHelper.RegularHelper
{
    public class RegularExpressions
    {
        public static List<Dictionary<string, Group>> ExtractGroupings(string source, string matchPattern, bool wantInitialMatch)
        {
            List<Dictionary<string, Group>> keyedMatches =
            new List<Dictionary<string, Group>>();
            int startingElement = 1;
            if (wantInitialMatch)
            {
                startingElement = 0;
            }
            Regex RE = new Regex(matchPattern, RegexOptions.Multiline);
            MatchCollection theMatches = RE.Matches(source);
            foreach (Match m in theMatches)
            {
                Dictionary<string, Group> groupings = new Dictionary<string, Group>();
                for (int counter = startingElement; counter < m.Groups.Count; counter++)
                {
                    // If we had just returned the MatchCollection directly, the
                    // GroupNameFromNumber method would not be available to use.
                    groupings.Add(RE.GroupNameFromNumber(counter), m.Groups[counter]);
                }
                keyedMatches.Add(groupings);
            }
            return (keyedMatches);
        }
        public static bool VerifyRegEx(string testPattern)
        {
            bool isValid = true;
            if ((testPattern?.Length ?? 0) > 0)
            {
                try
                {
                    Regex.Match("", testPattern);
                }
                catch (ArgumentException)
                {
                    // BAD PATTERN: syntax error
                    isValid = false;
                }
            }
            else
            {
                //BAD PATTERN: pattern is null or empty
                isValid = false;
            }
            return (isValid);
        }
        public static string MatchHandler(Match theMatch)
        {
            // Handle all ControlID_ entries.
            if (theMatch.Value.StartsWith("ControlID_", StringComparison.Ordinal))
            {
                long controlValue = 0;
                // Obtain the numeric value of the Top attribute.
                Match topAttributeMatch = Regex.Match(theMatch.Value, "Top=([-]*\\d*)");
                if (topAttributeMatch.Success)
                {
                    if (topAttributeMatch.Groups[1].Value.Trim().Equals(""))
                    {
                        // If blank, set to zero.
                        return (theMatch.Value.Replace(
                        topAttributeMatch.Groups[0].Value.Trim(),
                        "Top=0"));
                    }
                    else if (topAttributeMatch.Groups[1].Value.Trim().StartsWith("-"
                    , StringComparison.Ordinal))
                    {
                        // If only a negative sign (syntax error), set to zero.
                        return (theMatch.Value.Replace(
                        topAttributeMatch.Groups[0].Value.Trim(), "Top=0"));
                    }
                    else
                    {
                        // We have a valid number.
                        // Convert the matched string to a numeric value.
                        controlValue = long.Parse(topAttributeMatch.Groups[1].Value,
System.Globalization.NumberStyles.Any);
                        // If the Top attribute is out of the specified range,
                        // set it to zero.
                        if (controlValue < 0 || controlValue > 5000)
                        {
                            return (theMatch.Value.Replace(
                            topAttributeMatch.Groups[0].Value.Trim(),
                            "Top=0"));
                        }
                    }
                }


            }

            return (theMatch.Value);
        }
        public static string[] Tokenize(string equation)
        {
            Regex re = new Regex(@"([\+\–\*\(\)\^\\])");
            return (re.Split(equation));
        }
        public static List<string> GetLines(string source, string pattern, bool isFileName)
        {
            List<string> matchedLines = new List<string>();

            // If this is a file, get the entire file's text.
            if (isFileName)
            {
                using (FileStream FS = new FileStream(source, FileMode.Open,
                FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader SR = new StreamReader(FS))
                    {
                        Regex RE = new Regex(pattern, RegexOptions.Multiline);
                        string text = "";
                        while (text != null)
                        {
                            text = SR.ReadLine();
                            if (text != null)
                            {
                                // Run the regex on each line in the string.
                                if (RE.IsMatch(text))
                                {
                                    // Get the line if a match was found.
                                    matchedLines.Add(text);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // Run the regex once on the entire string.
                Regex RE = new Regex(pattern, RegexOptions.Multiline);
                MatchCollection theMatches = RE.Matches(source);
                // Use these vars to remember the last line added to matchedLines
                // so that we do not add duplicate lines.
                int lastLineStartPos = -1;
                int lastLineEndPos = -1;
                // Get the line for each match.
                foreach (Match m in theMatches)
                {
                    int lineStartPos = GetBeginningOfLine(source, m.Index);
                    int lineEndPos = GetEndOfLine(source, (m.Index + m.Length - 1));
                    // If this is not a duplicate line, add it.
                    if (lastLineStartPos != lineStartPos &&
                    lastLineEndPos != lineEndPos)
                    {
                        string line = source.Substring(lineStartPos,
                        lineEndPos - lineStartPos);
                        matchedLines.Add(line);
                        // Reset line positions.
                        lastLineStartPos = lineStartPos;
                        lastLineEndPos = lineEndPos;
                    }
                }
            }

            return (matchedLines);
        }
        public static int GetBeginningOfLine(string text, int startPointOfMatch)
        {
            if (startPointOfMatch > 0)
            {
                --startPointOfMatch;
            }
            if (startPointOfMatch >= 0 && startPointOfMatch < text?.Length)
            {
                // Move to the left until the first '\n char is found.
                for (int index = startPointOfMatch; index >= 0; index--)
                {
                    if (text?[index] == '\n')
                    {
                        return (index + 1);
                    }
                }
                return (0);
            }
            return (startPointOfMatch);
        }
        public static int GetEndOfLine(string text, int endPointOfMatch)
        {
            if (endPointOfMatch >= 0 && endPointOfMatch < text?.Length)
            {
                // Move to the right until the first '\n char is found.
                for (int index = endPointOfMatch; index < text.Length; index++)
                {
                    if (text?[index] == '\n')
                    {
                        return (index);
                    }
                }
                return (text.Length);
            }
            return (endPointOfMatch);
        }
        public static Match FindOccurrenceOf(string source, string pattern, int occurrence)
        {
            if (occurrence < 1)
            {
                throw (new ArgumentException("Cannot be less than 1",
                nameof(occurrence)));
            }
            // Make occurrence zero-based.
            --occurrence;
            // Run the regex once on the source string.
            Regex RE = new Regex(pattern, RegexOptions.Multiline);
            MatchCollection theMatches = RE.Matches(source);
            if (occurrence >= theMatches.Count)
            {
                return (null);
            }
            else
            {
                return (theMatches[occurrence]);
            }
        }
        public static List<Match> FindEachOccurrenceOf(string source, string pattern, int occurrence)
        {
            if (occurrence < 1)
            {
                throw (new ArgumentException("Cannot be less than 1",
                nameof(occurrence)));
            }
            List<Match> occurrences = new List<Match>();
            // Run the regex once on the source string.
            Regex RE = new Regex(pattern, RegexOptions.Multiline);
            MatchCollection theMatches = RE.Matches(source);

            for (int index = (occurrence - 1); index < theMatches.Count; index += occurrence)
            {
                occurrences.Add(theMatches[index]);
            }
            return (occurrences);
        }
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }
    }
}
