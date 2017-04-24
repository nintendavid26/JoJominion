using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SoundEffects : MonoBehaviour {

    public Dictionary<string, AudioClip> sfx;
    public string test;
    public List<string> names,prevNames;
    public List<AudioClip> sounds,prevSounds;
    public bool Updated;
    
    public void PlaySound(string Sound, float volume = 1,bool RandomPitch=false)
    {
        //Debug.Log(Music.Source.pitch);
        if (PlayerPrefs.GetInt("SFX") == 0) { return; }
        if (sfx == null) { sfx = new Dictionary<string, AudioClip>();
            sfx.FromLists(names, sounds);
        }
       // Debug.Log(sfx["Jump"]);
        try {
            AudioClip s = sfx[Sound];
            if (RandomPitch)
            {
                float r = UnityEngine.Random.Range(0.75f, 1.25f);
                //Debug.Log(r);
                Music.Source.pitch = r;
            }
            Music.PlaySound(s, volume);
            Music.Source.pitch = 1;
        }
        catch (Exception e)
        {
            Debug.Log("Sound Effect "+Sound+" not defined.");
        }
        
    }
   

}

public static class SFXext
{
    public static void PlaySound(this MonoBehaviour g, string soundName, float volume = 1, bool randomPitch = false)
    {
        SoundEffects sfx = g.GetComponent<SoundEffects>();
        if (sfx == null)
        {
            Debug.Log("PlaySound requires SoundEffects Component");
            return;
        }
        sfx.PlaySound(soundName, volume, randomPitch);
    }
}