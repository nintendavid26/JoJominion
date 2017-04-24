using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BiDictionary <T,S> {

    Dictionary<T, S> DT;
    Dictionary<S, T> DS;

    public BiDictionary(){
        if(typeof(T)==typeof(S)){
            throw new System.ArgumentException("The 2 types cannot be the same.");
        }
        else {
            DT = new Dictionary<T, S>();
            DS = new Dictionary<S, T>();
        }   
    }

    public S this[T t]
    {
        get
        {
            if (DT.ContainsKey(t)) { return DT[t]; }
            else { return default(S); }
        }

        set
        {
            if (DT.ContainsKey(t)) { DT[t]=value; DS[value] = t; }
            else { DT.Add(t, value);DS.Add(value, t); }
        }
    }

    public T this[S s]
    {
        get
        {
            if (DS.ContainsKey(s)) { return DS[s]; }
            else { return default(T); }
        }

        set
        {
            if (DS.ContainsKey(s)) { DS[s] = value; DT[value] = s; }
            else { DS.Add(s, value); DT.Add(value,s ); }
        }
    }

    public void Delete(T t)
    {
        if (DT.ContainsKey(t)) { DT.Remove(t); DS.Remove(DT[t]); }
    }
    public void Delete(S s)
    {
        if (DS.ContainsKey(s)) { DS.Remove(s); DT.Remove(DS[s]); }
    }

    public List<T> Ts()
    {
        return DT.Keys.ToList();
    }
    public List<S> Ss()
    {
        return DS.Keys.ToList();
    }



}
