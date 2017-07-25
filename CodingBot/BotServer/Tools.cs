using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodingBot
{
    public class Tools
    {
        public static byte GetFeedbackType(string input)
        {
            string utterance = input.ToLower();
            if (utterance.Equals("yes") || utterance.Equals("y") || utterance.Equals("yep") || utterance.Equals("yeah"))
            {
                return 1; // positive
            }
            else if (utterance.Equals("no") || utterance.Equals("n") || utterance.Equals("nope") || utterance.Equals("not yet") || utterance.Equals("not"))
            {
                return 0; // negative
            }
            else
            {
                return 2; // not sure
            }
        }

        public static bool IsFeedback(string input)
        {
            string utterance = input.ToLower();
            if (utterance.Equals("yes") || utterance.Equals("y") || utterance.Equals("yep") || utterance.Equals("yeah"))
            {
                return true; // positive feedback
            }
            else if (utterance.Equals("no") || utterance.Equals("n") || utterance.Equals("nope") || utterance.Equals("not yet") || utterance.Equals("not"))
            {
                return true; // negative feedback
            }
            else
            {
                return false; // not feedback
            }
        }

        public static bool IsAppreciation(string input)
        {
            string text = input.Trim();
            return Resource.m_hAppreciation.Contains(text);
        }

        public static bool IsGreeting(string input)
        {
            string text = input.Trim();
            return Resource.m_hGreeting.Contains(text);
        }

        public static byte IsHelp(string input)
        {
            if (input == "help" || input == "h")
            {
                return 99; // general help
            }
            else if (input.StartsWith("help"))
            {
                int index = input.IndexOf("help ");
                string param = input.Substring("help ".Length);
                if (param.IndexOf("product") >= 0)
                {
                    return 1;
                }
                else if (param.IndexOf("version") >= 0)
                {
                    return 2;
                }
                else if (param.IndexOf("platform") >= 0)
                {
                    return 4;
                }
                else if (param.IndexOf("function") >= 0)
                {
                    return 5;
                }
                else
                {
                    return 99; // general help
                }
            }
            else if (input.StartsWith("h "))
            {
                int index = input.IndexOf("h ");
                string param = input.Substring("h ".Length);
                if (param.IndexOf("product") >= 0)
                {
                    return 1;
                }
                else if (param.IndexOf("version") >= 0)
                {
                    return 2;
                }
                else if (param.IndexOf("platform") >= 0)
                {
                    return 4;
                }
                else if (param.IndexOf("function") >= 0)
                {
                    return 5;
                }
                else
                {
                    return 99; // general help
                }
            }
            else
            {
                return 0;
            }
        }

        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }


}