using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour {

    public static MenuScene CurrentScene;

    public static List<MenuScene> Scenes=new List<MenuScene>();
    public Scene test;

    public string Name;
    public bool Active;


    public void Start()
    {
        Scenes.Add(this);
    }

	// Use this for initialization
	public void ChangeMenu(MenuScene scene)
    {
       Debug.Log(scene);
        
       CurrentScene = scene;
       foreach(MenuScene Scene in Scenes)
        {
            Scene.Active = false;
            Scene.gameObject.SetActive(false);
        }

        scene.Active = true;
        scene.gameObject.SetActive(true);
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
