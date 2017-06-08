using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HumanPlayer : Player {

    // Use this for initialization
    public PlayerUI UI;

    public HumanPlayer(HumanPlayer p):base(p)
    {

    }

    public void PlayAllBasic()
    {
        StartCoroutine(PlayAllbasic());
    }

    public override void Destroy(Card c, List<Card> loc)
    {
        base.Destroy(c, loc);
        UI.UpdateHand();
    }

    public IEnumerator PlayAllbasic()
    {
        WaitForSeconds s = new WaitForSeconds(0.25f);
        List<Card> Basic = Hand.Where(x => x.Cost == 0).ToList();
        int j = Basic.Count;
        for (int i = 0; i < j; i++)
        {
            PlayCard(Basic[i]);
            yield return s;
        }
    }

    public override void Draw(int i = 1)
    {
        base.Draw(i);
        if (UI != null) { UI.UpdateHand(); }
    }

    public override void Gain(string c, List<Card> loc,bool fromShop=false, int pos = -1)
    {
        base.Gain(c, loc,fromShop ,pos);
        if (loc == Hand) { UI.UpdateHand(); }
    }
    public override void Discard(Card C, List<Card> location)
    {
        base.Discard(C, location);
        UI.UpdateHand();
    }
    public override void Buy(string c)
    {
        base.Buy(c);
        if (GameController.Controller.ShopCards.Keys.Contains(c)) { GameController.Controller.ShopCards[c]--; UI.UpdateShop(); }
    }

    public override void PlayCard(Card C)
    {
        base.PlayCard(C);
        //StartCoroutine(UI.MoveCardToPosition();
        UI.UpdateHand();
    }
    public override bool CurrentPlayer()
    {
        return GameController.Controller.CurrentPhase == GameController.Phase.Play
            || GameController.Controller.CurrentPhase == GameController.Phase.Buy
            || GameController.Controller.CurrentPhase == GameController.Phase.End;
    }

    public override bool InPlayPhase()
    {
        return GameController.Controller.CurrentPhase == GameController.Phase.Play;
    }

    public override void StartSelectionProcess(List<Card> cards, string Caller)
    {
        GameController.Controller.BringUpSelectCard(cards, Card.Get(Caller));
    }
}
