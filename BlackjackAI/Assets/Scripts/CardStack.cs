using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardStack : MonoBehaviour
{
    int numDecks = 1;
    List<int> cards;
    List<int> cardsss = new List<int>();

    public bool isGameDeck;

    public bool HasCards
    {
        get{ return cards != null && cards.Count > 0; }
    }

    public event CardEventHandler CardRemoved;
    public event CardEventHandler CardAdded;
    public int CardCount
    {
        get
        {
            if(cards == null)
            {
                return 0;
            }
            else
            {
                return cards.Count;
            }
        }
    }
    public IEnumerable<int> GetCards()
    {
        foreach (int i in cards)
        {
            yield return i;
        }
    }

    public int Pop()
    {
        int temp = cards[0];
        cards.RemoveAt(0);
        if(CardRemoved != null)
        {
            CardRemoved(this, new CardEventArgs(temp));
        }
        return temp;
    }

    public void Push(int card)
    {
        cards.Add(card);
        if(CardAdded != null)
        {
            CardAdded(this, new CardEventArgs(card));
        }
    }
    public int Hand(int pos)
    {
        cardsss.Clear();
        int aces = 0;
        int total = 0;
        int[] acesindex = new int[4 * numDecks];

        bool[] hasBeenChanged = new bool[4 * numDecks];

        foreach (int card in GetCards())
        {
            //52 cards / 13 = 4 => number of suits of a card per deck
            int cardRank = card % 13;
            //case card value is from 2 - 9
            if (cardRank < 8)
            {
                cardRank += 2;
                cardsss.Add(cardRank);
                total = total + cardRank;
            }
            //case card value is 10
            else if (cardRank >= 8 && cardRank < 12)
            {
                cardRank = 10;
                cardsss.Add(cardRank);
                total = total + cardRank;
            }
            else
            {

                cardRank = 11;
                hasBeenChanged[aces] = false;
                cardsss.Add(cardRank);
                total = total + cardRank;
                acesindex[aces] = card;

                aces++;


            }
            //j = card;
            for (int i = aces; i > 0 && total > 21; i--)
            {
               
                if(hasBeenChanged[i] == false)
                {
                    cardsss[acesindex[i]] = 1;
                    total -= 10;
                    hasBeenChanged[i] = true;
                    break; 
                }


            }
        }

        return cardsss[pos];
    }
    public int HandValue()
    {
        //cardsss.Clear();
        int total = 0;
        int aces = 0;
        foreach(int card in GetCards())
        {
            int cardRank = card % 13;
            //case card value is from 2 - 9
            if(cardRank < 8)
            {
                cardRank += 2;
               
                //Hand(cardRank);
                total = total + cardRank;
            }
            //case card value is 10
            else if(cardRank >=8 && cardRank <12)
            {
                cardRank = 10;
                //Hand(cardRank);
                total = total + cardRank;
            }          
            else
            {
                aces++;
            }
        }
        for(int i =0; i<aces; i++)
        {
            if(total +11 < 22)
            {
                total = total + 11;
                //Hand(11);
            }
            else
            {
                total = total + 1;
                //Hand(1);
            }
        }


        return total;
    }

    public void CreateDeck()
    {
        //int shuffle = 15;
        for (int i = 0; i < numDecks*52; i++)
        {
            cards.Add(i);
        }

        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;
        }
        //CreateDeck();
    }
    //returns number of decks used in the game
   

    public void Reset()
    {
        cards.Clear();
    }

    void Awake () 
    {

        cards = new List<int>();
        if (isGameDeck)
        {
            CreateDeck();
        }
    }
}
