
using CodingBot.Controls;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot.ToolWindows
{
    [Guid("9ff68ee2-64c8-4223-ade0-9eb6ae79baba")]
    public class ConversationToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodingBot"/> class.
        /// </summary>
        public ConversationToolWindow() : base(null)
        {
            this.Caption = "Conversation";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new ConversationControl();
        }
    }
}
