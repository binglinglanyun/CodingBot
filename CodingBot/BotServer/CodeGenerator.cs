using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot
{
    public static class CodeGenerator
    {
        public static void Main(string[] args)
        {
            string Operation = "APPLY";
            string SubOperation = "INNER JOIN";
            List<List<string>> Table1 = new List<List<string>> { new List<string> { "table1" }, new List<string> { "AdId", "KeyWord", "AdTitle" }, new List<string> { "int", "string", "string" } };
            List<List<string>> Table2 = new List<List<string>> { new List<string> { "table2" }, new List<string> { "AdId", "BidingWord", "AdText" }, new List<string> { "string", "string", "int" } };
            List<List<string>> Table3 = new List<List<string>> { new List<string> { "table3" }, new List<string> { "AdId", "BidingWord", "AdText" }, new List<string> { "string", "string", "string" } };
            List<List<List<string>>> Tables = new List<List<List<string>>> { Table1, Table2, Table3, new List<List<string>>() };
            List<List<string>> Keys = new List<List<string>> { new List<string> { "AdId", "KeyWord" }, new List<string> { "AdText", "BidingWord" } };
            string FilterCondition = ",";// "AdId != \"100010123\"";
            //List<List<string>> SelectColumns = new List<List<string>> {new List<string>()};
            List<List<string>> SelectColumns = new List<List<string>> { new List<string> { "AdId", "KeyWord" }, new List<string> { "AdText", "BidingWord" }, new List<string> { "AdId", "BidingWord" } };
            Dictionary<string, object> BotData = new Dictionary<string, object> { {"Operation", Operation},
                                                                                  {"SubOperation", SubOperation},
                                                                                      {"Tables", Tables},
                                                                                      {"Keys", Keys},
                                                                                      {"FilterCondition", FilterCondition},
                                                                                      {"SelectColumns",SelectColumns}};




            List<List<string>> res = CodeGenerate(BotData);


        }
        static int table_num = 1;
        public static List<List<string>> CodeGenerate(Dictionary<string, object> command)
        {

            string Operation = (string)command["Operation"];
            string SubOperation = (string)command["SubOperation"];
            List<List<List<string>>> Tables = (List<List<List<string>>>)command["Tables"];
            List<List<string>> Keys = (List<List<string>>)command["Keys"];
            string FilterCondition = (string)command["FilterCondition"];
            List<List<string>> SelectColumns = (List<List<string>>)command["SelectColumns"];


            string code_res = "";
            string cs_code_res = "";

            List<List<string>> table_res = new List<List<string>>();
            List<string> res_table_name = new List<string>();

            List<string> res_col_name = new List<string>();
            List<string> res_col_type = new List<string>();

            switch (Operation)
            { //DECLARE,EXTRACT,FILTER,EXCEPT,UNION,JOIN,AGGRAGATE,PROCESS,REDUCE,COMBINE,APPLY

                case "DECLARE":
                    List<List<string>> resource_list = Tables[0];
                    List<List<string>> reference_list = Tables[1];
                    List<List<string>> inpath_list = Tables[2];
                    List<List<string>> outpath_list = Tables[3];
                    if (resource_list.Count > 0)
                    {
                        for (int i = 0; i <= resource_list.Count - 1; i++)
                        {
                            code_res += "RESOURCE @\"" + resource_list[i][0] + "\";\n";
                        }
                    }
                    if (reference_list.Count > 0)
                    {
                        for (int i = 0; i <= reference_list.Count - 1; i++)
                        {
                            code_res += "REFERENCE @\"" + resource_list[i][0] + "\";\n";
                        }
                    }
                    for (int i = 0; i <= inpath_list.Count - 1; i++)
                    {

                        code_res += "#DECLARE input_path" + i.ToString() + " string = @\"" + inpath_list[i][0] + "\";\n";
                    }
                    for (int i = 0; i <= outpath_list.Count - 1; i++)
                    {
                        code_res += "#DECLARE output_path" + i.ToString() + " string = @\"" + outpath_list[i][0] + "\";\n";
                    }
                    table_res.Add(res_table_name);
                    table_res.Add(res_col_name);
                    table_res.Add(res_col_type);
                    break;

                case "EXTRACT":
                    code_res += "Table_" + table_num.ToString() + " = \n\t";

                    res_table_name.Add("Table_" + table_num.ToString());
                    table_res.Add(res_table_name);

                    string extracted_table_name = Tables[0][0][0];
                    List<string> extracted_table_colname = Tables[0][1];
                    List<string> extracted_table_coltype = Tables[0][2];

                    code_res += "EXTRACT ";
                    for (int i = 0; i <= extracted_table_colname.Count - 2; i++)
                    {
                        if (i == 0)
                        {
                            code_res += extracted_table_colname[i] + ":" + extracted_table_coltype[i] + ",\n";
                            res_col_name.Add(extracted_table_colname[i]);
                            res_col_type.Add(extracted_table_coltype[i]);

                        }
                        else
                        {
                            code_res += "\t\t" + extracted_table_colname[i] + ":" + extracted_table_coltype[i] + ",\n";
                            res_col_name.Add(extracted_table_colname[i]);
                            res_col_type.Add(extracted_table_coltype[i]);
                        }
                    }

                    if (extracted_table_colname.Count == 1)
                    {
                        code_res += extracted_table_colname[extracted_table_colname.Count - 1] + ":" + extracted_table_coltype[extracted_table_colname.Count - 1] + "\n";
                        res_col_name.Add(extracted_table_colname[extracted_table_colname.Count - 1]);
                        res_col_type.Add(extracted_table_coltype[extracted_table_colname.Count - 1]);
                    }
                    else
                    {
                        code_res += "\t\t" + extracted_table_colname[extracted_table_colname.Count - 1] + ":" + extracted_table_coltype[extracted_table_colname.Count - 1] + "\n";
                        res_col_name.Add(extracted_table_colname[extracted_table_colname.Count - 1]);
                        res_col_type.Add(extracted_table_coltype[extracted_table_colname.Count - 1]);
                    }
                    code_res += "\tFROM " + extracted_table_name + "\n\tUSING DefaultTextExtractor;\n";
                    table_res.Add(res_col_name);
                    table_res.Add(res_col_type);
                    break;

                case "FILTER":
                    code_res += "Table_" + table_num.ToString() + " = \n\t";

                    res_table_name.Add("Table_" + table_num.ToString());
                    table_res.Add(res_table_name);

                    string selected_table_name = Tables[0][0][0];
                    List<string> selected_table_colname = Tables[0][1];
                    List<string> selected_table_coltype = Tables[0][2];

                    List<string> cur_select_columns = SelectColumns[0];

                    code_res += "SELECT ";
                    if (cur_select_columns.Count == 0)
                    {
                        code_res += "*" + "\n\tFROM " + selected_table_name;

                        for (int i = 0; i <= selected_table_colname.Count - 1; i++)
                        {

                            res_col_name.Add(selected_table_colname[i]);
                            res_col_type.Add(selected_table_coltype[i]);

                        }

                    }
                    else
                    {
                        for (int i = 0; i <= cur_select_columns.Count - 2; i++)
                        {
                            code_res += cur_select_columns[i] + ", ";
                            res_col_name.Add(cur_select_columns[i]);
                            for (int j = 0; j <= selected_table_colname.Count - 1; j++)
                            {
                                if (selected_table_colname[j] == cur_select_columns[i])
                                {
                                    res_col_type.Add(selected_table_coltype[j]);
                                    break;
                                }
                            }
                        }
                        code_res += cur_select_columns[cur_select_columns.Count - 1] + "\n\tFROM " + selected_table_name;

                        res_col_name.Add(cur_select_columns[cur_select_columns.Count - 1]);
                        for (int j = 0; j <= selected_table_colname.Count - 1; j++)
                        {
                            if (selected_table_colname[j] == cur_select_columns[cur_select_columns.Count - 1])
                            {
                                res_col_type.Add(selected_table_coltype[j]);
                                break;
                            }
                        }
                    }
                    if (FilterCondition != "")
                    {
                        code_res += "\n\tWHERE " + FilterCondition + ";\n";
                    }
                    else
                        code_res += ";\n";
                    table_res.Add(res_col_name);
                    table_res.Add(res_col_type);
                    break;

                case "EXCEPT":
                    code_res += "Table_" + table_num.ToString() + " = \n\t";

                    res_table_name.Add("Table_" + table_num.ToString());
                    table_res.Add(res_table_name);

                    for (int i = 0; i <= Tables.Count - 2; i++)
                    {
                        code_res += "SELECT ";
                        for (int j = 0; j <= SelectColumns[i].Count - 2; j++)
                        {
                            code_res += SelectColumns[i][j] + ", ";
                        }
                        code_res += SelectColumns[i][SelectColumns[i].Count - 1] + "\n\t";
                        code_res += "FROM " + Tables[i][0][0] + "\n\tEXCEPT\n\t";

                    }

                    code_res += "SELECT ";
                    for (int j = 0; j <= SelectColumns[Tables.Count - 1].Count - 2; j++)
                    {
                        code_res += SelectColumns[Tables.Count - 1][j] + ", ";
                    }
                    code_res += SelectColumns[Tables.Count - 1][SelectColumns[Tables.Count - 1].Count - 1] + "\n\t";
                    code_res += "FROM " + Tables[Tables.Count - 1][0][0] + ";\n";

                    for (int j = 0; j <= SelectColumns[0].Count - 1; j++)
                    {
                        res_col_name.Add(SelectColumns[0][j]);
                        for (int k = 0; k <= Tables[0][1].Count - 1; k++)
                        {
                            if (SelectColumns[0][j] == Tables[0][1][k])
                            {
                                res_col_type.Add(Tables[0][2][k]);
                                break;
                            }
                        }
                    }


                    table_res.Add(res_col_name);
                    table_res.Add(res_col_type);
                    break;


                case "UNION":
                    code_res += "Table_" + table_num.ToString() + " = \n\t";

                    res_table_name.Add("Table_" + table_num.ToString());
                    table_res.Add(res_table_name);

                    for (int i = 0; i <= Tables.Count - 2; i++)
                    {
                        code_res += "SELECT ";
                        for (int j = 0; j <= SelectColumns[i].Count - 2; j++)
                        {
                            code_res += SelectColumns[i][j] + ", ";
                        }
                        code_res += SelectColumns[i][SelectColumns[i].Count - 1] + "\n\t";
                        code_res += "FROM " + Tables[i][0][0] + "\n\tUNION\n\t";

                    }

                    code_res += "SELECT ";
                    for (int j = 0; j <= SelectColumns[Tables.Count - 1].Count - 2; j++)
                    {
                        code_res += SelectColumns[Tables.Count - 1][j] + ", ";
                    }
                    code_res += SelectColumns[Tables.Count - 1][SelectColumns[Tables.Count - 1].Count - 1] + "\n\t";
                    code_res += "FROM " + Tables[Tables.Count - 1][0][0] + ";\n";

                    for (int j = 0; j <= SelectColumns[0].Count - 1; j++)
                    {
                        res_col_name.Add(SelectColumns[0][j]);
                        for (int k = 0; k <= Tables[0][1].Count - 1; k++)
                        {
                            if (SelectColumns[0][j] == Tables[0][1][k])
                            {
                                res_col_type.Add(Tables[0][2][k]);
                                break;
                            }
                        }
                    }


                    table_res.Add(res_col_name);
                    table_res.Add(res_col_type);
                    break;



                case "AGGREGATE":
                    code_res += "Table_" + table_num.ToString() + " = \n\t";

                    res_table_name.Add("Table_" + table_num.ToString());
                    table_res.Add(res_table_name);

                    string aggregate_table_name = Tables[0][0][0];
                    List<string> aggregate_table_colname = Tables[0][1];
                    List<string> aggregate_table_coltype = Tables[0][2];

                    code_res += "SELECT ";



                    for (int i = 0; i <= aggregate_table_colname.Count - 1; i++)
                    {
                        int flag = 0;
                        for (int j = 0; j <= SelectColumns.Count - 1; j++)
                        {
                            if (aggregate_table_colname[i] == SelectColumns[0][j])
                            {
                                flag = 1;
                                break;
                            }

                        }
                        if (flag == 1)
                        {
                            code_res += SubOperation + "(" + aggregate_table_colname[i] + ") AS " + aggregate_table_colname[i] + "_" + SubOperation.ToLower();
                            res_col_name.Add(aggregate_table_colname[i] + "_" + SubOperation.ToLower());
                            res_col_type.Add(aggregate_table_coltype[i]);
                            break;
                        }

                    }
                    code_res += "\n\tFROM " + aggregate_table_name + ";\n";

                    table_res.Add(res_col_name);
                    table_res.Add(res_col_type);

                    break;

                case "JOIN":
                    code_res += "Table_" + table_num.ToString() + " = \n\t";

                    res_table_name.Add("Table_" + table_num.ToString());
                    table_res.Add(res_table_name);
                    string table1_name = Tables[0][0][0];
                    string table2_name = Tables[1][0][0];

                    List<string> table1_colname = Tables[0][1];
                    List<string> table2_colname = Tables[1][1];


                    List<string> table1_coltype = Tables[0][2];
                    List<string> table2_coltype = Tables[1][2];

                    //rename col if same
                    List<String> table1_code_str = new List<string>();





                    for (int i = 0; i <= table1_colname.Count - 1; i++)
                    {
                        string old_colname = table1_colname[i];
                        string new_colname = "";
                        if (table2_colname.Contains(old_colname) && !Keys[1].Contains(old_colname))
                        {
                            new_colname = table1_name + "_" + old_colname;
                            table1_code_str.Add(table1_name + "." + old_colname + " AS " + new_colname);
                            res_col_name.Add(new_colname);
                            res_col_type.Add(table1_coltype[i]);
                        }
                        else
                        {
                            table1_code_str.Add(table1_name + "." + old_colname);
                            res_col_name.Add(old_colname);
                            res_col_type.Add(table1_coltype[i]);
                        }
                    }

                    List<String> table2_code_str = new List<string>();

                    for (int i = 0; i <= table2_colname.Count - 1; i++)
                    {
                        string old_colname = table2_colname[i];
                        string new_colname = "";
                        if (Keys[1].Contains(old_colname))
                            continue;
                        if (table1_colname.Contains(old_colname))
                        {
                            new_colname = table2_name + "_" + old_colname;
                            table2_code_str.Add(table2_name + "." + old_colname + " AS " + new_colname);
                            res_col_name.Add(new_colname);
                            res_col_type.Add(table2_coltype[i]);
                        }
                        else
                        {
                            table2_code_str.Add(table2_name + "." + old_colname);
                            res_col_name.Add(old_colname);
                            res_col_type.Add(table2_coltype[i]);
                        }
                    }

                    table_res.Add(res_col_name);
                    table_res.Add(res_col_type);

                    code_res += "SELECT ";
                    for (int i = 0; i <= table1_code_str.Count - 1; i++)
                    {
                        code_res += table1_code_str[i] + ",\n\t       ";
                    }

                    for (int i = 0; i <= table2_code_str.Count - 2; i++)
                    {
                        code_res += table2_code_str[i] + ",\n\t       ";
                    }
                    code_res += table2_code_str[table2_code_str.Count - 1] + "\n";
                    code_res += "\tFROM " + table1_name + "\n\t     " + SubOperation + "\n\t     \t" + table2_name + "\n\t     ON ";
                    for (int i = 0; i <= Keys[0].Count - 2; i++)
                    {
                        code_res += table1_name + "." + Keys[0][i] + " == " + table2_name + "." + Keys[1][i] + " AND ";
                    }
                    code_res += table1_name + "." + Keys[0][Keys[0].Count - 1] + " == " + table2_name + "." + Keys[1][Keys[0].Count - 1] + ";\n";

                    break;

                case "PROCESS":
                    string before_table_name = Tables[0][0][0];
                    List<string> after_table_colname = Tables[1][1];
                    List<string> after_table_coltype = Tables[1][2];

                    cs_code_res += "public class " + before_table_name + "Processor : Processor\n{\n\tpublic override Schema Produces(string[] requestedColumns, string[] args, Schema input)\n\t{\n\t\tvar outputSchema = new Schema(\"";
                    for (int i = 0; i <= after_table_colname.Count - 2; i++)
                    {
                        cs_code_res += after_table_colname[i] + ":" + after_table_coltype[i] + ", ";
                    }
                    cs_code_res += after_table_colname[after_table_colname.Count - 1] + ":" + after_table_coltype[after_table_colname.Count - 1] + "\");\n\t\treturn outputSchema;\n\t}\n\n\tpublic override IEnumerable<Row> Process(RowSet input_rowset, Row output_row, string[] args)\n{\n\t\tforeach (Row row in input_rowset.Rows)\n\t\t{\n\n\t\t}\n\t}\n}\n";


                    code_res += "Table_" + table_num.ToString() + " =\n\t" + "PROCESS " + before_table_name + "\n\t" + "PRODUCE ";


                    res_table_name.Add("Table_" + table_num.ToString());
                    table_res.Add(res_table_name);

                    for (int i = 0; i <= after_table_colname.Count - 2; i++)
                    {
                        res_col_name.Add(after_table_colname[i]);
                        res_col_type.Add(after_table_coltype[i]);
                        code_res += after_table_colname[i] + ":" + after_table_coltype[i] + ", ";
                    }
                    res_col_name.Add(after_table_colname[after_table_colname.Count - 1]);
                    res_col_type.Add(after_table_coltype[after_table_colname.Count - 1]);
                    table_res.Add(res_col_name);
                    table_res.Add(res_col_type);
                    code_res += after_table_colname[after_table_colname.Count - 1] + ":" + after_table_coltype[after_table_colname.Count - 1] + "\n\t" + "USING " + before_table_name + "Processor;\n";
                    break;

                case "REDUCE":
                    string before_table_name2 = Tables[0][0][0];
                    List<string> after_table_colname2 = Tables[1][1];
                    List<string> after_table_coltype2 = Tables[1][2];

                    cs_code_res += "public class " + before_table_name2 + "Reducer : Reducer\n{\n\tpublic override Schema Produces(string[] requestedColumns, string[] args, Schema input)\n\t{\n\t\tvar outputSchema = new Schema(\"";
                    for (int i = 0; i <= after_table_colname2.Count - 2; i++)
                    {
                        cs_code_res += after_table_colname2[i] + ":" + after_table_coltype2[i] + ", ";
                    }
                    cs_code_res += after_table_colname2[after_table_colname2.Count - 1] + ":" + after_table_coltype2[after_table_colname2.Count - 1] + "\");\n\t\treturn outputSchema;\n\t}\n\n\tpublic override IEnumerable<Row> Reduce(RowSet input_rowset, Row output_row, string[] args)\n{\n\t\tforeach (Row row in input_rowset.Rows)\n\t\t{\n\n\t\t}\n\t}\n}\n";


                    code_res += "Table_" + table_num.ToString() + " =\n\t" + "REDUCE " + before_table_name2 + "\n\t" + "PRODUCE ";


                    res_table_name.Add("Table_" + table_num.ToString());
                    table_res.Add(res_table_name);

                    for (int i = 0; i <= after_table_colname2.Count - 2; i++)
                    {
                        res_col_name.Add(after_table_colname2[i]);
                        res_col_type.Add(after_table_coltype2[i]);
                        code_res += after_table_colname2[i] + ":" + after_table_coltype2[i] + ", ";
                    }
                    res_col_name.Add(after_table_colname2[after_table_colname2.Count - 1]);
                    res_col_type.Add(after_table_coltype2[after_table_colname2.Count - 1]);
                    table_res.Add(res_col_name);
                    table_res.Add(res_col_type);
                    code_res += after_table_colname2[after_table_colname2.Count - 1] + ":" + after_table_coltype2[after_table_colname2.Count - 1] + "\n\t" + "USING " + before_table_name2 + "Reducer;\n";
                    break;

                case "COMBINE":
                    string before_table_name31 = Tables[0][0][0];
                    string before_table_name32 = Tables[1][0][0];
                    List<string> after_table_colname3 = Tables[2][1];
                    List<string> after_table_coltype3 = Tables[2][2];

                    cs_code_res += "public class " + before_table_name31 + "Combiner : Combiner\n{\n\tpublic override Schema Produces(string[] requestedColumns, string[] args, Schema leftSchema, string leftTable, Schema rightSchema, string rightTable)\n\t{\n\t\tvar outputSchema = new Schema(\"";
                    for (int i = 0; i <= after_table_colname3.Count - 2; i++)
                    {
                        cs_code_res += after_table_colname3[i] + ":" + after_table_coltype3[i] + ", ";
                    }
                    cs_code_res += after_table_colname3[after_table_colname3.Count - 1] + ":" + after_table_coltype3[after_table_colname3.Count - 1] + "\");\n\t\treturn outputSchema;\n\t}\n\n\tpublic override IEnumerable<Row> Combine(RowSet left, RowSet right, Row outputRow, string[] args)\n{\n\t\tforeach (Row rightRow in right.Rows)\n\t\t{\n\n\t\t}\n\t\tforeach (Row leftRow in left.Rows)\n\t\t{\n\n\t\t}\n\t}\n}\n";


                    code_res += "Table_" + table_num.ToString() + " =\n\t" + "COMBINE " + before_table_name31 + " WITH " + before_table_name32 + "\n\t" + "USING " + before_table_name31 + "Combiner;\n";


                    res_table_name.Add("Table_" + table_num.ToString());
                    table_res.Add(res_table_name);

                    for (int i = 0; i <= after_table_colname3.Count - 1; i++)
                    {
                        res_col_name.Add(after_table_colname3[i]);
                        res_col_type.Add(after_table_coltype3[i]);

                    }

                    table_res.Add(res_col_name);
                    table_res.Add(res_col_type);

                    break;

                case "APPLY":
                    code_res += "Table_" + table_num.ToString() + " = \n\t";

                    res_table_name.Add("Table_" + table_num.ToString());
                    table_res.Add(res_table_name);

                    string split_table_name = Tables[0][0][0];
                    List<string> split_table_colname = Tables[0][1];
                    List<string> split_table_coltype = Tables[0][2];
                    string choose_colname = SelectColumns[0][0];
                    code_res += "SELECT ";
                    for (int i = 0; i <= split_table_colname.Count - 2; i++)
                    {
                        int flag = 0;
                        if (split_table_colname[i] == choose_colname)
                        {
                            flag = 1;
                        }

                        if (flag == 1)
                        {
                            code_res += "new_" + choose_colname + " AS " + choose_colname + ",\n\t\t";
                            res_col_name.Add(choose_colname);
                            res_col_type.Add("string");
                        }
                        else
                        {
                            code_res += split_table_colname[i] + ",\n\t\t";
                            res_col_name.Add(split_table_colname[i]);
                            res_col_type.Add(split_table_coltype[i]);
                        }
                    }

                    int new_flag = 0;

                    if (split_table_colname[split_table_colname.Count - 1] == choose_colname)
                    {
                        new_flag = 1;

                    }
                    if (new_flag == 1)
                    {
                        code_res += "new_" + choose_colname + " AS " + choose_colname;
                        res_col_name.Add(choose_colname);
                        res_col_type.Add("string");
                    }
                    else
                    {
                        code_res += split_table_colname[split_table_colname.Count - 1];
                        res_col_name.Add(split_table_colname[split_table_colname.Count - 1]);
                        res_col_type.Add(split_table_coltype[split_table_colname.Count - 1]);
                    }
                    code_res += "\n\tFROM " + split_table_name + "\n\t\tCROSS APPLY\n\t\t\t" + choose_colname + ".Split(new string[]{ \"" + FilterCondition + "\" }, StringSplitOptions.None) AS new_" + choose_colname + ";\n"; //key.Split(new string[]{ "," }, StringSplitOptions.None) AS new_key
                    table_res.Add(res_col_name);
                    table_res.Add(res_col_type);
                    break;
            }
            List<string> code_list = new List<string>();
            code_list.Add(code_res);
            code_list.Add(cs_code_res);
            table_res.Add(code_list);
            Console.Write(table_res[3][0]);
            Console.Write(table_res[3][1]);

            /* Console.Write("Table structure:\n");
             Console.Write("Table name:" + table_res[0][0]+"\nTable column:\n");
             for (int i = 0; i <= table_res[1].Count - 1; i++)
             {
                 Console.Write(table_res[1][i] + ":" + table_res[2][i] + "\n");
             } */

            return table_res;

        }
    }
}
