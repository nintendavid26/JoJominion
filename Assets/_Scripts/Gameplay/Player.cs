using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MoonSharp.Interpreter;

[Serializable]

[MoonSharpUserData]
public abstract class Player : MonoBehaviour {

    //For Deck 0 is top card;
    public List<Card> Deck,Discarded,Hand,InPlay,Aside=new List<Card>();
    public string Name;
    public int Hp,MaxHp;
    public int Money;
    public Player Enemy;
    public int DmgDealt=0;
    public int BuysLeft = 1;
    public int ActionsLeft=2;
    public Dictionary<string, bool> Flags = new Dictionary<string, bool>();
    public HitEffect hitEffect;
    public CoinEffect coinEffect;
    public Vector2 hitEffSpawnLoc,coinEffSpawnLoc;

    
    public Player(Player p)
    {
        Deck = p.Deck;
        Discarded = p.Discarded;
        Hand = p.Hand;
        InPlay = p.InPlay;
        Aside = p.Aside;
        Hp = p.Hp;
        MaxHp = p.MaxHp;
        Enemy = p.Enemy;
        DmgDealt = p.DmgDealt;
        BuysLeft = p.BuysLeft;
        Flags = p.Flags;

    }

    void Start()
    {
        //Do(() => Debug.Log("Test"), 5);
        this.Do(()=> Gain("Coin",Deck),7);
        this.Do(() => Gain("Punch", Deck), 3);
        //this.Do(() => Gain("Hey Ya", Deck), 10);
        (Deck).Shuffle();
        Draw(5);
        Flags = new Dictionary<string, bool>();

    }

    

    public virtual void Draw(int i)
    {

        for (int j = 0; j < i; j++)
        {
            if (Deck.Count == 0 && Discarded.Count == 0) { return; }
            if (Deck.Count == 0)
            {
               
                Deck = new List<Card>(Discarded);
                Deck.Shuffle();
                Discarded.Clear();
            }
            Card c = Deck[0];
            Hand.Add(Deck[0]);
            Deck.RemoveAt(0);
            c.OnDraw(this);

        }
    }

    public void TakeDamage(int Damage)
    {
        Hp -= Damage;
        if (Hp <= 0)
        {
            Lose();
        }
        HitEffect.Make(Damage, hitEffSpawnLoc, hitEffect);
        Enemy.DmgDealt+=Damage;

    }

    public void GetMoney(int money)
    {
        Money += money;
        CoinEffect.Make(money, hitEffSpawnLoc, coinEffect);
    }

    public void GetActions(int amnt)
    {
        ActionsLeft += amnt;
        //CoinEffect.Make(money, hitEffSpawnLoc, coinEffect);
    }
    public void UseActions(int amnt) {
        ActionsLeft -= amnt;
    }

    public void Heal(int Amount)
    {
        Hp += Amount;
        Hp = Hp > MaxHp ? MaxHp : Hp;
    }

    public void Lose()
    {
        GameController.Controller.WinText.gameObject.SetActive(true);

    }

    public void EndTurn()
    {
        Discarded.AddRange(InPlay);
        InPlay.Clear();
        Discarded.AddRange(Hand);
        Hand.Clear();
        Money = 0;
        int draw = 5;
        if (GetFlag("Draw1Less")) { draw = 4;}
        Draw(draw);
        ResetFlags();
        DmgDealt = 0;
        BuysLeft = 1;
        ActionsLeft = 2;
    }

    public void DiscardHand()
    {

        Discarded.AddRange(Hand);
        Hand.Clear();
    }

    public bool CanPlayCard(Card c)
    {
        return ActionsLeft >= c.ActionCost;
    }

    public void Buy(Card C) {
        Buy(C.name);
    }

