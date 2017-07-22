using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingBot.Common
{
    public static class ToolWindowHelper
    {
        public static TWindowType DisplayToolWindow<TWindowType>(Package package, int id, bool create = true) where TWindowType : ToolWindowPane
        {
            TWindowType window = GetToolWindow<TWindowType>(package, id, create);
            if (window == null)
            {
                return null;
            }

            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            return window;
        }

        public static TWindowType GetToolWindow<TWindowType>(Package package, int id, bool create = true) where TWindowType : ToolWindowPane
        {
            TWindowType window = package.FindToolWindow(typeof(TWindowType), id, create) as TWindowType;

            if (TryFindToolWindow<TWindowType>(package, id, create, out window))
            {
                return window;
            }

            return null;
        }

        private static bool TryFindToolWindow<TWindowType>(Package package, int id, bool create, out TWindowType windowPane) where TWindowType : ToolWindowPane
        {
            windowPane = null;
            try
            {
                windowPane = package.FindToolWindow(typeof(TWindowType), id, create) as TWindowType;
            }
            catch { }

            return windowPane != null && windowPane.Frame != null;
        }
    }
}
