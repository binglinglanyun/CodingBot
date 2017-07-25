using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot.ViewModels
{
    public class RowItem
    {
        public RowItem(string columnName, string dataType)
        {
            this.ColumnName = columnName;
            this.DataType = dataType;
        }

        public string ColumnName
        {
            get;
            set;
        }

        public string DataType
        {
            get;
            set;
        }
    }

    public class TableItem
    {
        public TableItem()
        {
            this.TableData = new List<RowItem>();
        }

        public TableItem(List<string> tableName, List<string> columns, List<string> types)
        {
            this.TableName = tableName[0];

            List<RowItem> tableData = new List<RowItem>();

            for (int i = 0; i < columns.Count; i++)
            {
                RowItem rowItem = new RowItem(columns[i], types[i]);
                tableData.Add(rowItem);
            }

            this.TableData = tableData;
        }

        public string TableName
        {
            get;
            set;
        }

        public List<RowItem> TableData
        {
            get;
            set;
        }

        public List<List<string>> ToStringList()
        {
            List<List<string>> stringList = new List<List<string>> { new List<string> { TableName } };

            List<string> columns = new List<string>();
            List<string> types = new List<string>();

            foreach (RowItem rowItem in TableData)
            {
                columns.Add(rowItem.ColumnName);
                types.Add(rowItem.DataType);
            }

            stringList.Add(columns);
            stringList.Add(types);

            return stringList;
        }
    }
}
