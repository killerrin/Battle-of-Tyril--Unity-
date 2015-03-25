using UnityEngine;
using System.Collections;

public class PlayerSelection : MonoBehaviour {
    public Texture2D level1;
    public Texture2D level2;

    int width = 200;
    int height = 150;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void OnGUI () {
        if (GUI.Button(new Rect(100, 100, width, height), new GUIContent("Tyril", level1)))
        {
            Application.LoadLevel(4);
        }

        if (GUI.Button(new Rect(400, 100, width, height), new GUIContent("Sal'Dero", level2)))
        {
            Application.LoadLevel(5);
        }

        if (GUI.Button(new Rect(Screen.width - width, Screen.height - height, width, height), new GUIContent("Go Back")))
        {
            Application.LoadLevel(5);
        }
	}
}
