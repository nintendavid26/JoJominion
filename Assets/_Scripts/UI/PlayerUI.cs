using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public HumanPlayer player;
    public Text HP,EHP,Money,Deck;
    public List<InHandCard> HandButtons;
    public Dictionary<string, int> CardsInHand;
    public List<ShopCard> ShopCards;
    public Button Button;
    public Text currentPhase;
    public Image Discard;

    public struct ButtonOptions
    {
        Text Question;
        List<Button> Buttons;
    }
    public ButtonOptions Buttons;

    public Player Enemy;

    public void Start()
    {
      
    }

    public void UpdateHand()//will need to change so cards in hand don't change order
    {
        CardsInHand = player.HandToDict();
        HandButtons.ForEach(x => x.Reset());
        int i = 0;
        foreach (KeyValuePair<string,int> kvp in CardsInHand)
        {
            HandButtons[i].Set(kvp.Key, kvp.Value);
            i++;
            if (i > HandButtons.Count) { return; }
        }
    }
    public void UpdateShop()
    {
        ShopCards.ForEach(x => x.UpdateDisplay());
    }

    public void OnGUI()
    {
        //Set Text
        HP.text = "Player HP "+player.Hp + "/" + player.MaxHp;
        EHP.text = "Enemy HP " + Enemy.Hp + "/" + Enemy.MaxHp;
        Deck.text = player.Deck.Count.ToString();
        if (player.CurrentPlayer()) { Money.text = "Money: " + player.Money; }
        else { Money.text = "Enemy Money: " + Enemy.Money; }
        currentPhase.text = "Current Phase:\n"+Enum.GetName(typeof(GameController.Phase), GameController.Controller.CurrentPhase);
        if (player.Discarded.Count > 0) { Discard.gameObject.SetActive(true); Discard.sprite = player.Discarded.Last().sprite(); }
        else { Discard.gameObject.SetActive(false); }
        UpdateShop();
        UpdateHand();

        //Set Hand

    }

    public void ShowCardInfo(string card)
    {

    }

    


}
