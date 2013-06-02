using System;

using SdlDotNet.Core;
using SdlDotNet.Input;

namespace DW
{
    class Input
    {
        private Key key=Key.BackQuote;

        public Input()
        {
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(push);
            Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(release);
        }

        public void push(object sender, KeyboardEventArgs e)
        {
            key = e.Key;
        }

        public void release(object sender,KeyboardEventArgs e)
        {
            key = Key.BackQuote;
        }

        public bool equals(Key k)
        {
            if (key == k && key != Key.BackQuote)
                return true;
            return false;
        }
    }
}
