using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int dealersFirstCard = -1;
    int numDecks = 1;
    public int Dealercard;
    public int PlayerCard;
    public int AiCard;
    public int currcount = 0;
    public int trueCount = 0;
    public int error = 0;

    public CardStack player;
    public CardStack dealer;
    public CardStack deck;
    public CardStack AI;

    public Button HitButton;
    public Button StayButton;
    public Button Deal;
    public Button ShowCount;


    List<int> DealerHand = new List<int>();

    List<int> PlayerHand = new List<int>();
    List<int> AiHand = new List<int>();
    List<CardStack> players = new List<CardStack>();
    public bool CountDealerFlippedCard = false;


    public Text winText;
    public Text aiText;
    public Text showCountText;
    public Text playerBet;
    public Text AIBet;
    public Text countDisplay;

    int[,] hardMatrix = new int[,] {      { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                          { 0, 0, 0, 1, 1, 0, 0, 0, 0, 0 },
                                          { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
                                          { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                                          { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                          { 0, 0, 2, 2, 2, 0, 0, 0, 0, 0 },
                                          { 2, 2, 2, 2, 2, 0, 0, 0, 0, 0 },
                                          { 2, 2, 2, 2, 2, 0, 0, 0, 0, 0 },
                                          { 2, 2, 2, 2, 2, 0, 0, 0, 0, 0 },
                                          { 2, 2, 2, 2, 2, 0, 0, 0, 0, 0 },
                                          { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 } };

    int[,] softMatrix = new int[,]{       { 0,0,1,1,1,0,0,0,0,0 },
                                          { 0,0,1,1,1,0,0,0,0,0 },
                                          { 0,0,1,1,1,0,0,0,0,0},
                                          { 0,0,1,1,1,0,0,0,0,0},
                                          { 1,1,1,1,1,0,0,0,0,0},
                                          { 2,1,1,1,1,2,2,0,0,2},
                                          { 2,2,2,2,1,2,2,2,2,2},
                                          { 2,2,2,2,2,2,2,2,2,2}};

    int[,] pairMatrix = new int[,] {         {1,1,1,1,1,1,0,0,0,0 },
                                          {1,1,1,1,1,1,1,0,0,0},
                                          { 0,0,1,1,1,0,0,0,0,0},
                                          { 1,1,1,1,1,1,0,0,0,0},
                                          { 1,1,1,1,1,1,1,0,2,0},
                                          { 1,1,1,1,1,1,1,1,1,1},
                                          { 1,1,1,1,1,2,1,1,0,0},
                                          { 1,1,1,1,1,1,1,1,1,1} };












    public void Hit()
    {
        PlayerCard = deck.Pop();
        player.Push(PlayerCard);
        PlayerHand.Add(PlayerCard % 13 + 2);
        UpdateCount(PlayerCard);
        //player.Hand()
        if (player.HandValue() == 21)
        {
            winText.text = "Blackjack!";
            HitButton.interactable = false;
            StayButton.interactable = false;
            if (players.Count < 3)
            {
                DealAgain();
            }
            else
            {
                StartCoroutine(AiTurn(AI));
            }

        }
        if (player.HandValue() > 21)
        {

            //player bust
            HitButton.interactable = false;
            StayButton.interactable = false;
            StartCoroutine(AiTurn(AI));
            
        }
    }

    //called when you press 'STAY'
    public void Stick()
    {
        HitButton.interactable = false;
        StayButton.interactable = false;
        StartCoroutine(AiTurn(AI));
    }

    //check if dealer's hand is 21 before player can hit
    public void DealAgain()
    {
        Deal.interactable = false;
        CountDealerFlippedCard = false;
        player.GetComponent<CardStackView>().Clear();
        dealer.GetComponent<CardStackView>().Clear();
        AI.GetComponent<CardStackView>().Clear();
        winText.text = "";
        //sit.text = "";

        HitButton.interactable = true;
        StayButton.interactable = true;
        dealersFirstCard = -1;
        DealerHand.Clear();
        PlayerHand.Clear(); 
        AiHand.Clear();
        if (deck.CardCount < 15 * numDecks)
        {
            deck.GetComponent<CardStackView>().Clear();
            deck.CreateDeck();
            currcount = 0;
            StartGame();
        }
        else
        {
            StartGame();

        }


    }

    /* allows player to sit or stand while the game goes on*/
    /* public void Sit()
     {
         //removes button from scene
         //Spot1.image.enabled = false;
         //Spot1.interactable = false;

         sit.text = "";
         //puts player at beginning of players list
         players.Insert(0, player);
         //StartGame();
     }*/


    void Start()
    {

        currcount = 0;
        players.Add(player);
        players.Add(AI);
        players.Add(dealer);
        StartGame();
    }


    void StartGame()
    { 
        HitButton.interactable = false;
        StayButton.interactable = false;
        winText.text = "";
        aiText.text = "";
        // sit.text = "sit";
        int numPlayers = players.Count;

        // call betting method for ai bet

        for (int i = 0; i < 2; i++)
        {
            for (int a = 0; a < numPlayers; a++)
            {
                //check to see who is playing and who to deal to
                if (players[a] == player)
                {
                    PlayerCard = deck.Pop();
                    UpdateCount(PlayerCard);
                    player.Push(PlayerCard);
                    PlayerHand.Add(PlayerCard % 13 + 2);

                }
                else if (players[a] == AI)
                {
                    HitAI();
                    Debug.Log("first value: " + AI.HandValue());
                }
                else
                {
                    HitDealer();
                }
            }
        }
        if(player.HandValue()==21 && AI.HandValue() == 21)
        {
            HitButton.interactable = false;
            StayButton.interactable = false;
            StartCoroutine(DealersTurn());
            
        }
        else if(player.HandValue()==21)
        {
            HitButton.interactable = false;
            StayButton.interactable = false;
            StartCoroutine(AiTurn(AI));
        }
        else
        {
            HitButton.interactable = true;
            StayButton.interactable = true;
        }

        // if (players.Count == 2)
        // {
        //    StartCoroutine(AiTurn());
        //  }
        // else
        // {

        // }

    }

    /*public void Betting()
    {

    }*/

    void HitDealer()
    {
        Dealercard = deck.Pop();
        if (CountDealerFlippedCard == true)
        {
            UpdateCount(Dealercard);

        }
        if (dealersFirstCard < 0)
        {
            CountDealerFlippedCard = true;
            dealersFirstCard = Dealercard;
        }
        dealer.Push(Dealercard);
        DealerHand = dealer.Hand();

        // UpdateCount(Dealercard);
        if (dealer.CardCount >= 2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toggle(Dealercard, true);
        }
    }


    public void UpdateCount(int newcard)
    {
        //makes card normal deck number
        newcard = newcard % 13;
        if(newcard<8)
        {

        }



        //if card is a 7,8, or 9, there is no update to card count
        if (newcard == 7 || newcard == 8 || newcard == 9)
        {
            return;
        }
        //if card value is 10-ACE, subtract 1 from count
        //if card value is 2-6, count is incremented by 1 (this is default)
        //adjust true count accordingly 
        switch (newcard)
        {
            case 1:
                currcount -= 1;
                trueCount += currcount / numDecks;
                break;
            case 10:
                currcount -= 1;
                trueCount += currcount / numDecks;
                break;
            case 11:
                currcount -= 1;
                trueCount += currcount / numDecks;
                break;
            case 12:
                currcount -= 1;
                trueCount += currcount / numDecks;
                break;
            case 13:
                currcount -= 1;
                trueCount += currcount / numDecks;
                break;
            default:
                currcount += 1;
                trueCount += currcount / numDecks;
                break;
        }

    }


    public void ShowCountButton()
    {
        showCountText.text = "Show Count";
        ShowCount.enabled = false;
        if (ShowCount.enabled == false)
        {
            showCountText.text = "Hide Count";
            ShowCount.enabled = true;

        }
    }

    public void Update()
    {

        showCountText.text = "" + currcount;

    }
    public IEnumerator DealersTurn()
    {
        int cop = 2;
        HitButton.interactable = false;
        StayButton.interactable = false;
        CardStackView view = dealer.GetComponent<CardStackView>();
        view.Toggle(dealersFirstCard, true);
        view.ShowCards();
        UpdateCount(dealersFirstCard);
        yield return new WaitForSeconds(1f);
        if (player.HandValue() < 22 && AI.HandValue() <22)
        {
            while (dealer.HandValue() < 17)
            {
                HitDealer();
                //Debug.Log("dcard"+ dealer.Hand(cop));
                //Debug.Log("Dcard3 = " + DealerHand[2]);
                yield return new WaitForSeconds(1f);
                cop++;

            }
        }

        if (player.HandValue() > 21 || dealer.HandValue() > player.HandValue() && dealer.HandValue() < 22)
        {
            winText.text = "You lose";
        }
        else if (dealer.HandValue() > 21 || player.HandValue() < 22 && player.HandValue() > dealer.HandValue())
        {
            winText.text = "You win";
        }
        else
        {
            winText.text = "Push";
        }
        if (AI.HandValue() > 21 || dealer.HandValue() > AI.HandValue() && dealer.HandValue() < 22)
        {
            aiText.text = "You lose";
        }
        else if (dealer.HandValue() > 21 || AI.HandValue() < 22 && AI.HandValue() > dealer.HandValue())
        {
            aiText.text = "You win";
        }
        else
        {
            aiText.text = "Push";
        }
        yield return new WaitForSeconds(1f);
        Deal.interactable = true;
    }

    public CardStack getDeck()
    {
        return deck;
    }

    void HitAI()
    {
        AiCard = deck.Pop();
        AI.Push(AiCard);
        AiHand.Add(AiCard);
        UpdateCount(AiCard);

    }

    public bool isInHand()
    {
        bool acein = false;
        for(int g =0; g<AiHand.Count; g++)
        {
            if(AiHand[g]== 11)
            {
                acein = true;
                return acein;
            }
            else
            {
                acein = false;  
                return acein;
            }
        }
        return acein;
    }

    public int aiHandCount;
    public CardStack AI2;

    IEnumerator AiTurn(CardStack bleh)
    {
        yield return new WaitForSeconds(1.5f);
        CardStack curAI = bleh;
        aiHandCount = 0;
        HitButton.interactable = false;
        StayButton.interactable = false;
        int temp;
        int dindex = dealer.Hand()[1] - 2;
        int aindex = 0;
        int graphvalue = 0;
        //Going through basic strategy for blackjack
        //pair in hand
        if (curAI.HandValue() == 21)
        {
            if (aiHandCount > 1)
            {
                StartCoroutine(AiTurn(AI2));
            }
            else
            {
                StartCoroutine(DealersTurn());
            }

        }
        else if (curAI.HandValue() > 21)
        {
            if (aiHandCount > 1)
            {
                StartCoroutine(AiTurn(AI2));
            }
            else
            {
                StartCoroutine(DealersTurn());
            }
        }
        else
        {
            //hard hand
            if ((curAI.Hand()[0] != curAI.Hand()[1]) && !curAI.Hand().Contains(11))
            {
                if (curAI.HandValue() < 8)
                {
                    aindex = 0;
                    graphvalue = hardMatrix[aindex, dindex];
                    if (graphvalue == 0)
                    {

                        HitAI();
                        StartCoroutine(AiTurn(curAI));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else if (graphvalue == 1)
                    {
                        //double
                    }
                    else
                    {
                        if (aiHandCount > 1)
                        {
                            StartCoroutine(AiTurn(AI2));
                        }
                        else
                        {
                            StartCoroutine(DealersTurn());
                        }

                    }
                }
                else if (curAI.HandValue() > 16)
                {
                    aindex = 10;
                    graphvalue = hardMatrix[aindex, dindex];
                    if (graphvalue == 0)
                    {
                        HitAI();
                        StartCoroutine(AiTurn(curAI));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else if (graphvalue == 1)
                    {
                        //double
                    }
                    else
                    {
                        if (aiHandCount > 1)
                        {
                            StartCoroutine(AiTurn(AI2));
                        }
                        else
                        {
                            StartCoroutine(DealersTurn());
                        }
                    }
                }
                else
                {
                    aindex = curAI.HandValue() - 7;
                    graphvalue = hardMatrix[aindex, dindex];
                    if (graphvalue == 0)
                    {
                        HitAI();
                        StartCoroutine(AiTurn(curAI));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else if (graphvalue == 1)
                    {
                        //double
                    }
                    else
                    {
                        if (aiHandCount > 1)
                        {
                            StartCoroutine(AiTurn(AI2));
                        }
                        else
                        {
                            StartCoroutine(DealersTurn());
                        }
                    }
                }
            }
            //pair
            else if (curAI.Hand()[0] == curAI.Hand()[1] && curAI.Hand().Count == 2)
            {
                if (curAI.Hand()[0] < 5 && curAI.HandValue() != 12)
                {
                    aindex = curAI.Hand()[0] - 2;
                    graphvalue = pairMatrix[aindex, dindex];
                    if (graphvalue == 0)
                    {
                        HitAI();
                        StartCoroutine(AiTurn(curAI));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else if (graphvalue == 1)
                    {
                        //split

                        temp = curAI.Hand()[curAI.Hand().Count - 1];

                        AI2.Push(temp);
                        curAI.Hand().Remove(curAI.Hand().Count - 1);
                        curAI.Push(deck.Pop());
                        AI2.Push(deck.Pop());
                        StartCoroutine(AiTurn(curAI));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else
                    {
                        if (aiHandCount > 1)
                        {
                            StartCoroutine(AiTurn(AI2));
                            //yield return new WaitForSeconds(1.5f);
                        }
                        else
                        {
                            StartCoroutine(DealersTurn());
                        }
                    }

                }
                else if (curAI.Hand()[0] > 5 && curAI.Hand()[0] < 10)
                {
                    aindex = curAI.Hand()[0] - 3;
                    graphvalue = pairMatrix[aindex, dindex];
                    if (graphvalue == 0)
                    {
                        HitAI();
                        StartCoroutine(AiTurn(curAI));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else if (graphvalue == 1)
                    {
                        //split

                        temp = curAI.Hand()[curAI.Hand().Count - 1];

                        AI2.Push(temp);
                        curAI.Hand().Remove(curAI.Hand().Count - 1);
                        curAI.Push(deck.Pop());
                        AI2.Push(deck.Pop());
                        StartCoroutine(AiTurn(curAI));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else
                    {

                        if (aiHandCount > 1)
                        {
                            StartCoroutine(AiTurn(AI2));
                            //yield return new WaitForSeconds(1.5f);
                        }
                        else
                        {
                            StartCoroutine(DealersTurn());
                        }
                    }
                }
                else if(curAI.Hand()[0] == 1 || curAI.Hand()[1] == 1)
                {

                    aindex = 7;
                    graphvalue = pairMatrix[aindex, dindex];
                    if (graphvalue == 0)
                    {
                        HitAI();
                        StartCoroutine(AiTurn(curAI));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else if (graphvalue == 1)
                    {
                        //split

                        temp = curAI.Hand()[curAI.Hand().Count - 1];

                        AI2.Push(temp);
                        curAI.Hand().Remove(curAI.Hand().Count - 1);
                        curAI.Push(deck.Pop());
                        AI2.Push(deck.Pop());
                        StartCoroutine(AiTurn(curAI));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else
                    {
                        if (aiHandCount > 1)
                        {
                            StartCoroutine(AiTurn(AI2));
                            //yield return new WaitForSeconds(1.5f);
                        }
                        else
                        {
                            StartCoroutine(DealersTurn());
                        }
                    }
                    
                }
                else
                {
                    if (aiHandCount > 1)
                    {
                        StartCoroutine(AiTurn(AI2));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else
                    {
                        StartCoroutine(DealersTurn());
                    }
                }
            }
            //soft hand
            else if(curAI.Hand().Contains(11))
            {
                if (curAI.HandValue() > 19)
                {
                    aindex = 7;
                    graphvalue = hardMatrix[aindex, dindex];
                    if (graphvalue == 0)
                    {
                        HitAI();
                        StartCoroutine(AiTurn(curAI));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else if (graphvalue == 1)
                    {
                        //double
                    }
                    else
                    {
                        if (aiHandCount > 1)
                        {
                            StartCoroutine(AiTurn(AI2));
                        }
                        else
                        {
                            StartCoroutine(DealersTurn());
                        }
                    }
                }
                else
                {
                    aindex = curAI.HandValue() - 13;
                    graphvalue = hardMatrix[aindex, dindex];
                    if (graphvalue == 0)
                    {
                        HitAI();
                        StartCoroutine(AiTurn(curAI));
                        //yield return new WaitForSeconds(1.5f);
                    }
                    else if (graphvalue == 1)
                    {
                        //double
                    }
                    else
                    {
                        if (aiHandCount > 1)
                        {
                            StartCoroutine(AiTurn(AI2));
                        }
                        else
                        {
                            StartCoroutine(DealersTurn());
                        }
                    }
                }
                yield return new WaitForSeconds(1.5f);
            }
        }

    }
    /*
public int refernceTable()
{
    int stand = 1;
    int split = 2;
    //int double = 3;
    //int check = 4; 
    int betAction = 0;
    // bool ;
    int[] action = { 1, 1, 2, 2, 3, 1, 1, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1 };
    int[] AIhandtot = { 16, 15, 20, 20, 10, 12, 12, 11, 9, 10, 9, 16, 13, 12, 12, 12, 13 };
    int[] AIPerceptDealer = { 10, 10, 5, 6, 10, 3, 2, 1, 2, 1, 7, 9, 2, 4, 5, 6, 3 };
    int[] indexCC = { 0, 4, 5, 4, 4, 2, 3, 1, 1, 4, 3, 5, -1, 0, -2, -1, -2 };



    for (int i = 0; i < 17; i++)
    {
        if (dealer.HandValue() == AIPerceptDealer[i] && ai.HandValue() == AIhandtot[i])
        {

            if (currcount >= indexCC[i])
            {

                betAction = action[i];
            }


        }
    }
    if (AiHand[0] == AiHand[1])
    {
        if ((DealerHand[0] == 5 && currcount == 5) || (DealerHand[0] == 6 && currcount == 4))
            betAction = 2;
        else
            betAction = 1;
    }
    return betAction;



}
}
//implementaion of Illustrious 18 (only 17 shown) 
//Decides if the AI should hit, stay, double down, or split based off of the true count, 
//AI cards, dealer's card, and a set of indexes that were found online
//if the AI has certain total card value (AIhandtot) and dealer has certain card value (AIPerceptDealer) 
//and the true cound is equal to or greater than the index value (indexCC), then the AI should follow the action,
// otherwise, the AI should hit. 
*/
}