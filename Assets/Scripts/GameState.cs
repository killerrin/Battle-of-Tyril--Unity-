using UnityEngine;
using System.Collections;
using System;

public class GameState : MonoBehaviour {
	public enum Game_State
	{
		Playing, 
		Paused,
		GameOver,
		TimeUp
	}

	public Game_State gameState = Game_State.Playing;

	public bool DebugMode = true;

	public GameObject aI;
	public Level level;

	public static System.Random random = new System.Random();
	public static float CRASH_HEALTH_CONST = 10;
	public static float MAX_AI_SPAWN_TIMER = 4; // Seconds

	float aiSpawnTimer;
	public float totalTimeAlive = 0f;

	public int wave = 0;
	public int MaxAIsOnScreen = 120;

	// Use this for initialization
	void Start () {
        AudioManager.me.PlayGame();
	}
	
	// Update is called once per frame
	void Update () {
		if (Player.me.GetComponent<Health>().Alive == false)
		{
            AudioManager.me.Stop();
			gameState = Game_State.GameOver;
			GameObject[] ais = GameObject.FindGameObjectsWithTag("AI");
			foreach (GameObject _ai in ais) Destroy(_ai);
		}

		switch (gameState)
		{
			case Game_State.Playing:
                AudioManager.me.PlayGame();
				PlayingUpdate();
				break;
			case Game_State.Paused:
                AudioManager.me.PlayPause();
				break;
			case Game_State.GameOver:
				level.spriteRenderer.sprite = level.gameOver;
				break;
			case Game_State.TimeUp:
				level.spriteRenderer.sprite = level.timeUp;
				break;
			default:
				break;
		}
	}

	void PlayingUpdate()
	{
		VirtualJoysticks.UpdateJoystick();

		totalTimeAlive += Time.fixedDeltaTime;
		aiSpawnTimer -= Time.fixedDeltaTime;
		if (aiSpawnTimer <= 0)
		{
			aiSpawnTimer = MAX_AI_SPAWN_TIMER;
			SpawnShip(10 + wave);
			++wave;
		}
	}

	void OnGUI()
	{
		switch (gameState)
		{
			case Game_State.Playing:
				GUI.Label(new Rect(10, 10, 200, 50), new GUIContent("Health: " + Player.health.health));
				GUI.Label(new Rect(Screen.width / 2.5f, 10, 200, 50), new GUIContent("Next Wave: " + Math.Round(aiSpawnTimer, 2).ToString()));
				GUI.Label(new Rect(Screen.width - 150, 10, 200, 50), new GUIContent("Waves Survived: " + wave));
				break;
			case Game_State.Paused:
				break;
			case Game_State.GameOver:
				GUI.Label(new Rect(100, 300, 300, 50), new GUIContent("You died. But hey, atleast you managed to survive "+ wave+ "Waves and "+ totalTimeAlive + " Seconds"));
				if (GUI.Button(new Rect(100, 350, 300, 200), new GUIContent("Go back to Main"))) { Application.LoadLevel(1); }
				break;
			case Game_State.TimeUp:
				GUI.Label(new Rect(100, 300, 300, 50), new GUIContent("Congratulations you saved the planet! You managed to survive " + wave + "Waves and " + totalTimeAlive + " Seconds"));
				if (GUI.Button(new Rect(100, 350, 300, 200), new GUIContent("Go back to Main"))) { Application.LoadLevel(1); }
				break;
			default:
				break;
		}

		if (DebugMode)
		{
			if (VirtualJoysticks.LeftThumbstickCenter.HasValue)
			{
				GUI.Label(new Rect(10, Screen.height - 20, 100, 20), new GUIContent(VirtualJoysticks.LeftThumbstickCenter.Value.ToString()));
				GUI.Label(new Rect(130, Screen.height - 20, 100, 20), new GUIContent("ID: " + VirtualJoysticks.LeftID.ToString()));
				GUI.Label(new Rect(225, Screen.height - 20, 100, 20), new GUIContent(VirtualJoysticks.LeftThumbstick.ToString()));
			}
			else
			{
				GUI.Label(new Rect(10, Screen.height - 20, 100, 20), new GUIContent("No Left Thumbstick Center"));
			}

			if (VirtualJoysticks.RightThumbstickCenter.HasValue)
			{
				GUI.Label(new Rect(10, Screen.height - 40, 100, 20), new GUIContent(VirtualJoysticks.RightThumbstickCenter.Value.ToString()));
				GUI.Label(new Rect(130, Screen.height - 40, 100, 20), new GUIContent("ID: " + VirtualJoysticks.RightID.ToString()));
				GUI.Label(new Rect(225, Screen.height - 40, 100, 20), new GUIContent(VirtualJoysticks.RightThumbstick.ToString()));
			}
			else
			{
				GUI.Label(new Rect(10, Screen.height - 40, 100, 20), new GUIContent("No Right Thumbstick Center"));
			}

			int width = 100, height = width;
			foreach (Touch t in VirtualJoysticks.oldTouches)
			{
				Vector2 guiSpace = ConvertScreenSpaceToGuiSpace(t.position);

				GUI.Label(new Rect(guiSpace.x, guiSpace.y, width, height), new GUIContent("Touch Here"));
			}
		}
	}

	void SpawnShip(int num)
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag("AI");
		if (objects.Length >= MaxAIsOnScreen) { return; }

		for (int i = 0; i < num; i++)
		{
			GameObject ai = Instantiate(aI) as GameObject;
			ai.AddComponent<AI>().Setup();
		}
	}

	Vector2 ConvertScreenSpaceToGuiSpace(Vector2 screenspace)
	{
		return new Vector2(screenspace.x, Screen.height - screenspace.y);
	}
}
