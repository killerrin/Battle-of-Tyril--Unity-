using UnityEngine;
using System.Collections;

public class DestructionSquare : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		Debug.Log("Cube of Desturction Destroys All");
		Destroy(c.gameObject);
	}
}
