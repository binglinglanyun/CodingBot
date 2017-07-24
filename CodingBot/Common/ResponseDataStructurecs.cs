﻿using CodingBot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot.Common
{
    public enum TableOperationType
    {
        None = 0,  // Don't need to opreation any control
        ShowRadioBox = 1,  // Need to show radio box
        ShowCheckBox = 2,  // Need to show check box
        ShowMultiCheckBox = 3,  // Need to show multiple check box
        UpdateDataStatus = 4  // Need to Update Data Status
    }

    public class ResponseData
    {
        /// <summary>
        /// Script content that needs to show
        /// </summary>
        public string SciptContent;

        /// <summary>
        /// Bot message that needs to show
        /// </summary>
        public string BotMessage;

        /// <summary>
        /// Table operation type 
        /// </summary>
        public TableOperationType TableOperation;

        /// <summary>
        /// 1. TableOperationType.ShowRadioBox: TableName in each TableItem should not be empty
        /// 2. TableOperationType.ShowCheckBox/ShowMultiCheckBox/UpdateDataStatus: TableName and TableData in each TableItem should not be empty
        /// </summary>
        public List<TableItem> TableItems;

        public ResponseData()
        {
            this.TableOperation = TableOperationType.None;
            this.TableItems = new List<TableItem>();
        }
    }
}
