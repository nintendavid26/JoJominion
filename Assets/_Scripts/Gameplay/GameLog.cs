using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLog : MonoBehaviour {

    public static GameLog instance;
    public List<string> Logs;

    public static void Log(string msg)
    {
        instance.Logs.Add(msg);
    }

}
