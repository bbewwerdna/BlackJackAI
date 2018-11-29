using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CardView
{
    public GameObject Card { get; private set; }
    public bool IsFaceUp { get; set; }

    public CardView(GameObject card)
    {
        Card = card;
        IsFaceUp = false;

    }
}

