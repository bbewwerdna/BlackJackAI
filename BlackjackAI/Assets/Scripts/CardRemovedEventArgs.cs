using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CardRemovedEventArgs : EventArgs
{
    public int CardIndex { get; private set; }

    public  CardRemovedEventArgs(int cardIndex)
    {
        CardIndex = cardIndex;
    }
}