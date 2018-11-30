using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int dealersFirstCard = -1;

    public CardStack player;
    public CardStack dealer;
    public CardStack deck;
    public CardStack AI;
    public Button HitButton;
    public Button StayButton;
    public Button Deal;
    public int Dealercard;
    public int PlayerCard;
    public int AiCard;
    List<int> DealerHand = new List<int>();
    List<int> PlayerHand = new List<int>();
    List<int> AiHand = new List<int>();

    public Button Spot1;
    
    public Text winText;
    public Text sit;
    //public bool button = true;

    List<CardStack> players = new List<CardStack>();
    


    public void Hit()
    {
        player.Push(deck.Pop());
        if(player.HandValue() > 21)
        {

            //player bust
            HitButton.interactable = false;
            StayButton.interactable = false;
            //StartCoroutine(DealersTurn());
            StartCoroutine(AiTurn());
        }
    }
    public void Stick()
    {
        HitButton.interactable = false;
        StayButton.interactable = false;
        // dealer actions
        //StartCoroutine(DealersTurn());
        StartCoroutine(AiTurn());
    }

    public void DealAgain()
    {
        Deal.interactable = false;

        player.GetComponent<CardStackView>().Clear();
        dealer.GetComponent<CardStackView>().Clear();
        winText.text = "";

        HitButton.interactable = true;
        StayButton.interactable = true;
        dealersFirstCard = -1;
        Debug.Log("deck count" + deck.CardCount);
        DealerHand.Clear();
        PlayerHand.Clear();
        AiHand.Clear();
        if(deck.CardCount<15)
        {
            deck.GetComponent<CardStackView>().Clear();
            deck.CreateDeck();
            StartGame();
        }
        else
        {
            StartGame();

        }
        

    }

    /* allows player to sit or stand while the game goes on*/
    public void Sit()
    {
        //removes button from scene
        Spot1.image.enabled = false;
        Spot1.interactable = false;

        sit.text = "";
        //puts player at beginning of players list
        players.Insert(0, player);
        //StartGame();
    }


    void Start()
    {
        StartGame();
    }
    void StartGame()
    {
        //players.Add(player);
        HitButton.interactable = false;
        StayButton.interactable = false;
        players.Add(AI);
        players.Add(dealer);
        sit.text = "sit";
        int numPlayers = players.Count;
        for (int i = 0; i < 2; i++)
        {
            for (int a = 0; a < players.Count; a++)
            {
                //check to see who is playing and who to deal to
                if (players[a] == player)
                {
                    PlayerCard = deck.Pop();
                    player.Push(PlayerCard);
                    PlayerHand.Add(PlayerCard%13);
                }
                else if (players[a] == AI)
                {
                    HitAI();
                }
                else
                {
                    HitDealer();
                }
            }
        }
        Debug.Log("Dcard1 = " + DealerHand[0]);
        Debug.Log("Dcard2 = " + DealerHand[1]);

        Debug.Log("dealer card"+dealer.HandValue());
        if (players.Count == 2)
        {
            StartCoroutine(AiTurn());
        }
        else
        {
            HitButton.interactable = true;
            StayButton.interactable = true;
        }

    }
    void HitDealer()
    {
        Dealercard = deck.Pop();
        if (dealersFirstCard < 0)
        {
            dealersFirstCard = Dealercard;
        }
        dealer.Push(Dealercard);
        DealerHand.Add(Dealercard%13);
        if(dealer.CardCount>=2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toggle(Dealercard, true);
        }

        //Debug.Log("Dcard1 = " + DealerHand[0]);
        //Debug.Log("Dcard2" + DealerHand[1]);
        
    }
    IEnumerator DealersTurn()
    {
        HitButton.interactable = false;
        StayButton.interactable = false;
        CardStackView view = dealer.GetComponent<CardStackView>();
        view.Toggle(dealersFirstCard, true);
        view.ShowCards();
        yield return new WaitForSeconds(1f);
        if (player.HandValue() < 22)
        {
            while (dealer.HandValue() < 17)
            {
                HitDealer();
                yield return new WaitForSeconds(1f);
            }
        }

        if(player.HandValue() > 21 || dealer.HandValue() > player.HandValue() && dealer.HandValue() <22)
        {
            winText.text = "You lose";
        }
        else if(dealer.HandValue() > 21 || player.HandValue() <22 && player.HandValue() > dealer.HandValue())
        {
            winText.text = "You win";
        }
        else
        {
            winText.text = "Push";
        }
        yield return new WaitForSeconds(1f);
        Deal.interactable = true;
    }

    void HitAI()
    {
        AiCard = deck.Pop();
        AI.Push(AiCard);
        AiHand.Add(AiCard%13);
    }
    IEnumerator AiTurn()
    {
        HitButton.interactable = false;
        StayButton.interactable = false;
        //Going through basic strategy for blackjack
        //pair in hand
        if (AiHand[0] == AiHand[1])
        {
            for (int a = 2; a < 11; a++)
            {
                for (int b = 2; b < 10; b++)
                {
                    if(AiHand[0] == b)
                    {

                    }
                }
            }
        }
        //hard hand
        else if (AiHand[0]!=AiHand[1] && ((AiHand[0]!=11 || AiHand[0]!=1) && (AiHand[1]!=11 || AiHand[1]!=1)))
        {
            for (int c = 2; c < 11; c++)
            {
                if(DealerHand[0] == c)
                for (int d = 2; d < 18; d++)
                {
                    if((d>1 && d<9 || d==12) && ((c>1 && c<4) || (c>6 && c<11)
                }
            }
        }
        //soft hand
        else
        {
            for (int e = 2; e < 11; e++)
            {
                for (int f = 2; f < 18; f++)
                {

                }
            }
        }

        

        if(DealerHand[0] == 2 || DealerHand[0] ==3)
        {
            if(AI.HandValue()>12)
            {
                //stay
            }
            else if(AI.HandValue()>1&& AI.HandValue()<9 || AI.HandValue()==12)
            {
                //hit
            }
            else
            {
                //double
            }
        }
        else if(DealerHand[0] == 4)
        {
            if (AI.HandValue() > 11)
            {
                //stay
            }
            else if (AI.HandValue() > 1 && AI.HandValue() < 9)
            {
                //hit
            }
            else
            {
                //double
            }
        }
        else if(DealerHand[0] == 5 || DealerHand[0] == 6)
        {
            if (AI.HandValue() > 11)
            {
                //stay
            }
            else if (AI.HandValue() > 1 && AI.HandValue() < 8)
            {
                //hit
            }
            else
            {
                //double
            }
        }
        else if(DealerHand[0] >6 && DealerHand[0]<10)
        {
            if (AI.HandValue() > 16)
            {
                //stay
            }
            else if (AI.HandValue() > 1 && AI.HandValue() < 10 || AI.HandValue() >11 && AI.HandValue() <17)
            {
                //hit
            }
            else
            {
                //double
            }
        }
        else
        {
            if (AI.HandValue() > 17)
            {
                //stay
            }
            else if (AI.HandValue() > 1 && AI.HandValue() < 11 || AI.HandValue() > 11 && AI.HandValue() < 17)
            {
                //hit
            }
            else
            {
                //double
            }
        }
        yield return new WaitForSeconds(1.5f);
    }
}
