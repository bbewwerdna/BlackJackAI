using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int dealersFirstCard = -1;
    int numDecks = 1;
    public CardStack player;
    public CardStack dealer;
    public CardStack deck;
    public CardStack AI;
    public Button HitButton;
    public Button StayButton;
    public Button Deal;
    public Button ShowCount;
    public int Dealercard;
    public int PlayerCard;
    public int AiCard;
    List<int> DealerHand = new List<int>();
    List<int> PlayerHand = new List<int>();
    List<int> AiHand = new List<int>();
    public bool CountDealerFlippedCard = false;
    //public Button Spot1;

    public Text winText;
    public Text showCountText;
    //public bool button = true;


    public int currcount = 0;
    public int trueCount = 0;
    public int error = 0;

    public Text playerBet;
    public Text AIBet;
    public Text countDisplay;
    //public AI ai;
    //public bool button = true;

    List<CardStack> players = new List<CardStack>();



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
            // else
            // {
            //StartCoroutine(AITurn());
            // }

        }
        if (player.HandValue() > 21)
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
        StartCoroutine(AiTurn());
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
        //flag = false;
        //Debug.Log("deck count" + deck.CardCount);
        DealerHand.Clear();
        PlayerHand.Clear();
        //Dealercard = 0;
        // PlayerCard = 0; 
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
        winText.text = "Welcome to Blackjack!";

        currcount = 0;
        players.Add(player);
        // players.Add(AI);
        players.Add(dealer);
        StartGame();
    }


    void StartGame()
    {
        //Deal.interactable = true; 
        HitButton.interactable = false;
        StayButton.interactable = false;
        winText.text = "";
        // sit.text = "sit";
        int numPlayers = players.Count;
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
                //  else if (players[a] == AI)
                // {
                //     HitAI();
                //  }
                else
                {
                    HitDealer();
                }
            }
        }
        Debug.Log("Dcard1 = " + dealer.Hand(0));
        Debug.Log("Dcard2 = " + dealer.Hand(1));

        Debug.Log("dealer card" + dealer.HandValue());
        // if (players.Count == 2)
        // {
        //    StartCoroutine(AiTurn());
        //  }
        // else
        // {
        HitButton.interactable = true;
        StayButton.interactable = true;
        // }
        Debug.Log("num players= " + players.Count);


    }

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
        DealerHand.Add(Dealercard % 13 + 2);

        // UpdateCount(Dealercard);
        if (dealer.CardCount >= 2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toggle(Dealercard, true);
        }

        //Debug.Log("Dcard1 = " + DealerHand[0]);
        //Debug.Log("Dcard2" + DealerHand[1]);

    }


    public void UpdateCount(int newcard)
    {
        //makes card normal deck number
        newcard = newcard % 13 + 2;

        //if newcard is greater than 11, something went wrong, make error=1
        if (newcard > 11)
        {
            error = 1;
            return;
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
        if(ShowCount.enabled == false)
        {
            showCountText.text = "Hide Count";
            ShowCount.enabled = true;

        }
    }

    public void Update()
    {
        showCountText.text = "" + currcount;
    }
    IEnumerator DealersTurn()
    {
        int cop = 2;
        HitButton.interactable = false;
        StayButton.interactable = false;
        CardStackView view = dealer.GetComponent<CardStackView>();
        view.Toggle(dealersFirstCard, true);
        view.ShowCards();
        UpdateCount(dealersFirstCard);
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
            // Debug.Log("dcard3 " + dealer.Hand(2));
            //Debug.Log("dcard4 " + dealer.Hand(3));

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
        yield return new WaitForSeconds(1f);
        Deal.interactable = true;
    }

    void HitAI()
    {
        AiCard = deck.Pop();
        AI.Push(AiCard);
        AiHand.Add(AiCard % 13 + 2);
        UpdateCount(AiCard);

    }



    IEnumerator AiTurn()
    {

        HitButton.interactable = false;
        StayButton.interactable = false;
        int dc;
        int hv;
        //Going through basic strategy for blackjack
        //pair in hand

        for (dc = 2; dc < 12; dc++)
        {
            if (DealerHand[0] == dc)
            {
                //pair
                for (int aiCount = 0; aiCount < AiHand.Count; aiCount++)
                {
                    //if (AI.Hand(aiCount) == 11)
                }
                if (AiHand[0] == AiHand[1] && AiHand.Count == 2)
                {
                    for (hv = 2; hv < 10; hv++)
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
                            else if (AiHand.Count < 3)
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
//implementaion of Illustrious 18 (only 17 shown) 
//Decides if the AI should hit, stay, double down, or split based off of the true count, 
//AI cards, dealer's card, and a set of indexes that were found online
//if the AI has certain total card value (AIhandtot) and dealer has certain card value (AIPerceptDealer) 
//and the true cound is equal to or greater than the index value (indexCC), then the AI should follow the action,
// otherwise, the AI should hit. 
public int refernceTable()
{
    int stand = 1;
    int split = 2;
    int double = 3;
    //int check = 4; 
    int betAction = 0;
   // bool ;
    int[] action = { 1, 1, 2, 2, 3, 1, 1, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1 };
    int[] AIhandtot = { 16, 15, 20, 20, 10, 12, 12, 11, 9, 10, 9, 16, 13, 12, 12, 12, 13 };
    int[] AIPerceptDealer = { 10, 10, 5, 6, 10, 3, 2, 1, 2, 1, 7, 9, 2, 4, 5, 6, 3 };
    int[] indexCC = { 0, 4, 5, 4, 4, 2, 3, 1, 1, 4, 3, 5, -1, 0, -2, -1, -2 };



    for (i = 0; i < 17; i++)
    {
        if (dealer.HandValue() == AIPerceptDealer[i] && AI.HandValue() == AIhandtot[i])
        {

            if (currcount >= index[i])
            {

                betAction = action[i];
            }


        }
    }
    if (AiHand[0] == AIHand[1])
    {
        if ((dealerHand[0] == 5 && currcount == 5) || (dealerHand[0] == 6 && currcount == 4))
            betAction = 2;
        else
            betAction = 1;
    }
    return betAction;



}
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int dealersFirstCard = -1;

    public CardStack player;
    public CardStack dealer;
    public CardStack deck;
    //public CardStack AI;
    public Button HitButton;
    public Button StayButton;
    public Button Deal; 
    public Button Spot1;
    public Button Leave; 
    public int DealerCard;
    public int PlayerCard;
    //public int AiCard;
    public List<int> DealerHand = new List<int>();
    public List<int> PlayerHand = new List<int>();
    // List<int> AiHand = new List<int>();
    bool flag = false; 

    public int currcount = 0;
   //int trueCount = 0;
    public int error = 0;
    public Text winText;
    public Text playerBet;
    public Text AIBet; 
    //public AI ai;
    //public bool button = true;

   public List<CardStack> players = new List<CardStack>();

    void Start()
    {

        players.Add(player);
        //players.Add(AI);
        //players.Add(deck);
        players.Add(dealer);
        StartGame();

    }
    void StartGame()
    {


        // HitButton.interactable = false;
        //StayButton.interactable = false;


        int numPlayers = players.Count;
        for (int i = 0; i < 2; i++)
        {
            for (int a = 0; a < numPlayers; a++)
            {
                //check to see who is playing and who to deal to
                if (players[a] == player)
                {
                    PlayerCard = deck.Pop();
                    player.Push(PlayerCard);
                    PlayerHand.Add(PlayerCard % 13 + 2);
                    UpdateCount(PlayerCard);
                }
                // else if (players[a] == AI)
                // {
                //     HitAI();
                // }
                else
                {
                    HitDealer();
                }
            }
        }
        Debug.Log("Dcard1 = " + dealer.Hand(0));
        Debug.Log("Dcard2 = " + dealer.Hand(1));

        Debug.Log("dealer card" + dealer.HandValue());
        //if (players.Count == 2)
        // {
        // StartCoroutine(AiTurn());
        //  }
        // else
        // {
        HitButton.interactable = true;
        StayButton.interactable = true;
        // }

    }

    public void Hit()
    {
        player.Push(deck.Pop());
        //player.Hand()
        if(player.HandValue() == 21)
        {
            winText.text = "21!";

        }
        if (player.HandValue() > 21)
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
       // AI.GetComponent<CardStackView>().Clear();
       winText.text = "";
        //sit.text = "";

        HitButton.interactable = true;
        StayButton.interactable = true;
        dealersFirstCard = -1;
        flag = false;
        //Debug.Log("deck count" + deck.CardCount);
        DealerHand.Clear();
        PlayerHand.Clear();
       // Dealercard = 0;
       // PlayerCard = 0; 
      //  AiHand.Clear();
        if (deck.CardCount < 15)
        {
            deck.GetComponent<CardStackView>().Clear();
            deck.Creat eDec/;
            StartGame();
        }
        else
        {
            StartGame();

        }


    }

 /*  // allows player to sit or stand while the game goes on
    public void Sit()
    {

        //removes button from scene
        Spot1.image.enabled = false;
        Spot1.GetComponent<Text>().enabled = false;


        Spot1.interactable = false;
        Leave.image.enabled = true;
        Leave.interactable = true;


        HitButton.enabled = true;
        StayButton.enabled = true; //Spot1. = ""; //puts player at beginning of players list
        players.Insert(0, player);
        //StartGame();
    }

    //allows player to sit or stand while the game goes on
    public void LeaveTable()
    {
        //removes button from scene

       
        Leave.image.*enabled = false;
      Leave.interactable = false;
        Spot1.image.enabled = false;
        //Spot1. = "";
        Spot1.interactable = false;

        //puts player at beginning of players list
        players.Remove(player);
        HitButton.enabled = false;
        StayButton.enabled =false;
        //StartGame();
    }
   

    void HitDealer()
    {

        DealerCard = deck.Pop();
        //
        if (flag)
        {
            UpdateCount(DealerCard);
        }
        if (dealersFirstCard < 0)
        {
            flag = true;
            dealersFirstCard = DealerCard;
        }

        dealer.Push(DealerCard);
        DealerHand.Add(DealerCard % 13 + 2);
       
       
        if (dealer.CardCount >= 2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toggle(DealerCard, true);

            //view.ShowCards();
           // yield return new WaitForSeconds(1f);
        }

        //Debug.Log("Dcard1 = " + DealerHand[0]);
        //Debug.Log("Dcard2" + DealerHand[1]);

    }

    IEnumerator DealersTurn()
    {
       // int cop = 2;
        HitButton.interactable = false;
        StayButton.interactable = false;
        CardStackView view = dealer.GetComponent<CardStackView>();
        view.Toggle(dealersFirstCard, true);
        view.ShowCards();
        UpdateCount(dealersFirstCard); //when card is flipped, add it to AI percept yield return new WaitForSeconds(1f);
        if (player.HandValue() < 22)
        {
            while (dealer.HandValue() < 17)
           {
                HitDealer();
                //Debug.Log("dcard"+ dealer.Hand(cop));
                //Debug.Log("Dcard3 = " + DealerHand[2]);
               yield return new WaitForSeconds(1f);
               // cop++;

          }
           // Debug.Log("dcard1 " + dealer.Hand(0));
           // Debug.Log("dcard2 " + dealer.Hand(1));
           // Debug.Log("dcard3 " + dealer.Hand(2));
           // Debug.Log("dcard4 " + dealer.Hand(3));

        }


            
        if(player.HandValue() == 21)
        {
            winText.text = "21!";
        }
        else if (dealer.HandValue() == 21)
        {
            winText.text = "21!";
        }
        else if (player.HandValue() > 21 || (dealer.HandValue() > player.HandValue() && dealer.HandValue() < 22))
        {
            winText.text = "You lose";
        }
        else if (dealer.HandValue() > 21 || (player.HandValue() < 22 && player.HandValue() > dealer.HandValue()))
        {
            winText.text = "You win";
        }
        else
        {
            winText.text = "Push";
        }
        //winText.
        yield return new WaitForSeconds(1f);

        Deal.interactable = true;

       // DealAgain();
         //yield return new WaitForSeconds(1f);
    }

    void UpdateCount(int newcard)
    {
        //makes card normal deck number
        newcard = newcard % 13 + 2;
        //if newcard is greater than 11, something went wrong, make error=1
        if (newcard > 11)
        {
            error = 1;
            return;
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
               // trueCount += count / numDecks();
                break;
            case 10:
                currcount -= 1;
               // trueCount += count / numDecks();
                break;
            case 11:
                currcount -= 1;
                //trueCount += count / numDecks();
                break;
            default:
                currcount += 1;
                //trueCount += count / numDecks();
                break;
        }

        return;

    }

    /* void HitAI()
     {
         AiCard = deck.Pop();
         AI.Push(AiCard);
         AiHand.Add(AiCard % 13 + 2);

     }*/


/* IEnumerator AiTurn()
 {
     HitButton.interactable = false;
     StayButton.interactable = false;
     int dc;
     int hv;
     //Going through basic strategy for blackjack
     //pair in hand
if currcount < -9
doubledown = T;

if(currcount > -3){
    referenceTable();    
    }


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

//implementaion of Illustrious 18 (only 17 shown) 
//Decides if the AI should hit, stay, double down, or split based off of the true count, 
//AI cards, dealer's card, and a set of indexes that were found online
//if the AI has certain total card value (AIhandtot) and dealer has certain card value (AIPerceptDealer) 
//and the true cound is equal to or greater than the index value (indexCC), then the AI should follow the action,
// otherwise, the AI should hit. 
int refernceTable(){
int stand = 1;
int split = 2;
int double = 3; 
//int check = 4; 
int betAction = 0; 
boolean TT; 
int[] action[17] = {1,1,2,2,3,1,1,3,3,3,3,1,1,1,1,1,1};
int[] AIhandtot[17] = {16,15,20,20,10,12,12,11,9,10,9,16,13,12,12,12,13};
int[] AIPerceptDealer[17] = {10,10,5,6,10,3,2,1,2,1,7,9,2,4,5,6,3};
int[]  indexCC[17] = {0,4,5,4,4,2,3,1,1,4,3,5,-1,0,-2,-1,-2};



for(i = 0; i<17; i++){
    if(dealer.HandValue() == AIPerceptDealer[i] && AI.HandValue() == AIhandtot[i]){

               if(currcount >= index[i]){

                        betAction= action[i];
                   }


       }       
    }
    if(AIHand[0]==AIHand[1]){
        if((dealerHand[0] == 5 && currcount == 5) ||(dealerHand[0] == 6 && currcount == 4) )
                betAction = 2;
        else
                betAction = 1; 
   }                   
   return betAction;  



} */
