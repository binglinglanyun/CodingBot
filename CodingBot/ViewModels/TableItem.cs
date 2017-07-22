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
    }
}
