using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

    public CardStack player;
    public CardStack dealer;
    public CardStack deck;
    public CardStack AIplayer;

    public int Dealercard;
    public int PlayerCard;
    public int AiCard;

    List<int> DealerHand = new List<int>();
    List<int> PlayerHand = new List<int>();
    List<int> AiHand = new List<int>();

    public void Start()
    {
        //DealerHand = dealer.Hand();
        //AiHand = AIplayer.Hand();
        //AiTurn();
    }

    void HitAI()
    {
        AiCard = deck.Pop();
        AIplayer.Push(AiCard);
        //AiHand.Add(AiCard % 13 + 2);
        //UpdateCount(AiCard);

    }

    public IEnumerator AiTurn()
    {

        int dc;
        int hv;
        //Going through basic strategy for blackjack
        //pair in hand

        for (dc = 2; dc < 12; dc++)
        {
            if (DealerHand[1] == dc)
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
                        if (AIplayer.HandValue() == hv)
                        {
                            if ((((hv > 1 && hv < 9) || hv == 12) && (dc > 1 && dc < 4))
                            || ((hv > 1 && hv < 9) && dc == 4)
                            || ((hv > 1 && hv < 8) && (dc > 4 && dc < 7))
                            || (((hv > 1 && hv < 10) || (hv > 11 && hv < 17)) && (dc > 6 && dc < 10))
                            || (((hv > 1 && hv < 11) || (hv > 11 && hv < 17)) && (dc > 9 && dc < 12)))
                            {
                                HitAI();
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
                        if (AIplayer.HandValue() == hv)
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
