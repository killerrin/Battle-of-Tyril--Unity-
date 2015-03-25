using UnityEngine;
using System.Collections;

public class RightThumbstick : MonoBehaviour {
    public Texture2D texture;

    public static float xMin = 0f;
    public static float xMax = 8.15f;
    public static float yMin = -4.8f;
    public static float yMax = 4.8f;

    // Use this for initialization
    void Start()
    {
    }

    void OnGUI()
    {
        if (!VirtualJoysticks.RightThumbstickCenter.HasValue)
        {
            GUI.Label(new Rect(300, Screen.height - 40, 100, 20), new GUIContent("Not Updating Right Thumbstick"));
        }
        else
        {
            GUI.Label(new Rect(300, Screen.height - 40, 100, 20), new GUIContent("Updating Right Thumbstick"));

            Vector2 guiSpace = ConvertScreenSpaceToGuiSpace(VirtualJoysticks.RightThumbstickCenter.Value);
            GUI.Label(new Rect(guiSpace.x, guiSpace.y, texture.width, texture.height), new GUIContent(texture));

        }
    }

    Vector2 ConvertScreenSpaceToGuiSpace(Vector2 screenspace)
    {
        return new Vector2(screenspace.x, Screen.height - screenspace.y);
    }
}
