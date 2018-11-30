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
        //player.Hand()
        if(player.HandValue() > 21)
        {

            //player bust
            HitButton.interactable = false;
            StayButton.interactable = false;
            StartCoroutine(DealersTurn());
            //StartCoroutine(AiTurn());
        }
    }
    public void Stick()
    {
        HitButton.interactable = false;
        StayButton.interactable = false;
        // dealer actions
        StartCoroutine(DealersTurn());
        //StartCoroutine(AiTurn());
    }

    public void DealAgain()
    {
        Deal.interactable = false;

        player.GetComponent<CardStackView>().Clear();
        dealer.GetComponent<CardStackView>().Clear();
        AI.GetComponent<CardStackView>().Clear();
        winText.text = "";

        HitButton.interactable = true;
        StayButton.interactable = true;
        dealersFirstCard = -1;
        //Debug.Log("deck count" + deck.CardCount);
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
        players.Add(player);
        players.Add(AI);
        players.Add(dealer);
        StartGame();
    }
    void StartGame()
    {
       
        HitButton.interactable = false;
        StayButton.interactable = false;

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
                    PlayerHand.Add(PlayerCard%13+2);
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
        Debug.Log("Dcard1 = " + dealer.Hand(0));
        Debug.Log("Dcard2 = " + dealer.Hand(1));

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
        DealerHand.Add(Dealercard%13+2);
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
        int cop = 2;
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
                //Debug.Log("dcard"+ dealer.Hand(cop));
                //Debug.Log("Dcard3 = " + DealerHand[2]);
                yield return new WaitForSeconds(1f);
                cop++;

            }
            Debug.Log("dcard1 " + dealer.Hand(0));
            Debug.Log("dcard2 " + dealer.Hand(1));
            Debug.Log("dcard3 " + dealer.Hand(2));
            Debug.Log("dcard4 " + dealer.Hand(3));

        }

        if (player.HandValue() > 21 || dealer.HandValue() > player.HandValue() && dealer.HandValue() <22)
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
        AiHand.Add(AiCard%13+2);

    }
    IEnumerator AiTurn()
    {
        HitButton.interactable = false;
        StayButton.interactable = false;
        int dc;
        int hv;
        //Going through basic strategy for blackjack
        //pair in hand

        for(dc = 2; dc<12; dc++)
        {
            if (DealerHand[0] == dc)
            {
                //pair
                for(int aiCount = 0; aiCount<AiHand.Count;aiCount++)
                {
                    if(AI.Hand(aiCount) == 11)
                }
                if (AiHand[0] == AiHand[1] && AiHand.Count == 2)
                {
                    for(hv = 2; hv<10; hv++)
                    {

                    }
                }
                //hard hand
                else if (AiHand[0] != AiHand[1] && ((AiHand[0] != 11 || AiHand[1] != 11)))
                {
                    for (hv = 5; hv < 21; hv++)
                    {
                        if (AI.HandValue() == hv)
                        {
                            if ((((hv > 1 && hv < 9) || hv == 12) && (dc > 1 && dc < 4))
                            || ((hv > 1 && hv < 9) && dc == 4)
                            || ((hv > 1 && hv < 8) && (dc > 4 && dc < 7))
                            || (((hv > 1 && hv < 10) || (hv > 11 && hv < 17)) && (dc > 6 && dc < 10))
                            || (((hv > 1 && hv < 11) || (hv > 11 && hv < 17)) && (dc > 9 && dc < 12)))
                            {

                                //hit
                                //hv =5
                            }
                            else if ((hv > 12 && (dc > 1 && dc < 4))
                            || (hv > 11 && (dc > 3 && dc < 7))
                            || (hv > 17 && (dc > 6 && dc < 11)))
                            {
                                //stay
                            }
                            else if (AiHand.Count < 3)
                            {
                                //double
                            }
                        }
                    }
                }
                //soft hand
                else
                {
                    for (hv = 13; hv < 21; hv++)
                    {
                        if (AI.HandValue() == hv)
                        {
                            if (((dc > 1 && dc < 4) && hv < 17)
                            || (((dc > 6 && dc < 9) || dc == 11) && hv < 18)
                            || ((dc > 8 && dc < 11) && hv < 19))
                            {
                                //hit
                            }
                            else if ((((dc > 1 && dc < 3) || (dc > 6 && dc < 9) || dc == 11) && hv > 17)
                            || (((dc > 6 && dc < 9) || dc == 11) && hv < 18)
                            || ((dc > 8 && dc < 11) && hv < 19))
                            {
                                //stay
                            }
                            else if(AiHand.Count<3)
                            {
                                //double
                            }
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(1.5f);
    }
}
