using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour, IPointerClickHandler {

    public string card;
    public Text Cost;
    public Text Amt;
    public PlayerUI UI;
    public Image image;
    public RectTransform r;
    public Player buyer;

    public void UpdateDisplay() {
        Card c = Card.Get(card);
        int amt=0;
        try {
            amt = GameController.Controller.ShopCards[card]; }
        catch(Exception e) { Debug.LogError(card + " error"); }
        int cost = c.Cost;
        Cost.text = cost.ToString();
        Amt.text = amt.ToString();
        image.sprite = c.sprite();
        Vector2 R = new Vector2(r.localPosition.x, r.localPosition.y);
        R.y = c.ShopImgY;
        r.localPosition = R;

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Right) {
            GameController.Controller.ShowCardInfo(card);
        }
    }


    public void Buy()
    {
        if (GameController.Controller.ShowingCardInfo) { return; }
        if (GameController.Controller.CurrentPhase == GameController.Phase.Buy&&buyer.Money>=Card.Get(card).Cost&&GameController.Controller.ShopCards[card]>0)
        {
            buyer.Buy(card);
        }
    }

}
