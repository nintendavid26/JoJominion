using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System;
using MoonSharp.Interpreter;
using UnityEngine.SceneManagement;

[System.Serializable]
[MoonSharpUserData]

public class GameController : MonoBehaviour {

    public Dictionary<string,int> ShopCards=new Dictionary<string, int>();
    public List<string> Cards=new List<string>();
    public List<Card> Destroyed;
    public static GameController Controller;
    public Card C;
    public HumanPlayer player;
    public Player enemy;
    public bool DoingCard=false;
    public List<Card> AllCards;
    public bool PickingCard=false;
    public CardSelector Selector;
    public InfoCard Info;
    public bool ShowingCardInfo = false;
    public Card AIcardInUse = null;
    public Card AIselectedCard = null;
    public Text WinText;

    public Queue<ActionAndCard> ActionQueue=new Queue<ActionAndCard>();

    public enum Phase {Play,Buy,End,EPlay,EBuy,EEnd};
    public Phase CurrentPhase;
    public void Awake()
    {
        Controller = this;
    }

    public void Start()
    {
        //Get all cards from json
        GetAllFromJson();
        //Get 10 piles
        if (Cards.Count == 10) {
            Cards.ForEach(x => ShopCards.Add(x,10));
        }
        else {
            this.Do(() => AddRandomCardToSupply(2, 4), 5);
            this.Do(() => AddRandomCardToSupply(5, 7), 3);
            this.Do(() => AddRandomCardToSupply(8, 10), 2);
            }
       List<string> shopcards = ShopCards.Keys.ToList().OrderBy(x => Card.Get(x).Cost).ToList();
       for (int i = 0; i < 10; i++)
       {
           player.UI.ShopCards[i].card = Card.Get(shopcards[i]).name;
       }
        player.UI.UpdateShop();
        
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Selector.InUse != null && Selector.Selected != null)
        { 
            Card c=Selector.GetSelected();
            Selector.Selected = null;
            Selector.InUse.OnSelect(player, c);
            if (player.Hand.Count == 0)
            {
                NextPhase();
            }

        }
        if (AIcardInUse != null && AIselectedCard != null)
        {
            AIcardInUse.OnSelect(enemy,AIselectedCard);
            AIselectedCard = null;
            AIcardInUse = null;
        }
        

    }


    public void NextPhase()
    {
        if (CurrentPhase == Phase.EEnd)
        {
            CurrentPhase = Phase.Play;
        }
        else { CurrentPhase++; }

        if (CurrentPhase == Phase.Buy || CurrentPhase == Phase.EBuy) {
            ShopCards.Keys.ToList().ToCards().ForEach(x => x.OnStartBuyPhase(player));
        }
        else if (CurrentPhase == Phase.End || CurrentPhase == Phase.EEnd)
        {
            ShopCards.Keys.ToList().ToCards().ForEach(x => x.OnEndBuyPhase(player));
            CurrentPlayer().EndTurn();
            NextPhase();
        }
        else if( CurrentPhase == Phase.EPlay)
        {
            StartCoroutine(enemy.GetComponent<AIPlayer>().BeginMainTurnLoop());
        }
    }
    public Player CurrentPlayer()
    {
        if (CurrentPhase == Phase.EPlay
           || CurrentPhase == Phase.EBuy
           || CurrentPhase == Phase.EEnd)
        {
            return enemy;
        }
        else return player;
    }
    public void AddRandomCardToSupply(int min, int max)
    {

        //Get random card from all, between min and max, not in supply
        List<Card>available = Card.Cards.Values.Where(c => c.Cost >= min && c.Cost <= max).Where(c => !ShopCards.Any(c2 => c2.Key == c.Name)).ToList();
        Card card=available.RandomItem();
            ShopCards.Add(card.Name, 10);
        
        //Add 10 of it to Supply
       
    }

 

    public void BringUpSelectCard(List<Card> Selectable,Card C)
    {

        Selector.gameObject.SetActive(true);
        PickingCard = true;
        Selector.MakeOptions(Selectable,C);
        PickingCard = false;
    }

    public void Wait(string card)
    {
        PickingCard=true;
    }

    public void ShowCardInfo(string CardName)
    {
        if (ShowingCardInfo) { return; }
        ShowingCardInfo = true;
        Info.gameObject.SetActive(true);
        Info.UpdateDisplay(Card.Get(CardName));
    }

    public bool CanUseCard(HumanPlayer player)
    {
        return !ShowingCardInfo&&player.CurrentPlayer()&&CurrentPhase==Phase.Play&& !Selector.Active;
    }

    public void GetAllFromJson()
    {
        DirectoryInfo d = new DirectoryInfo(Application.streamingAssetsPath + "/Cards/JSON/");
        FileInfo[] f = d.GetFiles();
        foreach (FileInfo file in f)
        {
            if (file.Name.Split('.').Last() == "json")
            {
                Card c = ScriptableObject.CreateInstance<Card>();
                c.FromJSON(file.Name.Split('.')[0]);
                c.name = c.Name;
                //c.ToJSON();
                //ScriptableObjectUtility.CopyAsset(c, Application.dataPath+"/Cards/", c.Name);
                if (!Card.Cards.ContainsKey(c.Name))
                {
                    Card.Cards.Add(c.Name, c);
                }
               
            }
        }
        AllCards = Card.Cards.Values.Where(x=>x.Collectible).ToList();
    }

}
