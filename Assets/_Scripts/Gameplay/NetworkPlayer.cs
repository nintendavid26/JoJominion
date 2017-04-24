using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Player
{

    public NetworkPlayer(NetworkPlayer p) : base(p)
    {

    }

    // Use this for initialization
    public override bool CurrentPlayer()
    {
        throw new NotImplementedException();
    }

    public override bool InPlayPhase()
    {
        return GameController.Controller.CurrentPhase == GameController.Phase.EPlay;
    }

    public override void StartSelectionProcess(List<Card> cards, string Caller)
    {
        throw new NotImplementedException();
    }
}
