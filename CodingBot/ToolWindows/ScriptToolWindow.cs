﻿using CodingBot.Controls;
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
    [Guid("AFBED00D-415C-4DE8-8431-00310F9B22DC")]
    public class ScriptToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodingBot"/> class.
        /// </summary>
        public ScriptToolWindow() : base(null)
        {
            this.Caption = "Script Preview";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            AutoGeneratedScriptControl control = new AutoGeneratedScriptControl();
            AutoGeneratedScriptControlViewModel viewModel = new AutoGeneratedScriptControlViewModel();
            control.DataContext = viewModel;
            this.Content = control;
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (this.Content != null)
            {
                this.Content = null;
            }
        }

        public void UpdateScript(string script)
        {
            var control = this.Content as AutoGeneratedScriptControl;
            if (control != null)
            {
                var viewModel = control.DataContext as AutoGeneratedScriptControlViewModel;
                if (viewModel != null)
                {
                    viewModel.UpdateScript(script);
                }
            }
            
        }
    }
}
