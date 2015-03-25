using UnityEngine;
using System.Collections;

public class Titlescreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.me.PlayMenu();
	}
	
	// Update is called once per frame
	void OnGUI () {
	    if (GUI.Button(new Rect(100,100,200,100), new GUIContent("Play Game")))
        {
            Application.LoadLevel(3);
        }

        if (GUI.Button(new Rect(100,210,200,100), new GUIContent("Exit Game")))
        {
            Application.Quit();
        }
	}
}
