using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Health))]
public class AI : MonoBehaviour
{
	enum AIState
	{
		Spawning,
		RandomMovement,
		Chasing
	}
	enum RandomMovementState
	{
		GetRandomPosition,
		MoveToRandomPosition
	}

	private AIState state;
	private RandomMovementState movementState;

	private Sprite texture;

	private Rect sightRange;

	public Health health;

	public GameObject BulletPrefab;

	private Vector2 randMovement = Vector2.zero;
	private Vector2 Velocity = Vector2.zero;

	public int EnemyType { get; private set; }
	private int shootctr = 0; //{ get; set; }
	private int maxShootCtr = 80;

	private bool pathFindX;
	private bool pathFindY;

	private float range;
	public float Acceleration;

	public void Setup()
	{
		int x = GameState.random.Next((int)LeftThumbstick.xMax, (int)RightThumbstick.xMax);
		int y = GameState.random.Next((int)RightThumbstick.yMin, (int)RightThumbstick.yMax);
		transform.position = new Vector3(x, y, transform.position.z);

		health = gameObject.GetComponent<Health>();

		switch (GameState.random.Next(0, 3))
		{
			case 0:
				Acceleration = 0.40f;// 0.70f
				health.health = 80.0f;
				range = 2.5f;
				EnemyType = 0;
				break;
			case 1:
				Acceleration = 0.35f;//0.65f
				health.health = 70.0f;
				range = 2f;
				EnemyType = 1;
				break;
			case 2:
				Acceleration = 0.20f;//0.60f
				health.health = 50.0f;
				range = 1.5f;
				EnemyType = 2;
				break;
			default:
				Acceleration = 0.15f;//0.55f
				health.health = 50.0f;
				range = 1.5f;
				EnemyType = 3;
				break;
		}
		//Debug.Log("Enemy Health: "+health.health);

		Acceleration += 1.20f; // Add on speed
		range -= 0.5f;

		texture = gameObject.GetComponent<SpriteRenderer>().sprite;

		Vector2 position = new Vector2(transform.position.x, transform.position.y);
		sightRange = new Rect((int)(position.x - range), (int)(position.y - range), (int)(range + texture.texture.width + range), (int)(range + texture.texture.height + range));

		shootctr = 0;
		pathFindX = true;
		pathFindY = true;

		randMovement = Vector2.zero;

		state = AIState.RandomMovement;
		movementState = RandomMovementState.GetRandomPosition;

		//player = GameObject.FindGameObjectWithTag("Player");
		//playerScript = player.GetComponent<Player>();
	}
	void Start () {

	}
	void Update () {
		//Debug.Log("Player" + Player.me.transform.position);

		sightRange.x = (int)(transform.position.x - range);
		sightRange.x = (int)(transform.position.y - range);

		switch (state)
		{
			case AIState.Spawning:
				SpawningUpdate();
				break;
			case AIState.RandomMovement:
				RandomMovementUpdate();
				break;
			case AIState.Chasing:
				ChasePlayerUpdate();
				break;
		}

		FireBullet();
	}

