using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour {

    public GameObject Box;
    public Button BaseButton;
    public Text text;
    public Card InUse=null;
    public List<SelectCard> Buttons;
    public List<Card> Cards=null;
    public Card Selected=null;
    public bool Active;
    public Vector3 Initial;
    public float x, y,xOff, yOff,rowWidth;
    public SelectCard BaseSelect;

    
    public void MakeOptions(List<Card> cards,Card inUse)
    {
        if (cards.Count == 1) { Selected = cards[0]; return; }
        Active = true;
        InUse = inUse;
        Cards = cards;
        Vector3 spawn=Initial;
        spawn.x = -xOff * (cards.Count-1)/2;
        text.text = inUse.WaitDes;
        for(int i = 0; i < Cards.Count; i++)
        {
            Card c = Cards[i];
            SelectCard SC = Instantiate(BaseSelect,Camera.main.WorldToScreenPoint(spawn),Quaternion.identity,transform);
            SC.Set(c,this);
            Buttons.Add(SC);
            spawn.x += xOff;
            //if (i == rowWidth) { spawn.x = x;spawn.y += yOff; }


        }
        
        
    }


    public Card GetSelected()
    {
        Active = false;
        for(int i = 0; i < Buttons.Count; i++)
        {

            Destroy(Buttons[i].gameObject);
        }
        Buttons = new List<SelectCard>();
        gameObject.SetActive(false);
        return Selected;
    }

}
