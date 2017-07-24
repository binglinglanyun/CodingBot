using CodingBot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot.Common
{
    public enum EnumContorlType
    {
        RadioBox = 0,
        CheckBox = 1,
        Others = 2
    }

    public class ResponseData
    {
        public string SciptContent;
        public string BotMessage;

        EnumContorlType ControlType;
        public List<TableItem> TableItems;
    }
}
