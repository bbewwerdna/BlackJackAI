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
    public Button HitButton;
    public Button StayButton;
    public Button Deal;

    public Button Spot1;
    
    public Text winText;
    public bool button = true;


    public void Hit()
    {
        player.Push(deck.Pop());
        if(player.HandValue() > 21)
        {

            //player bust
            HitButton.interactable = false;
            StayButton.interactable = false;
            StartCoroutine(DealersTurn());
        }
    }
    public void Stick()
    {
        HitButton.interactable = false;
        StayButton.interactable = false;
        // dealer actions
        StartCoroutine(DealersTurn());
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
    public void Sit()
    {
        
        if(HitButton.IsActive())
        {

        }
        StartGame();
    }
    void Start()
    {
        //SSit();
        StartGame();
    }
    void StartGame()
    {
        for(int i =0; i<2;i++)
        {
            player.Push(deck.Pop());
            HitDealer();
        }
    }
    void HitDealer()
    {
        int card = deck.Pop();
        if (dealersFirstCard < 0)
        {
            dealersFirstCard = card;
        }
        dealer.Push(card);
        if(dealer.CardCount>=2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toggle(card, true);
        }
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
}
