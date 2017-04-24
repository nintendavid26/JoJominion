using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public delegate void SimulatedActionC(Card c);
public delegate void SimulatedAction();
//public delegate void SimulatedAction();
public struct ActionAndCard
{
    public SimulatedActionC TypeOfActionC;
    public SimulatedAction TypeOfAction;

    public Card Card;

    public ActionAndCard(SimulatedActionC Action, Card c)
    {
        TypeOfActionC = Action;
        TypeOfAction = null;
        Card = c;
    }

    public ActionAndCard(SimulatedAction Action)
    {
        TypeOfAction = Action;
        TypeOfActionC = null;
        Card = null;
    }

    public bool UsesCard()
    {
        return Card != null;
    }
}
public class AIPlayer : Player
{

    /*
    I would like for the AI to use a decision tree
    It would have a bunch of gamestates, give each one a value
    then pick the best line of decisions.
    The biggest difficulty would be how to simulate the gamestate.
    One option is going through all of the functions I've made and have them alter a game state's value
    instead of the actual values, this would involve alot of code writing and would likely be very messy
    with GameState.something everywhere. If I can figure out how to make that work then I think it will be fine.
    Also like 90% of this class is just AI, so I think it's fine for non AI things to be in here also.

    I will also make the thinking code into a coroutine so that all of it doesn't happen in one frame
    */

    public struct Decision
    {
        public SimulatedActionC action;
        public GameState result;
        public int score;

        public Decision(SimulatedActionC Action,GameState Result,int Score)
        {
            action = Action;
            result = Result;
            score = Score;
        }

    }
   

    public TreeNode<Decision> DecisionTree;
    public Decision CurrentDecision;


    public AIPlayer(AIPlayer p):base(p)
    {

       
    }

    public IEnumerator BeginMainTurnLoop()
    {
        yield return new WaitForSeconds(0.5f);
        while (CurrentPlayer()) {
            ActionAndCard AC = DecideAction(GameState.current);
            DoAction(AC);
            yield return new WaitForSeconds(0.5f);
        }

    }

    /// <summary>
    /// Given a state, what are all possible actions
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public List<ActionAndCard> PossibleActions(GameState state)
    {
        List<ActionAndCard> actions=new List<ActionAndCard>();
        if (GameController.Controller.CurrentPhase==GameController.Phase.EPlay) {
            for(int i = 0; i < Hand.Count; i++)
            {
                Card c = Hand[i];
                if (c.HasFunction("Use")) {
                    actions.Add(new ActionAndCard(PlayCard,c));
                }
            }
        }
        else if (GameController.Controller.CurrentPhase == GameController.Phase.EBuy)
        {

            foreach(KeyValuePair<string,int> kvp in GameController.Controller.ShopCards)
            {
                bool instock = kvp.Value > 0;
                Card c = Card.Get(kvp.Key);
                bool enoughmoney = Money>=c.Cost;
                if (instock && enoughmoney) {
                    actions.Add(new ActionAndCard(Buy, c));
                }
            }
            actions=actions.OrderBy(x => x.Card.Cost).ToList();
        }
        actions.Insert(0,new ActionAndCard(EndPhase));

        return actions;
    }


    public ActionAndCard DecideAction(GameState state)
    {
        List<ActionAndCard> actions = PossibleActions(state);
        //For now I'm just going to have it do random stuff until it can't any more
        //I'll add deciision trees later

        if (actions.Count == 1) { return actions[0]; }//Next Phase
        else { return actions.Last(); }
    }

    public void DoAction(ActionAndCard AC)
    {
        if (AC.TypeOfAction == null)
        {
            UnityEngine.Debug.Log("DoAction Called on " + AC.TypeOfActionC.Method + " " + AC.Card.Name);
            AC.TypeOfActionC.Invoke(AC.Card);
        }
        else { AC.TypeOfAction.Invoke(); }
       
    }
    public enum Strategy { Aggro,Control,Other};

    public void InitializeAI()
    {
        //DecideStrategy()
    }

    public int GiveScore(GameState state)
    {
        int x = 0;
        //x=Algorithm();
        return x;
    }

    public Card DecideCardToUse()
    {
        return null;
    }

    public Card DecideCardToBuy()
    {
        return null;
    }

    public Card SelectCard(List<Card> cards, Card card)
    {
        return null;
    }

    public GameState Simulate(GameState state,Card c)
    {
        return new GameState(state);
        //return state.Simulate();
    }


    public override bool CurrentPlayer()
    {

        return GameController.Controller.CurrentPhase == GameController.Phase.EPlay
            || GameController.Controller.CurrentPhase == GameController.Phase.EBuy
            || GameController.Controller.CurrentPhase == GameController.Phase.EEnd;
    }

    public override bool InPlayPhase()
    {
        return GameController.Controller.CurrentPhase == GameController.Phase.EPlay;
    }

    public override void StartSelectionProcess(List<Card> cards, string Caller)
    {
        Card c = Card.Get(Caller);
        GameController.Controller.AIcardInUse = c;
        //card selected=c.HelpAISelect();
        GameController.Controller.AIselectedCard=cards[0];
    }
}
