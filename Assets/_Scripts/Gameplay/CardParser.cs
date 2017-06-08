using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NCalc;//For string arithmetic
using MoonSharp.Interpreter;
using System.IO;
using System;

public static class CardParser {

    public static void UseEffect(Card card,string FName,Player User)
    {
        if (!ContainsFunction(FName,card.Name)) { return; }
        Script script = Parse(card,User);
        try {
            script.Call(script.Globals[FName]);
        }
        catch(Exception e)
        {
            
            Debug.LogError(card.Name+" calling " + FName+" failed."+e + "\n");
        }


        #region backup
        //Keeping this just incase
        /*
        Player Enemy = User.Enemy;
        //Initialize Dictionaries
        Dictionary<string, string> stringSubs=new Dictionary<string, string>();
        Dictionary<string, int> intSubs = new Dictionary<string, int> {
            { "DECKSIZE",User.Deck.Count },
            { "EDECKSIZE",Enemy.Deck.Count },
            { "DISCARDSIZE",User.Discarded.Count },
            { "EDISCARDSIZE",Enemy.Deck.Count },
            { "HANDSIZE",User.Hand.Count },
            { "EHANDSIZE",Enemy.Hand.Count },
        };

        User.InPlay.Add(card);
        User.Hand.Remove(card);


        List<string> lines = card.EffectCode.Split('\n').ToList();
        foreach (string line in lines)
        {
            //Replace Values
            foreach (KeyValuePair<string, string> entry in stringSubs)
            {
                line.Replace(entry.Key, entry.Value);
            }
            foreach (KeyValuePair<string, int> entry in intSubs)
            {
                line.Replace(entry.Key, entry.Value.ToString());
            }
            List<string> words = line.Split(' ').ToList();

            if (words.Contains("in"))
            {
                int IN = words.IndexOf("in");
                string CardName = words[IN - 1];
                string Location = words[IN + 1];
                List<Card> pile=new List<Card>();
                if (Location == "Discard") { pile = User.Discarded; }
                else if (Location == "Hand") { pile = User.Hand; }
                else if (Location == "EDiscard") { pile = User.Discarded; }

                int Amount = pile.FindAll(x => x.Name == CardName).Count;

                words.RemoveRange(IN - 1, 3);
                words.Insert(IN-1,Amount.ToString());
            }


            //Damage N
            if (words[0] == "Damage")
            {
                int Damage = Eval(words[1]);
                Enemy.TakeDamage(Damage);
                Debug.Log("Dealt " + Damage + " damage.");
            }

            //Money N
            else if (words[0] == "Money")
            {
                int Money = Eval(words[1]);
                Debug.Log("Got " + Money + " money.");
                User.GetMoney(Money);
            }

            //Heal N
            else if (words[0] == "Heal")
            {
                int Heal = Eval(words[1]);
                Debug.Log("Healed " + Heal + " Hp.");
                User.Heal(Heal);
            }

            //Gain S
            else if (words[0] == "Gain")
            {
               new MonoBehaviour().Do( ()=>User.Gain(words[1], User.Deck),Eval(words[2]));
            }


            else { Debug.LogError("Error: " + words[0] + " invalid command."); }
        }
        */
        #endregion


    }

    public static List<Card> GetSelectable(Card card,Player User)
    {
        List<Card> Selectable=null;
        Script script = Parse(card, User);
        DynValue val = script.Call(script.Globals["SelectCards"]);
        Selectable = script.Globals.Get("Selectable").ToObject<List<Card>>();
        return Selectable;
    }

    public static void OnSelect(Card card, string FName, Player User,Card c)//Used after a card is selected
    {
        if (!ContainsFunction(FName, card.Name)) { return; }
        Dictionary<string,object> d = new Dictionary<string, object>() { { "Selected", (Card)c } };
        Script script = Parse(card, User,d);
        try {
            script.Call(script.Globals[FName]);
        }
        catch(Exception e)
        {
            Debug.LogError("Error calling " + FName + " in " + card.Name+"\n"+ e);
        }
    }
  



    public static bool ContainsFunction(string FName,string card) {//There's probably a better way to do this
        return File.ReadAllText(Application.streamingAssetsPath + "/Cards/LUA/" + card + ".lua").Contains(FName);
    }

    public static Script Parse(Card card, Player User,Dictionary<string,object> vars=null)
    {
        UserData.RegisterAssembly();
        UserData.RegisterType<List<Card>>();
        //UserData.RegisterType<Debug>();
        Script script = new Script();
        string code = File.ReadAllText(Application.streamingAssetsPath + "/Cards/LUA/" + card.Name + ".lua");
        SetGlobals(script, card, User, vars);
        script.DoString(code);
        return script;
    }

    public static void SetGlobals(Script script, Card card,Player User,Dictionary<string,object>vars =null) {
        script.Globals.Set("User", UserData.Create(User));
        script.Globals.Set("Card", UserData.Create(card));
        script.Globals.Set("Enemy", UserData.Create(User.Enemy));
        script.Globals.Set("Controller", UserData.Create(GameController.Controller));
        script.Globals.Set("Shop", UserData.Create(GameController.Controller.ShopCards.Keys.ToList().ToCards()));
        if (vars != null)
        {
            foreach (KeyValuePair<string, object> kvp in vars)
            {
                script.Globals.Set(kvp.Key, UserData.Create(kvp.Value));
            }
        }
        //script.Globals.Set()
        script.Globals["UserDeckCount"] = User.Deck.Count;
    }

    public static void SetGlobals(Script script, Card card, Player User,GameState state, Dictionary<string, object> vars = null)
    {
        script.Globals.Set("User", UserData.Create(state.Player));
        script.Globals.Set("Card", UserData.Create(card));
        script.Globals.Set("Enemy", UserData.Create(state.Enemy));
        script.Globals.Set("Controller", UserData.Create(state.con));
        script.Globals.Set("Shop", UserData.Create(state.con.ShopCards.Keys.ToList().ToCards()));
        if (vars != null)
        {
            foreach (KeyValuePair<string, object> kvp in vars)
            {
                script.Globals.Set(kvp.Key, UserData.Create(kvp.Value));
            }
        }
        //script.Globals.Set()
        script.Globals["UserDeckCount"] = User.Deck.Count;
    }



    public static Script Parse(Card card, Player User,GameState state, Dictionary<string, object> vars = null)
    {
        UserData.RegisterAssembly();
        UserData.RegisterType<List<Card>>();
        //UserData.RegisterType<Debug>();
        Script script = new Script();
        string code = File.ReadAllText(Application.streamingAssetsPath + "/Cards/LUA/" + card.Name + ".lua");
        #region SetVariables
        script.Globals.Set("User", UserData.Create(User));
        script.Globals.Set("Card", UserData.Create(card));
        script.Globals.Set("Enemy", UserData.Create(User.Enemy));
        script.Globals.Set("Controller", UserData.Create(GameController.Controller));
        script.Globals.Set("Shop", UserData.Create(GameController.Controller.ShopCards.Keys.ToList().ToCards()));
        if (vars != null)
        {
            foreach (KeyValuePair<string, object> kvp in vars)
            {
                script.Globals.Set(kvp.Key, UserData.Create(kvp.Value));
            }
        }
        //script.Globals.Set()
        script.Globals["UserDeckCount"] = User.Deck.Count;
        #endregion
        script.DoString(code);
        return script;
    }

    public static int Eval(string eq)
    {
        Expression e=new Expression(eq);
        return (int)e.Evaluate();
    }

    //Helper funtions for lua scripts



}
