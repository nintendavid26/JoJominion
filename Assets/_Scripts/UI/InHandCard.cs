using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InHandCard : MonoBehaviour, IPointerClickHandler{

    public HumanPlayer Owner;
    public string CardName;
    public int CardAmount;
    public Text Amt;
    public Text DisplayName;
    public SpriteRenderer Renderer;
    public Image i;
    public GameObject info;
    public GameObject Count;
    public Text NameText;

    public void OnMouseDown()
    {
        if (GameController.Controller.CanUseCard(Owner)&&CardName!="")
        {
            Debug.Log("pressed");
            Owner.PlayCard(CardName);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameController.Controller.ShowCardInfo(CardName);
        }
    }

    public void Reset()
    {
        CardAmount = 0;
        CardName = "";
        i.enabled = false;
        Count.SetActive(false);
        NameText.text = "";
    }
    public void Set(string Name,int Amount)
    {
        CardName = Name;
        CardAmount=Amount;
        i.enabled = true;
        i.sprite = Card.Get(Name).sprite();
        //Renderer.enabled = true;
        Count.SetActive(true);
        Amt.text = CardAmount.ToString();
        NameText.text = CardName;
    }

}
