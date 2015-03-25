using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour {
	public static Player me;

	private Vector2 Velocity = Vector2.zero;

	public int maxShootCtr = 20;
	private int shootctr = 0;
	private float Rotation;

	public static Health health;
	public float Acceleration;
	public GameObject bullet;
	public Rect CollideRectangle;

	void Start () {
		health = gameObject.GetComponent<Health>();
		CollideRectangle = gameObject.GetComponent<SpriteRenderer>().sprite.textureRect;
	}

	void Update () {
		if (me == null) me = this;

		Velocity += VirtualJoysticks.LeftThumbstick * Acceleration;

		Vector2 pos = new Vector2(transform.position.x, transform.position.y); // new Vector2(transform.position.x, transform.position.y);
		pos += Velocity * Time.deltaTime;

		transform.position = new Vector3(pos.x, pos.y, transform.position.z);

		Velocity *= 0.99f;

		// update our ship's rotation based on the left thumbstick
		if (VirtualJoysticks.LeftThumbstick.x != 0 && VirtualJoysticks.LeftThumbstick.y != 0)
		{
			Rotation = Mathf.Atan2(VirtualJoysticks.LeftThumbstick.x, -VirtualJoysticks.LeftThumbstick.y);
			transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 0, Rotation));
		}

		ClampToScreen();

		CollideRectangle.x = transform.position.x;
		CollideRectangle.y = transform.position.y;

		FireBullet();
	}

	void FireBullet()
	{
		if (shootctr > maxShootCtr)
		{
			if (VirtualJoysticks.RightThumbstick != Vector2.zero)
			{
				GameObject _bullet = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
				_bullet.GetComponent<Bullet>().Velocity = VirtualJoysticks.RightThumbstick;
				_bullet.GetComponent<Bullet>().Acceleration = 2f;
				shootctr = 0;
			}
		}
		shootctr++;
	}
	void ClampToScreen()
	{
		if (transform.position.x < LeftThumbstick.xMax)
		{
			//Position.X = -ScreenSize.Width / 2f;
			transform.position = new Vector3(LeftThumbstick.xMax, transform.position.y, transform.position.z);
			if (Velocity.x < 0f)
			{
				//Velocity.X = 0f;
				Velocity = new Vector2(0f, Velocity.y);
			}
		}

		if (transform.position.x > RightThumbstick.xMax)
		{
			//Position.X = ScreenSize.Width / 2f;
			transform.position = new Vector3(RightThumbstick.xMax, transform.position.y, transform.position.z);
			if (Velocity.x > 0f)
			{
				//Velocity.X = 0f;
				Velocity = new Vector2(0f, Velocity.y);
			}
		}

		if (transform.position.y < RightThumbstick.yMin)
		{
			//Position.Y = -ScreenSize.Height / 2f;
			transform.position = new Vector3(transform.position.x, RightThumbstick.yMin, transform.position.z);
			if (Velocity.y < 0f)
			{
				//Velocity.Y = 0f;
				Velocity = new Vector2(Velocity.x, 0f);
			}
		}

		if (transform.position.y > RightThumbstick.yMax)
		{
			//Position.Y = ScreenSize.Height / 2f;
			transform.position = new Vector3(transform.position.x, RightThumbstick.yMax, transform.position.z);
			if (Velocity.y > 0f)
			{
				//Velocity.Y = 0f;
				Velocity = new Vector2(Velocity.x, 0f);
			}
		}
	}
	void OnTriggerEnter2D(Collider2D c)
	{
		Debug.Log("Player Trigger Enter");

		if (c.gameObject.tag == "AI")
		{
			Debug.Log("Player Collided with AI");
			health.DecreaseHealth(GameState.CRASH_HEALTH_CONST);
		}

		if (c.gameObject.tag == "AIBullet")
		{
			Debug.Log("Player Collided with AI Bullet");
			health.DecreaseHealth(c.gameObject.GetComponent<Bullet>().Damage);
		}
	}

}
