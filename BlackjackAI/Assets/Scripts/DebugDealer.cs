using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDealer : MonoBehaviour {

    public CardStack dealer;
    public CardStack player;
    //int[] cards = new int[] {4, 12, 16};

    //int count = 0;

    void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,256,28), "Hit Me!"))
        {
            player.Push(dealer.Pop());
        }
        /*if (GUI.Button(new Rect(10, 10, 256, 28), "Hit Me!"))
        {
            player.Push(cards[count++]);
        }*/
    }
}
