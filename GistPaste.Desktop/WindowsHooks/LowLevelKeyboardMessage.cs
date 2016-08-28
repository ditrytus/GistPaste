using System;

namespace GistPaste.Desktop
{
    class LowLevelKeyboardMessage
    {
        public LowLevelKeyboardMessage(int nCode, IntPtr wParam, IntPtr lParam)
        {
            NCode = nCode;
            WParam = wParam;
            LParam = lParam;
        }

        public int NCode { get; }
        public IntPtr WParam { get; }
        public IntPtr LParam { get; }
    }
}
