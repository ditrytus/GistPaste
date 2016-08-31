using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GistPaste.Desktop.WindowsHooks
{
    class DllImportedKernel32 : IKernel32
    {
        public IntPtr GetModuleHandle(string lpModuleName)
        {
            return Kernel32DllImports.GetModuleHandle(lpModuleName);
        }
    }
}
