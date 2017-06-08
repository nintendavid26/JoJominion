using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectCard : MonoBehaviour, IPointerClickHandler{

    public Card C;
    public string Pile;
    public Image I;
    public CardSelector Selector;
    public Text Name;

    public void Set(Card c,CardSelector CS)
    {
        C = c;
        Selector = CS;
        Name.text = c.Name;
        I.sprite = c.sprite();
        GetComponent<Button>().onClick.AddListener(() => Selector.Selected = C);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameController.Controller.ShowCardInfo(C.Name);
        }
    }


}
