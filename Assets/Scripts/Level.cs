using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]
public class Level : MonoBehaviour {
	public SpriteRenderer spriteRenderer;

	public Sprite background;
	public Sprite gameOver;
	public Sprite timeUp;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = background;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
