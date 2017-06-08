using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[MoonSharpUserData]
[System.Serializable]
[CreateAssetMenu(menuName ="Card")]
public class Card : ScriptableObject {

    public int Cost;
    public int id;
    public string Name;
    [Multiline]
    public string EffectDes;
    public Action<Player> Effect;
    public bool Collectible;
    public static Dictionary<string, Card> Cards=new Dictionary<string, Card>(); 
    public float ShopImgY= -115;
    public float InfoImgY = 0;
    public float InfoImgH = 375;
    public int ActionCost=0;
    //public float ShopImgH = 395;
    public string[] Tags;
    [Multiline]
    public string WaitDes;

    


    public void Awake()
    {
        //ToJSON();
    }

    public void Use(Player User)
    {
        CardParser.UseEffect(this,"Use", User);
        
    }

    public bool HasFunction(string f)
    {
        return CardParser.ContainsFunction(f, Name);
    }

    public void OnSelect(Player User, Card Option)
    {
        CardParser.OnSelect(this, "OnSelect", User, Option);
    }


    public void AlterCost(Player User)
    {
        CardParser.UseEffect(this, "AlterCost", User);
    }
    public void OnGain(Player Gainer)
    {
        Debug.Log("OnGain");
        CardParser.UseEffect(this, "OnGain", Gainer);
        
    }

    public void OnBuy(Player Buyer)
    {
        if (Buyer.GetComponent<AIPlayer>()) { GameController.Controller.ShopCards[Name]--; }
        CardParser.UseEffect(this, "OnBuy", Buyer);
        if (Buyer.GetFlag("MayPlaceOnTopOnBuy"))
        {
            //
            //
        }
        CardParser.UseEffect(this, "OnGain", Buyer);

    }
    public void OnDraw(Player Drawer)
    {
        CardParser.UseEffect(this, "OnDraw", Drawer);
    }

    public void Destroy(Player Controller,List<Card> Loc)
    {
        GameController.Controller.Destroyed.Add(this);
        Loc.Remove(this);
        CardParser.UseEffect(this, "OnDestroy", Controller);
    }

    public void OnStartBuyPhase(Player Current)
    {
        CardParser.UseEffect(this, "OnStartBuyPhase", Current);
    }

    public void OnEndBuyPhase(Player Current)
    {
        CardParser.UseEffect(this, "OnEndBuyPhase", Current);
    }

    public void SelectCard(Player Current)
    {
        if (CardParser.ContainsFunction("SelectCard", Name))
        {
            List<Card> Selectable = CardParser.GetSelectable(this, Current);
            if (Selectable.Count != 0)
            {
                Current.StartSelectionProcess(Selectable, Name);
            }
        }
    }


    public void FromJSON(string s)
    {
        string FilePath = Application.streamingAssetsPath + "/Cards/JSON/" + s + ".json";
        string json = File.ReadAllText(FilePath);
        try { JsonUtility.FromJsonOverwrite(json, this); }
        catch (Exception e)
        {
            Debug.LogError(s+" has bad JSON\n"+e);
        }
    }
    public void ToJSON()
    {
        Debug.Log("Saved "+Name+" to json");
        string json = JsonUtility.ToJson(this,true);
        File.WriteAllText(Application.streamingAssetsPath + "/Cards/JSON/" + Name+".json", json);
    }

    public Sprite sprite()
    {
        Sprite s= Resources.Load<Sprite>(Name);
        return s;
    }

    public bool HasTag(string tag) {
        return Tags.Contains(tag);
    }

    public static Card Get(string s)
    {
        if (s=="") { Debug.Log("Empty?"); return null; }
        if (Cards.ContainsKey(s))
        {
            return Cards[s];
        }
        else {Debug.Log("Invalid Key "+s); return null; }
    }

    

    //public Card 

}