    public virtual void Buy(string c) {
        if (Money < Card.Get(c).Cost) { UnityEngine.Debug.Log("Not enough money"); return; }
        if (GetFlag("MayPlaceOnTopOnBuy"))
        {
            int yes=0;
            //bring up buttons
            //wait until pressed
            //yield return new WaitUntil(()=>yes != 0);
            if (yes==1)
            {
                Gain(Card.Get(c).Name, Deck, pos:0);
                return;
            }
        }
        Gain(Card.Get(c).Name, Discarded);
        Money -= Card.Get(c).Cost;
        Card.Get(c).OnBuy(this);
        BuysLeft--;
        if (BuysLeft == 0)
        {
            GameController.Controller.NextPhase();
        }
       
       // yield return null;
    }

    public void EndPhase() {
        GameController.Controller.NextPhase();
    }

    public virtual void Gain(string c, List<Card> loc,bool fromShop=false,int pos=-1)
    {
        Card C;
        C = Instantiate(Card.Get(c), transform);
        if (pos == -1||(pos == 0 && loc.Count == 0))
        {
            loc.Add(C);
        }
        else
        {
            loc.Insert(pos, C);
        }
        if (fromShop&&GameController.Controller.ShopCards.ContainsKey(c)) { GameController.Controller.ShopCards[c]--; }
        //return C;
    }

    public virtual void Discard(Card C,List<Card> location)
    {
        Discarded.Add(C);
        location.Remove(C);
       
    }
    
    public virtual void AddToHand(Card C, List<Card> location)
    {
        Hand.Add(C);
        location.Remove(C);
        C.OnDraw(this);
    }

    

    public void PlayCard(string C)
    {
        
        Card c = Hand.Find(x=>x.Name== C);
        PlayCard(c);

    }

    public virtual void PlayCard(Card C)
    {
        
        if (!InPlayPhase()||!CanPlayCard(C)) { return; }
        InPlay.Add(C);
        Hand.Remove(C);
        C.Use(this);
        if (Hand.Count == 0&&!GameController.Controller.Selector.Active) { GameController.Controller.NextPhase(); }
    }

    public abstract bool CurrentPlayer();

    public abstract bool InPlayPhase();

    public Dictionary<string,int> HandToDict()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        foreach(Card c in Hand)
        {
            if (dict.ContainsKey(c.Name))
            {
                dict[c.Name]++;
            }
            else { dict.Add(c.Name, 1); }
        }
        return dict;
    }

    public virtual void Destroy(Card c, List<Card> loc)
    {
        c.Destroy(this,loc);
        
    }

    public void ResetFlags()
    {
        List<string> keys = Flags.Keys.ToList();
        foreach(string key in keys)
        {
          
           Flags[key] = false;
        }
        
    }
    public void SetFlag(string flag,bool val) {
        if (!Flags.ContainsKey(flag))
        {
            Flags.Add(flag, val);
        }
        else
        {
            Flags[flag] = val;
        }
    }
    public bool GetFlag(string flag) {
        if (Flags == null) { Flags = new Dictionary<string, bool>(); }
        if (!Flags.ContainsKey(flag)) { return false; }
        else { return Flags[flag]; }
    }

    public List<Card> RevealTop(int n) {
        List<Card> Top = new List<Card>();
        
        if (Discarded.Count == 0) { n = Mathf.Min(n, Deck.Count); }
        else if (n > Deck.Count)
        {
            
            Discarded.Shuffle();
            Deck.AddRange(Discarded);
            Discarded.Clear();
        }
        for (int j = 0; j < n; j++)
        {
            if (Deck.Count==j) { return Top; }
            Card c = Deck[j];
            Top.Add(c);
        }
        
        return Top;
    }

    public void Debug(string msg)
    {
        UnityEngine.Debug.Log("LUA:"+msg);
    }

    public bool HasCard(string card,List<Card> loc)
    {
        return loc.Any(x => x.Name == card);
    }
    public void AddDiscardToDeck()
    {

    }
    public abstract void StartSelectionProcess(List<Card> cards,string Caller);

}