	private void SpawningUpdate()
	{
		/// To be added at a later date.
	}
	private void ChasePlayerUpdate()
	{
		Vector2 position = new Vector2(transform.position.x, transform.position.y);

		sightRange = new Rect((int)(position.x - range), (int)(position.y - range), (int)(range + texture.texture.width + range), (int)(range + texture.texture.height + range));
		if (!sightRange.Overlaps(Player.me.CollideRectangle)) { state = AIState.RandomMovement; RandomMovementUpdate(); return; }

		//Vector2 temp = Vector2.zero;

		float x = 0f;
		if (position.x <= Player.me.transform.position.x) { x = +Acceleration; }
		else if (position.x >= Player.me.transform.position.x) { x = -Acceleration; }
		//temp.x = x;

		float y = 0f;
		if (position.y <= Player.me.transform.position.y) { y = +Acceleration; }
		else if (position.y >= randMovement.y) { y = -Acceleration; }
		//temp.y = y;

		Velocity = new Vector2(x, y);
		position += Velocity * (Time.deltaTime * Acceleration);

		transform.position = new Vector3(position.x, position.y, transform.position.z);
	}
	private void RandomMovementUpdate()
	{
		float movementPointRadiusCheck = 25f;
		Vector2 position = new Vector2(transform.position.x, transform.position.y);

		sightRange = new Rect((int)(position.x - range), (int)(position.y - range), (int)(range + texture.texture.width + range), (int)(range + texture.texture.height + range));
		if (sightRange.Overlaps(Player.me.CollideRectangle))
		{
			state = AIState.Chasing;
			movementState = RandomMovementState.GetRandomPosition;
			ChasePlayerUpdate();
			return;
		}

		switch (movementState)
		{
			case RandomMovementState.GetRandomPosition:
				randMovement = new Vector2(GameState.random.Next((int)LeftThumbstick.xMax, (int)RightThumbstick.xMax), GameState.random.Next((int)RightThumbstick.yMin, (int)RightThumbstick.yMax));
				pathFindX = true;
				pathFindY = true;
				movementState = RandomMovementState.MoveToRandomPosition;
				break;

			case RandomMovementState.MoveToRandomPosition:
				if (sightRange.Overlaps(Player.me.CollideRectangle)) { state = AIState.Chasing; ChasePlayerUpdate(); return; }

				Vector2 temp = Vector2.zero;
				if (pathFindX)
				{
					float x = 0f;
					if (position.x <= randMovement.x) { x = +Acceleration; }
					else if (position.x >= randMovement.x) { x = -Acceleration; }

					temp.x = x;

					if (position.x <= (randMovement.x + movementPointRadiusCheck) && position.x >= randMovement.x - movementPointRadiusCheck) { pathFindX = false; }
				}
				else { temp.x = 0f; }
				if (pathFindY)
				{
					float y = 0f;
					if (position.y <= randMovement.y) { y = +Acceleration; }
					else if (position.y >= randMovement.y) { y = -Acceleration; }

					temp.y = y;

					if (position.y <= (randMovement.y + movementPointRadiusCheck) && position.y >= randMovement.y - movementPointRadiusCheck) { pathFindY = false; }
				}
				else { temp.y = 0f; }

				Velocity = temp;
				position += Velocity * (Time.deltaTime * Acceleration);

				if (!pathFindX && !pathFindY) { movementState = RandomMovementState.GetRandomPosition; }

				transform.position = new Vector3(position.x, position.y, transform.position.z);
				break;
		}
	}

	void FireBullet()
	{
		if (shootctr > maxShootCtr)
		{
			if (VirtualJoysticks.RightThumbstick != Vector2.zero)
			{
				float posX = transform.position.x - Player.me.transform.position.x;
				float posY = transform.position.y - Player.me.transform.position.y; // Player.me.transform.position.x
				float movementAngle = Mathf.Atan(posY/posX);

				Vector2 bulletvelocity = new Vector2(Mathf.Cos(movementAngle), Mathf.Sin(movementAngle));

				GameObject _bullet = Instantiate(BulletPrefab, transform.position, transform.rotation) as GameObject;
				_bullet.GetComponent<Bullet>().Velocity = bulletvelocity;
				_bullet.GetComponent<Bullet>().Acceleration = 2f;

				shootctr = 0;
			}
		}
		shootctr++;
	}
	void OnTriggerEnter2D(Collider2D c)
	{
		Debug.Log("AI Trigger Enter");

        try
        {
            if (c.gameObject.tag == "Player")
            {
                Debug.Log("AI Collided with Player");
                health.DecreaseHealth(GameState.CRASH_HEALTH_CONST * 10);
            }

            if (c.gameObject.tag == "PlayerBullet")
            {
                Debug.Log("AI Collided with Player Bullet");
                health.DecreaseHealth(c.gameObject.GetComponent<Bullet>().Damage);
            }
        }
        catch (System.Exception) { }
	}
}
