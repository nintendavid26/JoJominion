using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ExtensionMethods {//Will separate into multiple scripts if it gets really big

    public static Vector3 setX(this Vector3 v,float X)
    {

        return new Vector3(X, v.y,v.z);
        
    }
    public static void Do(this MonoBehaviour m, Action A, int x) //used to do a function x times
    {
        for (int i = 0; i < x; i++)
        {
            A();
        }
    }
    public static T[] SubArray<T>(this T[] data, int index, int length)
    {
        T[] result = new T[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }

    public static int Sum(this int[] a)
    {
        int s = 0;
        foreach (int i in a)
        {
            s += i;
        }
        return s;
    }

    public static int Sum(this List<int> a)
    {
        return a.ToArray().Sum();
    }

    public static T RandomItemConditional<T>(this T[] list, Predicate<T> condition = null)
    {
        return list.ToList().RandomItem(condition);
    }

    public static T RandomItem<T>(this List<T> list,Predicate<T> condition=null)
    {
        List<T> temp = condition==null? list :list.FindAll(condition);
        if (temp.Count == 0) { return default(T); }
        int r = UnityEngine.Random.Range(0, list.Count);
        return temp[r];

    }
    public static Vector3 setY(this Vector3 v, float Y)
    {
        
        return new Vector3(v.x, Y,v.z);
    }
    public static void Next(this Enum e){
       // Enum.GetValues(e);
    }

    //would like to have this in the sfx file but I can't

    public static bool AllSame<T>(this List<T> list,T val)//Can check for given value or any value
    {
        if (list == null)
        {
            return true;
        }
        if (list.Capacity < 1)//Should it return true for 0 or 1 item list?
        {
            return true;
        }
        foreach (T i in list)
        {
            if (!Equals(i,val))
            {
                return false;
            }
        }
        return true;
    }

    public static void SetAll<T>(this List<T> list, T val)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = val;
        }
    }



    public static List<T> RemoveLast<T>(this List<T> list )
    {
        List<T> list2 = list;
        list2.RemoveAt(list2.Count-1);
        return list2;
    }

    public static bool Contains<T>(this T[] a,T val)
    {
        foreach (T t in a)
        {
            if (t.Equals(val)) { return true; }
        }
        return false;
    }

    public static void ToLists<T, S>(this Dictionary<T, S> d , List<T> l1, List<S> l2)
    {
        l1 = new List<T>();
        l2 = new List<S>();
        foreach (KeyValuePair<T, S> pair in d)
        {
            l1.Add(pair.Key);
            l2.Add(pair.Value);
        }
    }
    public static bool FromLists<T, S>(this Dictionary<T, S> d, List<T> l1, List<S> l2)
    {
        if (l1.Count != l2.Count)
        {
            Debug.Log("Error, both lists need to be the same length to convert to dictionary.\nList1 Count=" + l1.Count + " List2 Count=" + l2.Count);
            return false;
        }
        if (l1.Count != l1.Distinct().Count())
        {
            Debug.Log("Error, Can not create dictionary with duplicate keys ");
            return false;
        }
        d.Clear();
        for (int i = 0; i < l1.Count; i++)
        {
            d.Add(l1[i], l2[i]);
        }
        return true;
    }
    
    public static void GetNotified(this MonoBehaviour g,Action Caller,Action OnNotify)
    {
        Action e;
        e = new Action(OnNotify);
        Caller += e;
        Debug.Log("Target="+Caller.Target);
        Debug.Log("Method=" + Caller.Method);
        Debug.Log(Caller.GetInvocationList());
    }
    public static void DontGetNotified(this MonoBehaviour g, Action Caller, Action OnNotify)
    {
        Caller -= OnNotify;
    }

    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static List<Card> ToCards(this List<string> list)
    {
        List<Card> NewList = new List<Card>();
        list.ForEach(x =>
        {
            NewList.Add(Card.Get(x));
        }); 
        return NewList;
    }

    public static T Last<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }
    
}

