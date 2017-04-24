using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Useful for saving and loading, replays (maybe?), and VERY useful for AI decision trees
/// </summary>
public class GameState:MonoBehaviour {

    public static GameState current;
    public Player Player;
    public Player Enemy;
    public string FileName;
    public int TurnCount;
    public GameController con;
    public GameController.Phase phase;
    [HideInInspector]
    public List<int> ShopCount;
    [HideInInspector]
    public List<string> ShopCards, PlayerHand, PlayerDeck, PlayerDiscard, PlayerFlagS,
                        EnemyHand, EnemyDeck, EnemyDiscard, EnemyFlagS;
    [HideInInspector]
    public int PlayerHP, EnemyHP;
    [HideInInspector]
    public List<bool> PlayerFlagV, EnemyFlagV;

    // Use this for initialization
    
    public GameState(GameState g)
    {
        Player = g.Player;
        Enemy = g.Enemy;
        TurnCount = g.TurnCount;
        ShopCount = new List<int>(g.ShopCount);
        PlayerHand = new List<string>(g.PlayerHand);
        PlayerDeck = new List<string>(g.PlayerDeck);
        PlayerDiscard = new List<string>(g.PlayerDiscard);
        PlayerFlagS = new List<string>(g.PlayerFlagS);
        EnemyHand = new List<string>(g.EnemyHand);
        EnemyDeck = new List<string>(g.EnemyDeck);
        EnemyDiscard = new List<string>(g.EnemyDiscard);
        EnemyFlagS = new List<string>(g.EnemyFlagS);
        PlayerHP = g.PlayerHP;
        EnemyHP = g.EnemyHP;
        phase = g.phase;
        PlayerFlagV = new List<bool>(g.PlayerFlagV);
        EnemyFlagV = new List<bool>(g.EnemyFlagV);



    }

    public GameState()
    {
    }

    void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        //player = new Player(GameController.Controller.player);
    }

	/// <summary>
    /// Save a GameState to a file
    /// </summary>
    /// <param name="FilePath"></param>
    /// <returns></returns>
    public void Save(string FilePath)
    {
       


        ShopCards = GameController.Controller.ShopCards.Keys.ToList();
        ShopCount = GameController.Controller.ShopCards.Values.ToList();
        Player player = GameController.Controller.player;
        Player enemy = GameController.Controller.enemy;

        PlayerHand = player.Hand.Select(x=>x.Name).ToList();
        PlayerDeck = player.Deck.Select(x => x.Name).ToList();
        PlayerDiscard = player.Discarded.Select(x => x.Name).ToList();
        PlayerFlagS = player.Flags.Keys.ToList();
        PlayerFlagV = player.Flags.Values.ToList();
        PlayerHP = player.Hp;

        EnemyHand = enemy.Hand.Select(x => x.Name).ToList();
        EnemyDeck = enemy.Deck.Select(x => x.Name).ToList();
        EnemyDiscard = enemy.Discarded.Select(x => x.Name).ToList();
        EnemyFlagS = enemy.Flags.Keys.ToList();
        EnemyFlagV = enemy.Flags.Values.ToList();
        EnemyHP = enemy.Hp;

        string json = JsonUtility.ToJson(this, true);
        if (FilePath == "") { FilePath = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-sszzz"); }
        FilePath = Application.dataPath + "/SavedGames/" + FilePath + ".json";
        File.WriteAllText(FilePath,json);
        return;
    }

    /// <summary>
    /// Loads a gamestate from a json file
    /// </summary>
    /// <param name="FilePath"></param>
    public static GameState Load(string FilePath)
    {

        FilePath = Application.dataPath + "/SavedGames/" + FilePath + ".json";
        GameState state=new GameState();
        JsonUtility.FromJsonOverwrite(File.ReadAllText(FilePath),state);
        return state;
        
    }

    public void LoadAndSetMain(string FilePath)
    {
        Load(FilePath).SetMain();
    }
    /// <summary>
    /// Sets the current Gamestate to the caller
    /// </summary>
    public void SetMain()
    {
        current = this;
        GameController.Controller.ShopCards.Clear();
        for (int i = 0; i < ShopCards.Count; i++)
        {
            GameController.Controller.ShopCards.Add(ShopCards[i], ShopCount[i]);
        }
        HumanPlayer player = GameController.Controller.player;
        Player enemy = GameController.Controller.enemy;

        player.Hand = PlayerHand.Select(x => Card.Get(x)).ToList();
        player.Deck = PlayerDeck.Select(x => Card.Get(x)).ToList();
        player.Discarded = PlayerDiscard.Select(x => Card.Get(x)).ToList();
        player.Flags.Clear();
        for (int i = 0; i < PlayerFlagS.Count; i++)
        {
            player.SetFlag(PlayerFlagS[i], PlayerFlagV[i]);
        }
        player.Hp = PlayerHP;

        enemy.Hand = EnemyHand.Select(x => Card.Get(x)).ToList();
        enemy.Deck = EnemyDeck.Select(x => Card.Get(x)).ToList();
        enemy.Discarded = EnemyDiscard.Select(x => Card.Get(x)).ToList();
        enemy.Flags.Clear();
        for (int i = 0; i < EnemyFlagS.Count; i++)
        {
            enemy.SetFlag(EnemyFlagS[i], EnemyFlagV[i]);
        }
        enemy.Hp = EnemyHP;
        player.UI.UpdateHand();
        int j = 0;
        foreach(KeyValuePair<string,int> kvp in GameController.Controller.ShopCards)
        {
            player.UI.ShopCards[j].card = kvp.Key;
            j++;
            //player.UI.ShopCards[i]. = kvp.Value;
        }
        
    }

    public GameState Simulate (SimulatedActionC Action,Card C,Player P)
    {
        Action(C);
        return new GameState(this);
    }

}
