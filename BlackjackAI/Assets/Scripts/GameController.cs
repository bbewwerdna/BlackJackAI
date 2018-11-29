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
    public int card;
    List<int> DealerHand = new List<int>();

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
                    player.Push(deck.Pop());
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

            //player.Push(deck.Pop());
            //AI.Push(deck.Pop());
            //HitAI();
            //HitDealer();
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
        card = deck.Pop();
        if (dealersFirstCard < 0)
        {
            dealersFirstCard = card;
        }
        dealer.Push(card);
        DealerHand.Add(card%13);
        if(dealer.CardCount>=2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toggle(card, true);
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
        AI.Push(deck.Pop());
    }
    IEnumerator AiTurn()
    {
        HitButton.interactable = false;
        StayButton.interactable = false;
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

        }
        else if(DealerHand[0] == 5 || DealerHand[0] == 6)
        {

        }
        else if(DealerHand[0] >6 && DealerHand[0]<10)
        {

        }
        else
        {

        }
        yield return new WaitForSeconds(1.5f);
    }
}
