using CodingBot.Common;
using CodingBot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot
{
    class Bot
    {

        public Dispatcher m_Dispatcher;
        private string m_userId;

        public void Init()
        {
            m_Dispatcher = new Dispatcher();
            Resource.Load(@".\");
        }




        public ResponseData InitializeUserInputData(UserData userData)
        {
            List<string> inputPaths = userData.InputPath;
            List<string> outputPaths = userData.OutputPath;
            List<string> resourcePaths = userData.ResourcePath;
            List<string> referencePaths = userData.ReferencePath;



            List<List<string>> resource_list = new List<List<string>>();
            foreach (string resourcePath in resourcePaths)
            {
                resource_list.Add(new List<string> { resourcePath });
            }

            List<List<string>> reference_list = new List<List<string>>();
            foreach (string referencePath in referencePaths)
            {
                reference_list.Add(new List<string> { referencePath });
            }

            List<List<string>> inpath_list = new List<List<string>>();
            foreach (string inputPath in inputPaths)
            {
                inpath_list.Add(new List<string> { inputPath });
            }


            List<List<string>> outpath_list = new List<List<string>>();
            foreach (string outputPath in outputPaths)
            {
                reference_list.Add(new List<string> { outputPath });
            }

            string Operation = "DECLARE";
            string SubOperation = "";
            List<List<List<string>>> Tables = new List<List<List<string>>> { resource_list, reference_list, inpath_list, outpath_list };
            List<List<string>> Keys = new List<List<string>> { new List<string> { "AdId", "KeyWord" }, new List<string> { "AdText", "BidingWord" } };
            string FilterCondition = ",";// "AdId != \"100010123\"";
            List<List<string>> SelectColumns = new List<List<string>> { new List<string> { "AdId", "KeyWord" }, new List<string> { "AdText", "BidingWord" }, new List<string> { "AdId", "BidingWord" } };
            Dictionary<string, object> BotData = new Dictionary<string, object> { {"Operation", Operation},
                                                                                  {"SubOperation", SubOperation},
                                                                                      {"Tables", Tables},
                                                                                      {"Keys", Keys},
                                                                                      {"FilterCondition", FilterCondition},
                                                                                      {"SelectColumns",SelectColumns}};

            //List<List<string>> codeResults = CodeGenerator.CodeGenerate(BotData);

            //***************************************************************************************************

            List<string> TableName = new List<string> { "Table1" };
            List<string> Columns = new List<string> { "AdId", "AdText" };
            List<string> Types = new List<string> { "string", "string" };
            List<string> Code = new List<string> { "Table1 = SELECT AdId:string, AdText:string FROM @\"input1\"", "" };
            List<List<string>> codeResults = new List<List<string>> { TableName, Columns, Types, Code };
            //***************************************************************************************************

            // whether the CodeGenerator produced new table
            if (codeResults[0].Count > 0)
            {
                TableItem newTableItem = new TableItem(codeResults[0], codeResults[1], codeResults[2]);
                m_Dispatcher.m_Issue.AllTableItems.Add(newTableItem.TableName, newTableItem);
            }


            //TODO:Delete 
            List<string> TableName1 = new List<string> { "Table2" };
            List<string> Columns1 = new List<string> { "AdId", "AdTitle" };
            List<string> Types1 = new List<string> { "string", "string" };
            TableItem newTableItem1 = new TableItem(TableName1, Columns1, Types1);
            m_Dispatcher.m_Issue.AllTableItems.Add(newTableItem1.TableName, newTableItem1);


            string scriptCode = codeResults[3][0];
            string csharpCode = codeResults[3][1];

            if (scriptCode != "")
            {
                m_Dispatcher.m_Issue.ScriptCode += scriptCode;
            }
            if (csharpCode != "")
            {
                m_Dispatcher.m_Issue.CSharpCode += csharpCode;
            }

            string question = "";
            question += "\n What do you want to do next?\n";

            // parse only one table: filter, process, reduce, aggregate , cross apply
            // parsed multiple table: union, join, except, combine
            question += "1. Filter\t2. PROCESS\t3. REDUCE\t4.AGGREGATE\t5.CROSS APPLY\n";
            if (m_Dispatcher.m_Issue.AllTableItems.Count > 1)
            {
                question += "6. UNION\t7.JOIN\t8.EXCEPT\t9.COMBINE\n";

            }
            question += "Or just input what you need:";


            ResponseData responseData = new ResponseData();

            responseData.BotMessage = question;

            m_Dispatcher.m_Issue.IsSessionStart = true;

            responseData.SciptContent = m_Dispatcher.m_Issue.ScriptCode + "#CS\n" + m_Dispatcher.m_Issue.CSharpCode + "\n#ENDCS";



            return responseData;
        }


        public ResponseData GetResponse(string userInput)
        {

            m_Dispatcher.Parse(userInput);
            ResponseData responseData = m_Dispatcher.Action();

            return responseData;
        }


        public ResponseData GetResponseInteractive(List<List<string>> userInput)
        {
            int actionType = m_Dispatcher.m_Issue.ActionType;

            m_Dispatcher.m_Issue.ActionType = 0;

            if (actionType == 1) // select one table
            {
                string table_name = userInput[0][0];
                TableItem tableItem = m_Dispatcher.m_Issue.AllTableItems[table_name];
                m_Dispatcher.m_Issue.SelectedTableItems = new List<TableItem> { tableItem };
            }
            else if (actionType == 2) // select columns
            {
                List<string> selectedColumns = userInput[0];

                m_Dispatcher.m_Issue.SelectedColumns = selectedColumns;
            }
            else if (actionType == 3) // select keys
            {
                List<string> keys1 = userInput[0];
                List<string> keys2 = userInput[1];

                m_Dispatcher.m_Issue.SelectedKeys = new List<List<string>> { keys1, keys2 };
            }
            else if (actionType == 4) // select two tables
            {
                string table1 = userInput[0][1];
                string table2 = userInput[0][2];
            }
            else
            {
                // Invalid input
            }
            m_Dispatcher.Parse("");
            ResponseData responseData = m_Dispatcher.Action();

            return responseData;

        }

    }
}
