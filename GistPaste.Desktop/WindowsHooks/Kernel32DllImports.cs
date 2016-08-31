using System;
using System.Runtime.InteropServices;

namespace GistPaste.Desktop
{
    static class Kernel32DllImports
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
