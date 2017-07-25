using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CodingBot.ViewModels;

namespace CodingBot
{
    public class Issue
    {
        public int m_nCurrentTopicId;
        public List<string> m_lDesc; // all text
        public HashSet<string> m_hKeywords;

        //record which table user has selected
        public Dictionary<string,TableItem> AllTableItems;

        public List<TableItem> SelectedTableItems;



        public List<string> SelectedColumns;

        public List<List<string>> SelectedKeys;
        public int ActionType;

        public bool IsSessionStart;

        public string Operation;
        public string SubOperation;

        public string ScriptCode;
        public string CSharpCode;

        public string FilterCondition;
        public Issue()
        {

            // V2 issue structure
            m_nCurrentTopicId = 1;
            m_hKeywords = new HashSet<string>();
            m_lDesc = new List<string>();

            AllTableItems = new Dictionary<string, TableItem>();

            SelectedTableItems = new List<TableItem>();

            ActionType = 0;

            ScriptCode = "";
            CSharpCode = "";
        }

        public void Clear()
        {

            // V2 issue structure
            m_hKeywords.Clear();
            m_lDesc.Clear();

            m_nCurrentTopicId = 1;
            AllTableItems.Clear();
        }

        public string Format()
        {
            string issue = "";

            // V2 keywords          
            if (m_hKeywords.Count > 0)
            {
                string keywords = "";
                issue = "\r" + "{Keywords: ";
                foreach (string key in m_hKeywords)
                {
                    if (keywords.Length > 0)
                    {
                        keywords += ", ";
                    }
                    keywords += key;
                }

                if (keywords.Length > 0)
                {
                    issue += keywords;
                }

                issue += "}";
            }

            // conclude 
            issue += "}";
            issue += "\r";

            return issue;
        }

    }
}
