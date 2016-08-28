using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GistPaste.Desktop
{
    struct KeyboardEvent
    {
        public KeyboardEvent(Key key, MessageTypes message)
        {
            Key = key;
            Message = message;
        }

        public Key Key { get; }
        public MessageTypes Message { get; }
    }
}
