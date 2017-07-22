using CodingBot.Controls;
using CodingBot.ViewModels;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot.ToolWindows
{
    [Guid("24EC3268-B73F-47E5-9ACF-ACF3AF8BB205")]
    public class DataStatusToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodingBot"/> class.
        /// </summary>
        public DataStatusToolWindow() : base(null)
        {
            this.Caption = "DataStatus";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new DataStatusControl();
        }

        public void BindInputDataStatus(UserData userData)
        {
            (this.Content as DataStatusControl).BindInputDataStatus(userData);
        }

        public void BindTableDataStatus(List<TableItem> tableItems)
        {
            (this.Content as DataStatusControl).BindTableDataStatus(tableItems);
        }
    }
}
