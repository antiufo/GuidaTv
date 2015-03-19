using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace antiufo.TvGuide
{
    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {



        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public WindowWrapper(Window window)
        {
            _hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }
}
