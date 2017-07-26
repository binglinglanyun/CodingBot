using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using CodingBot.Common;
using CodingBot.ViewModels;

namespace CodingBot
{
    public class Taxonomy
    {
        public Taxonomy()
        { }


        public ResponseData getResponseData(ref Issue issue)  //get question and update the currentid and keywords
        {
            ResponseData responseData = new ResponseData();

            string question = "";

            int nextnode = 1;

            // check whether it is a new session
            if (issue.IsSessionStart)
            {
                // we need to judge which operation user has select
                nextnode = getRootNode(ref issue);
                issue.IsSessionStart = false;
            }
            else
            {
                // input desc and themeid, get nextnode and update the keywords
                nextnode = getNextOneNode(ref issue);
            }

            bool validInput = true;
            //check whether users input is valid
            if ((nextnode != -1) && (nextnode != 1) && (issue.m_lDesc.Count >0) && (issue.m_lDesc.Last() != ""))
            {
                HashSet<string> keywordsOfCurrentNode = Resource.m_DTopicTree[nextnode].Item3;
                string userInput = issue.m_lDesc.Last();

                validInput = false;

                foreach(var keyword in keywordsOfCurrentNode)
                {
                    if(userInput.ToLower().IndexOf(keyword.ToLower(), StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        validInput = true;
                    }
                }

                if (!validInput)
                {
                    nextnode = -1;
                }
            }


            if (nextnode == -1)
            {
                // no node found, we regard user's input as invalid

                question = "Please correct your input";
                responseData.BotMessage = question;
            }
            else if (nextnode == 1)
            {
                // workflow completed! Need to generate code
                question += "Code Generation Completed!\n\n";

                question += "\n What do you want to do next?\n";
                // parse only one table: filter, process, reduce, aggregate , cross apply
                // parsed multiple table: union, join, except, combine

                question += "1. Filter\t2. PROCESS\t3. REDUCE\t4.AGGREGATE\t5.CROSS APPLY\n";
                if (issue.AllTableItems.Count > 1)
                {
                    question += "6. UNION\t7.JOIN\t8.EXCEPT\t9.COMBINE\n";

                }
                question += "Or just input what you need:";

                responseData.BotMessage = question;

                issue.IsSessionStart = true;
                //TODO: generate code and refresh table
                responseData.SciptContent = generateCode(ref issue);
            }
            else
            {
                //TODO: if nextnode is suboperation of join or aggregate, we need to update the issue.Suboperation field
                if (nextnode == 105001 || nextnode == 102001)
                {
                    if(issue.SubOperation == "")
                        issue.SubOperation = issue.m_lDesc.Last();
                }

                //TODO: remenber users's input, such as filter/APPLY
                if (nextnode == 101002 || nextnode == 106002 || nextnode == 107002 || nextnode == 108002 || nextnode == 109002)
                {
                    issue.FilterCondition = issue.m_lDesc.Last();
                }
                question = Resource.m_DTopicTree[nextnode].Item4;
                responseData.BotMessage = question;
                if (Resource.m_DTopicTree[nextnode].Item5 != 0)
                {

/*
    public enum TableOperationType
    {
        None = 0,  // Don't need to opreation any control
        ShowRadioBox = 1,  // Need to show radio box
        ShowCheckBox = 2,  // Need to show check box
        ShowMultiComboBox = 3,  // Need to show multiple check box
        UpdateDataStatus = 4,  // Need to Update Data Status
        ShowCheckBoxForSingleTable = 5, // Show CheckBox For Single Table
    }
 */
                    int actionType = Resource.m_DTopicTree[nextnode].Item5;

                    issue.ActionType = actionType;

                    // select table
                    if (actionType == 1)
                    {
                        responseData.TableOperation = TableOperationType.ShowRadioBox;
                        responseData.TableItems = issue.AllTableItems.Values.ToList();
                    }
                    // select columns
                    else if (actionType == 5)
                    {
                        responseData.TableOperation = TableOperationType.ShowCheckBoxForSingleTable;
                        responseData.TableItems = issue.SelectedTableItems;
                    }
                    // select keys for join
                    else if (actionType == 3)
                    {
                        responseData.TableOperation = TableOperationType.ShowMultiComboBox;
                        responseData.TableItems = issue.SelectedTableItems;
                    }
                    // select two table
                    else if (actionType == 2)
                    {
                        responseData.TableOperation = TableOperationType.ShowCheckBox;
                        responseData.TableItems = issue.AllTableItems.Values.ToList();
                    }
                }


            }

            if(validInput)
                issue.m_nCurrentTopicId = nextnode; //if invalid, we keep the last node
            return responseData;
        }



        //input desc and themeid, return nextnode and update the keywords
        public int getNextOneNode(ref Issue issue)
        {
            List<int> subNodes = getAllChildren(issue.m_nCurrentTopicId);


            if (subNodes.Count == 1)
            {
                return subNodes[0];
            }
            else
            {
                int matchedNode = MatchKeyword(issue.m_lDesc.Last(), issue.m_nCurrentTopicId);
                return matchedNode;
            }


        }



        public int MatchKeyword(string input, int themeid) //matched nodes id and matched keywords
        {

            input = input.ToLower();
            char[] spaceSpliter = { ' ' };


            string[] words = input.Split(spaceSpliter, StringSplitOptions.RemoveEmptyEntries);
            int ngram = 4;// word ngram=4

            int matchedNode = -1;
            for (int i = 0; i < words.Length; i++)
            {
                List<string> wordNgramlist = new List<string>();
                int k = 0;
                for (int j = 0; (i + j < words.Length) && k < ngram; j++)
                {
                    wordNgramlist.Add(words[i + j]);
                    k++;
                }


                do
                {
                    string wordNgram = List2String(wordNgramlist);
                    int matchedCount = 0;

                    foreach (var child in getAllChildren(themeid))
                    {
                        if (Resource.m_DTopicTree[child].Item3.Contains(wordNgram))
                        {
                            matchedNode = child;
                            matchedCount += 1;
                            break;
                        }
                    }

                    if (matchedCount > 0)
                    {
                        break;
                    }
                    wordNgramlist.RemoveAt(wordNgramlist.Count - 1);
                } while (wordNgramlist.Count > 0);


            }//end for

            return matchedNode;
        }

        private string List2String(List<string> wordNgramlist)
        {
            if (wordNgramlist == null || wordNgramlist.Count == 0)
            {
                return "";
            }
            string result = "";
            foreach (var item in wordNgramlist)
            {
                if (result != "")
                {
                    result += " ";
                }
                result += item;
            }
            return result;
        }



        public List<int> getAllChildren(int nodeid) //get all children including itselft
        {
            List<int> result = new List<int>();

            if (Resource.m_DTopicTree.ContainsKey(nodeid))
            {

                if (Resource.m_DTopicTree[nodeid].Item2.Count != 0)
                {
                    foreach (var item in Resource.m_DTopicTree[nodeid].Item2)
                    {
                        result.Add(item);
                    }
                }
            }


            return result;
        }

        public string generateCode(ref Issue issue)
        {

            string Operation = issue.Operation;
            string SubOperation = issue.SubOperation;

            List<List<List<string>>> Tables = new List<List<List<string>>>();
            foreach (TableItem tableItem in issue.SelectedTableItems)
            {
                Tables.Add(tableItem.ToStringList());

            }


            List<List<string>> Keys = issue.SelectedKeys;
            string FilterCondition = "";

            FilterCondition = issue.FilterCondition;

            List<List<string>> SelectColumns = new List<List<string>> { issue.SelectedColumns };

            string newTableName = issue.m_lDesc.Last();
            if (newTableName == "" || newTableName == "no" || newTableName == "NO" || newTableName == "N" || newTableName == "n")
            {
                newTableName = "Table_" + (1 + issue.AllTableItems.Count);
            }

            Dictionary<string, object> BotData = new Dictionary<string, object> { {"Operation", Operation},
                                                                                  {"SubOperation", SubOperation},
                                                                                      {"Tables", Tables},
                                                                                      {"Keys", Keys},
                                                                                      {"FilterCondition", FilterCondition},
                                                                                      {"SelectColumns",SelectColumns},
                                                                                      { "TableName", newTableName} };

            List<List<string>> codeResults = CodeGenerator.CodeGenerate(BotData);

            // whether the CodeGenerator produced new table
            if (codeResults[0].Count > 0)
            {
                TableItem newTableItem = new TableItem(codeResults[0], codeResults[1], codeResults[2]);
                issue.AllTableItems.Add(newTableItem.TableName, newTableItem);
            }

            string scriptCode = codeResults[3][0];
            string csharpCode = codeResults[3][1];

            if (scriptCode != "")
            {
                issue.ScriptCode += scriptCode;
            }
            if (csharpCode != "")
            {
                issue.CSharpCode += csharpCode;
            }

            string FinalCode = issue.ScriptCode + "#CS\n" + issue.CSharpCode + "\n#ENDCS";

            return FinalCode;
        }
        public int getRootNode(ref Issue issus)
        {
            // we need to judge which operation user has selected from users' input

            string userInput = issus.m_lDesc.Last();

            //1. Filter\t2. PROCESS\t3. REDUCE\t4.AGGREGATE\t5.CROSS APPLY\n
            //6. UNION\t7.JOIN\t8.EXCEPT\t9.COMBINE\n
            int nextnode = -1;
            if (userInput == "1" || userInput.IndexOf("Filter", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                issus.Operation = "FILTER";
                nextnode = 101;
            }
            else if (userInput == "2" || userInput.IndexOf("process", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                issus.Operation = "PROCESS";
                nextnode = 106;
            }
            else if (userInput == "3" || userInput.IndexOf("REDUCE", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                issus.Operation = "REDUCE";
                nextnode = 107;
            }
            else if (userInput == "4" || userInput.IndexOf("AGGREGATE", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                issus.Operation = "AGGREGATE";
                nextnode = 105;
            }
            else if (userInput == "5" || userInput.IndexOf("CROSS APPLY", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                issus.Operation = "APPLY";
                nextnode = 109;
            }
            else if (userInput == "6" || userInput.IndexOf("UNION", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                issus.Operation = "UNION";
                nextnode = 104;
            }
            else if (userInput == "7" || userInput.IndexOf("JOIN", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                issus.Operation = "JOIN";
                nextnode = 102;
            }
            else if (userInput == "8" || userInput.IndexOf("EXCEPT", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                issus.Operation = "EXCEPT";
                nextnode = 103;
            }
            else if (userInput == "9" || userInput.IndexOf("COMBINE", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                issus.Operation = "COMBINE";
                nextnode = 108;
            }
            else
            {
                // user can only input valid suboperation

                Dictionary<string, int> suboperations = new Dictionary<string, int>();

                //JOIN :    INNER JOIN,LEFT OUTER JOIN,RIGHT OUTER JOIN,LEFT SEMIJOIN,RIGHT SEMIJOIN,CROSS JOIN

                suboperations.Add("INNER JOIN", 102001); suboperations.Add("LEFT OUTER JOIN", 102001); suboperations.Add("RIGHT OUTER JOIN", 102001); suboperations.Add("LEFT SEMIJOIN", 102001);
                suboperations.Add("RIGHT SEMIJOIN", 102001); suboperations.Add("CROSS JOIN", 102001);

                //Aggregate: SUM/AVG/COUNT
                suboperations.Add("SUM", 105001); suboperations.Add("AVG", 105001); suboperations.Add("COUNT", 105001); suboperations.Add("SUMMARY", 105001); suboperations.Add("CNT", 105001); suboperations.Add("AVERAGE", 105001);

                string userinput_ = userInput.ToUpper();

                string specialKey = "";
                foreach (var key in suboperations.Keys)
                {
                    if (userinput_.IndexOf(key) >= 0)
                    {
                        specialKey = key;
                        break;
                    }
                }

                if (specialKey != "")
                {
                    specialKey = specialKey == "SUMMARY" ? "SUM" : specialKey;
                    specialKey = specialKey == "CNT" ? "COUNT" : specialKey;
                    specialKey = specialKey == "AVERAGE" ? "AVG" : specialKey;
            


                    nextnode = suboperations[specialKey];


                    issus.SubOperation = specialKey;
                    if (nextnode == 102001)
                    {
                        issus.Operation = "JOIN";
                    }
                    else if (nextnode == 105001)
                    {
                        issus.Operation = "AGGREGATE";
                    }
                }
            }

            return nextnode;

        }
    }
}
