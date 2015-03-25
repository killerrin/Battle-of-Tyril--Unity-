using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour {
	public bool isPlayerBullet;

	public float Damage = 10f;
	public float Acceleration = 0.4f;
	public Vector2 Velocity = Vector2.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 newPos = new Vector2(transform.position.x, transform.position.y);
		newPos += Velocity * (Time.deltaTime * Acceleration);

		transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		Debug.Log("Bullet Collided with something");

        if (isPlayerBullet)
        {
            if (gameObject.tag == "AI")
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (gameObject.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
	}
}
