using CodingBot.Common;
using CodingBot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
namespace CodingBot
{
    class Bot
    {

        public Dispatcher m_Dispatcher;
        private string m_userId;

        public void Init()
        {
            m_Dispatcher = new Dispatcher();

            string startupPath = Environment.CurrentDirectory;
            string resourceDir = System.IO.Directory.GetParent(startupPath).FullName;
            Resource.Load(resourceDir);
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
                outpath_list.Add(new List<string> { outputPath });
            }

            //generate declare code
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
                                                                                      {"SelectColumns",SelectColumns},
                                                                                       {"TableName", "" } };

            List<List<string>> DeclareCodeResults = CodeGenerator.CodeGenerate(BotData);


            string scriptCode = DeclareCodeResults[3][0];
            string csharpCode = DeclareCodeResults[3][1];

            if (scriptCode != "")
            {
                m_Dispatcher.m_Issue.ScriptCode += scriptCode;
            }
            if (csharpCode != "")
            {
                m_Dispatcher.m_Issue.CSharpCode += csharpCode;
            }

            
            // parse file path
            PathParser pathParser = new PathParser();


            for (int i = 0; i < inputPaths.Count; i++)
            {
                TableItem inputTableItem = pathParser.ParseFilePath(i, inputPaths[i]);
               

                // generate EXTRACT code or SELECT code
                Operation = "EXTRACT";
                if (inputPaths[i].IndexOf(".ss") == inputPaths[i].Length - 3)
                {
                    Operation = "FILTER";
                }

                SubOperation = "";
                Tables = new List<List<List<string>>> {inputTableItem.ToStringList()};

                Keys = new List<List<string>> { new List<string> { "AdId", "KeyWord" }, new List<string> { "AdText", "BidingWord" } };
                FilterCondition = "";// "AdId != \"100010123\"";
                SelectColumns = new List<List<string>> { new List<string> { "AdId", "KeyWord" }, new List<string> { "AdText", "BidingWord" }, new List<string> { "AdId", "BidingWord" } };
                BotData = new Dictionary<string, object> { {"Operation", Operation},
                                                                                  {"SubOperation", SubOperation},
                                                                                      {"Tables", Tables},
                                                                                      {"Keys", Keys},
                                                                                      {"FilterCondition", FilterCondition},
                                                                                      {"SelectColumns",SelectColumns},
                                                                                        { "TableName", "Table_" + (1 + i)} };


                List<List<string>> EXTRACTCodeResults = CodeGenerator.CodeGenerate(BotData);
                TableItem newTableItem1 = new TableItem(EXTRACTCodeResults[0], EXTRACTCodeResults[1], EXTRACTCodeResults[2]);
                m_Dispatcher.m_Issue.AllTableItems.Add(newTableItem1.TableName, newTableItem1);

                scriptCode = EXTRACTCodeResults[3][0];
                csharpCode = EXTRACTCodeResults[3][1];

                if (scriptCode != "")
                {
                    m_Dispatcher.m_Issue.ScriptCode += scriptCode;
                }
                if (csharpCode != "")
                {
                    m_Dispatcher.m_Issue.CSharpCode += csharpCode;
                }

            }

            
            

            /* 
                        //***************************************************************************************************

                        List<string> TableName = new List<string> { "Table1" };
                        List<string> Columns = new List<string> { "AdId", "AdText" };
                        List<string> Types = new List<string> { "string", "string" };
                        List<string> Code = new List<string> { "Table1 = SELECT AdId:string, AdText:string FROM @\"input1\"", "" };
                        List<List<string>> codeResults = new List<List<string>> { TableName, Columns, Types, Code };
                        //***************************************************************************************************

            */

            /*
            //DELETE: 
            List<string> TableName1 = new List<string> { "Table1" };
            List<string> Columns1 = new List<string> { "AdId", "AdTitle" };
            List<string> Types1 = new List<string> { "string", "string" };
            TableItem newTableItem1 = new TableItem(TableName1, Columns1, Types1);
            m_Dispatcher.m_Issue.AllTableItems.Add(newTableItem1.TableName, newTableItem1);

            List<string> TableName2 = new List<string> { "Table2" };
            List<string> Columns2 = new List<string> { "AdId", "AdText" };
            List<string> Types2 = new List<string> { "string", "string" };
            TableItem newTableItem2 = new TableItem(TableName2, Columns2, Types2);
            m_Dispatcher.m_Issue.AllTableItems.Add(newTableItem2.TableName, newTableItem2);
            
            */

            string question = "";
            question += "What do you want to do next?\n";

            // parse only one table: filter, process, reduce, aggregate , cross apply
            // parsed multiple table: union, join, except, combine
            question += "1. Filter\n2. PROCESS\n3. REDUCE\n4.AGGREGATE\n5.CROSS APPLY\n";
            if (m_Dispatcher.m_Issue.AllTableItems.Count > 1)
            {
                question += "6. UNION\n7.JOIN\n8.EXCEPT\n9.COMBINE\n";

            }
            question += "Or just input what you need:";


            ResponseData responseData = new ResponseData();

            responseData.BotMessage = question;

            m_Dispatcher.m_Issue.IsSessionStart = true;

            responseData.SciptContent = m_Dispatcher.m_Issue.ScriptCode + "#CS\n" + m_Dispatcher.m_Issue.CSharpCode + "\n#ENDCS";

            responseData.AllTableItems = m_Dispatcher.m_Issue.AllTableItems.Values.ToList();

            return responseData;
        }


        public ResponseData GetResponse(string userInput)
        {

            m_Dispatcher.Parse(userInput);
            ResponseData responseData = m_Dispatcher.Action();

            responseData.AllTableItems = m_Dispatcher.m_Issue.AllTableItems.Values.ToList();
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
            else if (actionType == 5) // select columns
            {
                List<string> selectedColumns = userInput[0];
                List<string> new_list = new List<string>();
                for(int i = 0;i<= userInput[0].Count - 1; i++)
                {
                    string tmp_str = userInput[0][i].Split(new char[] { '('})[0];
                    new_list.Add(tmp_str);
                }

                m_Dispatcher.m_Issue.SelectedColumns = new_list;
            }
            else if (actionType == 3) // select keys
            {

                List<List<string>> keys = new List<List<string>>();
                for (int j = 0; j <= userInput.Count - 1; j++)
                {
                    List<string> temp_key = userInput[j];
                    keys.Add(temp_key);
                }


                List<string> pureKeys1 = new List<string>();
                List<string> pureKeys2 = new List<string>();

                for (int k = 0; k <= keys.Count - 1; k++) { 
                   pureKeys1.Add(keys[k][0].Split(new char[] { '(' })[0]);
                   pureKeys2.Add(keys[k][1].Split(new char[] { '(' })[0]);
                }
  

                m_Dispatcher.m_Issue.SelectedKeys = new List<List<string>> { pureKeys1, pureKeys2 };
            }
            else if (actionType == 2) // select two tables
            {
                string table1 = userInput[0][0];
                string table2 = userInput[0][1];
                m_Dispatcher.m_Issue.SelectedTableItems = new List<TableItem> { m_Dispatcher.m_Issue.AllTableItems[table1], m_Dispatcher.m_Issue.AllTableItems[table2] };
            }
            else
            {
                // Invalid input
            }
            m_Dispatcher.Parse("");
            ResponseData responseData = m_Dispatcher.Action();

            responseData.AllTableItems = m_Dispatcher.m_Issue.AllTableItems.Values.ToList();
            return responseData;

        }

    }
}
