using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace CodingBot
{
    public static class Resource
    {
        public static bool Load(string dir)
        {
            bool success = true;

            // utterance template
            success &= LoadUtterances(dir);

            // dictionary
            success &= LoadDictionary(dir);

            // taxonomy
            success &= LoadTaxonomy(dir);

            // 
            return success;
        }

        private static bool LoadDictionary(string dir)
        {
            bool success = true;
            success &= LoadAppreciations(dir + @"\dict\thank.txt");
            success &= LoadGreetings(dir + @"\dict\greetings.txt");

            return success;
        }

        private static bool LoadTaxonomy(string dir)
        {
            bool success = true;
            success &= LoadTaxonomyTree(dir + @"\resource\workflow.txt");

            return success;
        }

        private static bool LoadUtterances(string dir)
        {
            bool success = true;

            // utterance template
            success &= LoadInitUtterances(dir + @"\template\init.txt");
            success &= LoadResumeUtterances(dir + @"\template\resume.txt");
            success &= LoadConfirmUtterances(dir + @"\template\confirm.txt");
            success &= LoadReplyUtterances(dir + @"\template\reply.txt");
            success &= LoadFeedbackUtterances(dir + @"\template\feedback.txt");
            success &= LoadAppreciationUtterances(dir + @"\template\appreciation.txt");
            success &= LoadGreetingUtterances(dir + @"\template\greeting.txt");
            success &= LoadEncourageUtterances(dir + @"\template\encourage.txt");
            success &= LoadHelpUtterance(dir + @"\template\help.txt");
            success &= LoadReminderleafUtterances(dir + @"\template\leaf.txt");
            success &= LoadNoThemeUtterances(dir + @"\template\notheme.txt");
            success &= LoadClosePositiveUtterances(dir + @"\template\close.positive.txt");
            success &= LoadCloseNegativeUtterances(dir + @"\template\close.negative.txt");

            return success;
        }

        //load no theme
        private static bool LoadNoThemeUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lNoThemeUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "I have difficulty to fiture out the theme of your issue. Could you elaborate your issue? ";
                    m_lNoThemeUtterances.Add(line);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("No-Theme Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        //leaf
        private static bool LoadReminderleafUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lReminderleafUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "I need finally to know any details of the product you are using including {0}. ";
                    m_lReminderleafUtterances.Add(line);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Reminder-Leaf Utterances  failure failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadAppreciationUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lAppreciationUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Thank you. Let me know anything else I can help with.";
                    m_lGreetingUtterances.Add(line);

                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Appreciation Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadGreetingUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lGreetingUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Greetings! ";
                    m_lGreetingUtterances.Add(line);

                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Greeting Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadEncourageUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lEncourageUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Got it.";
                    m_lEncourageUtterances.Add(line);

                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Encourage Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadHelpUtterance(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            string[] parts = line.Split('\t');
                            if (parts.Length == 2)
                            {
                                m_dHelpUtterances.Add(parts[0].ToLower(), parts[1]);
                            }
                        }
                    }

                    srUtterances.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Help Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadInitUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lInitUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Please describe your issue.";
                    m_lInitUtterances.Add(line);

                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Init Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadResumeUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lResumeUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Your issue is not yet finished. Please continue.";
                    m_lInitUtterances.Add(line);

                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Resume Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadConfirmUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lConfirmUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Issue summary: {0}";
                    m_lConfirmUtterances.Add(line);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Confirmation Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadReplyUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lReplyUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Solution(s) for your reference.";
                    m_lReplyUtterances.Add(line);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Reply Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadFeedbackUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lFeedbackUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Is the solution helpful? ";
                    m_lFeedbackUtterances.Add(line);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Feedbak Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadClosePositiveUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lClosePositiveUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Great! Let's move forward. ";
                    m_lClosePositiveUtterances.Add(line);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Close-positive Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadCloseNegativeUtterances(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_lCloseNegativeUtterances.Add(line);
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Sorry for that. Please contact agent.";
                    m_lCloseNegativeUtterances.Add(line);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Close-negative Utterances loading failure: {0}", e.Message);
                return false;
            }
        }

        private static bool LoadGreetings(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_hGreeting.Add(line.ToLower());
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Hi! ";
                    m_hGreeting.Add(line);

                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Greetings dictionary loading failure: {0}", e.Message);
                return false;
            }
        }


        private static bool LoadAppreciations(string path)
        {
            try
            {
                StreamReader srUtterances = new StreamReader(path);
                if (srUtterances != null)
                {
                    while (!srUtterances.EndOfStream)
                    {
                        string line = srUtterances.ReadLine().Trim();
                        if (line.Length > 0)
                        {
                            m_hAppreciation.Add(line.ToLower());
                        }
                    }
                    srUtterances.Close();
                    return true;
                }
                else
                {
                    string line = "Thanks. ";
                    m_hAppreciation.Add(line);

                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Appreciation dictionary loading failure: {0}", e.Message);
                return false;
            }
        }

        // load taxonomy
        private static bool LoadTaxonomyTree(string path)
        {
            try
            {
                StreamReader srTopicTree = new StreamReader(path); //root node=srTopicTree
                if (srTopicTree != null)
                {
                    while (!srTopicTree.EndOfStream)
                    {
                        string line = srTopicTree.ReadLine();
                        string[] tabseperator = new string[] { "\t" };
                        string[] items = line.Split(tabseperator, StringSplitOptions.None);

                        if (items.Length == 6)
                        {
                            string[] seperator = new string[] { "," };
                            //id pid cid(list) keyword(list) question operation
                            // node id
                            int id = Convert.ToInt32(items[0]);

                            // parent id
                            int pid = items[1] == "" ? -1 : Convert.ToInt32(items[1].Trim());

                            // children id list
                            List<int> cid = new List<int>();
                            if (items[2].Length != 0)
                            {
                                string[] parts = items[2].Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var part in parts)
                                {
                                    cid.Add(Convert.ToInt32(part.Trim()));
                                }
                            }

                            // keyword list
                            HashSet<string> value = new HashSet<string>();
                            if (items[3].Length != 0)
                            {
                                string[] parts = items[3].Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var part in parts)
                                {
                                    if (!value.Contains(part.ToLower()))
                                    {
                                        value.Add(part.ToLower());
                                    }
                                }
                            }

                            // question
                            string question = items[4];

                            // operation
                            int act = Convert.ToInt32(items[5]);

                            m_DTopicTree.Add(id, Tuple.Create(pid, cid, value, question, act));
                        }
                    }
                    srTopicTree.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Taxonomy dictionary loading failure: {0}", e.Message);
                return false;
            }
        }

        public static string Random(List<string> utterances)
        {
            int len = utterances.Count;
            string item = "";
            if (len <= 0) // no item in the list
            {
                return item;
            }
            else if (len == 1) // there is only one item
            {
                item = utterances[0];
            }
            else // there are more than one
            {
                // random the utterances if 
                Random r = new Random();
                int random = r.Next(0, len);
                if (random >= 0 && random < utterances.Count)
                {
                    try
                    {
                        item = utterances[random];
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Trace.TraceError("LX Random utterance with {2} in {1}: {0}", e.Message, len, random);
                    }
                }
            }

            return item;
        }

        public static string Help(byte helpID)
        {
            string result = "";

            switch (helpID)
            {
                case 1: // product help
                    if (m_dHelpUtterances.ContainsKey("product"))
                    {
                        result = m_dHelpUtterances["product"];
                    }
                    else
                    {
                        result = "Product refers to the general software name, e.g., Outlook.";
                    }
                    break;
                case 2: // version help
                    if (m_dHelpUtterances.ContainsKey("version"))
                    {
                        result = m_dHelpUtterances["version"];
                    }
                    else
                    {
                        result = "Version refers to the unique version numbers to unique states of computer software, e.g., 2016 professional. ";
                    }
                    break;
                case 4: // platform help
                    if (m_dHelpUtterances.ContainsKey("platform"))
                    {
                        result = m_dHelpUtterances["platform"];
                    }
                    else
                    {
                        result = "Platform refers to varies operation system or hardware environment, e.g., Win 10.";
                    }
                    break;
                case 5: // function help
                    if (m_dHelpUtterances.ContainsKey("function"))
                    {
                        result = m_dHelpUtterances["function"];
                    }
                    else
                    {
                        result = "Function refers to unique functions of computer software, e.g., sending email. ";
                    }
                    break;
                default: // general
                    if (m_dHelpUtterances.ContainsKey("general"))
                    {
                        result = m_dHelpUtterances["general"];
                    }
                    else
                    {
                        result = "SupportAgent is a chat bot developed by Ads Incubation Beijing Team. It provides automatic technique support Office 365 users. ";
                    }
                    break;
            }

            return result;
        }

        // utterance templates
        public static List<string> m_lInitUtterances = new List<string>();
        public static List<string> m_lResumeUtterances = new List<string>();
        public static List<string> m_lReminderProductUtterances = new List<string>();
        public static List<string> m_lReminderTopicUtterances = new List<string>();
        public static List<string> m_lReminderFunctionUtterances = new List<string>();
        public static List<string> m_lReminderDetailsUtterances = new List<string>();
        public static List<string> m_lConfirmUtterances = new List<string>();
        public static List<string> m_lReplyUtterances = new List<string>();
        public static List<string> m_lFeedbackUtterances = new List<string>();
        public static List<string> m_lGreetingUtterances = new List<string>();
        public static List<string> m_lEncourageUtterances = new List<string>();
        public static List<string> m_lAppreciationUtterances = new List<string>();
        public static List<string> m_lReminderleafUtterances = new List<string>();
        public static List<string> m_lNoThemeUtterances = new List<string>();
        public static List<string> m_lClosePositiveUtterances = new List<string>();
        public static List<string> m_lCloseNegativeUtterances = new List<string>();


        public static Dictionary<string, string> m_dHelpUtterances = new Dictionary<string, string>();

        public static HashSet<string> m_hAppreciation = new HashSet<string>();
        public static HashSet<string> m_hGreeting = new HashSet<string>();

        // taxonomy
        public static Dictionary<int, Tuple<int, List<int>, HashSet<string>, string, int>> m_DTopicTree = new Dictionary<int, Tuple<int, List<int>, HashSet<string>, string, int>>();
    }
}
