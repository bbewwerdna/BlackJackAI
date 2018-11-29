using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CardEventArgs : EventArgs
{
    public int CardIndex { get; private set; }

    public  CardEventArgs(int cardIndex)
    {
        CardIndex = cardIndex;
    }
}