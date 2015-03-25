using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	private bool alive = true;
	public bool destroyObjectWhenNoHealth = true;
	public float deathTimer = 1.5f;

	public bool Alive
	{
		get { return alive; }
		set { alive = value; }
	}

	public float health;

	public void IncreaseHealth(float _health) { health += _health; }
	public void DecreaseHealth(float _health) 
	{
		health -= _health;

		if (health <= 0) {
			health = 0;

			Alive = false;
			gameObject.active = false;

			if (destroyObjectWhenNoHealth) { Destroy(gameObject, deathTimer); }
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
