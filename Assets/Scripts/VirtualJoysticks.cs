using UnityEngine;
using System.Collections;

public class VirtualJoysticks : MonoBehaviour {
	private const float maxThumbstickDistance = 50f;    // the distance in screen pixels that represents a thumbstick value of 1f.

	// the current positions of the physical touches
	private static Vector2 leftPosition; 
	private static Vector2 rightPosition;

	// the IDs of the touches we are tracking for the thumbsticks
	private static int leftId = -1;
	private static int rightId = -1;

	/// <summary>
	/// Gets the center position of the left thumbstick.
	/// </summary>
	public static Vector2? LeftThumbstickCenter { get; private set; }

	/// <summary>
	/// Gets the center position of the right thumbstick.
	/// </summary>
	public static Vector2? RightThumbstickCenter { get; private set; }

	public static int LeftID { get { return leftId; } }
	public static int RightID { get { return rightId; } }

	public static Touch[] oldTouches;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public static void UpdateJoystick()
	{
		//Debug.Log("Updating Virtual Thumbsticks");

		Touch? leftTouch = null;
		Touch? rightTouch = null; //TouchLocation? leftTouch = null, rightTouch = null;

		Touch[] touches = Input.touches;
		//TouchCollection touches = TouchPanel.GetState();

		// Examine all the touches to convert them to virtual dpad positions. Note that the 'touches'
		// collection is the set of all touches at this instant, not a sequence of events. The only
		// sequential information we have access to is the previous location for of each touch.
		foreach (var touch in touches)
		{
			if (touch.fingerId == leftId)
			{
				// This is a motion of a left-stick touch that we're already tracking
				leftTouch = touch;
				continue;
			}

			if (touch.fingerId == rightId)
			{
				// This is a motion of a right-stick touch that we're already tracking
				rightTouch = touch;
				continue;
			}

			// We're didn't continue an existing thumbstick gesture; see if we can start a new one.
			//
			// We'll use the previous touch position if possible, to get as close as possible to where
			// the gesture actually began.
			Touch earliestTouch;
			if (!TryGetPreviousLocation(touch, out earliestTouch))
				earliestTouch = touch;

			if (leftId == -1)
			{
				// if we are not currently tracking a left thumbstick and this touch is on the left
				// half of the screen, start tracking this touch as our left stick
				if (earliestTouch.position.x < Screen.width / 2)
				{
					leftTouch = earliestTouch;
					continue;
				}
			}

			if (rightId == -1)
			{
				// if we are not currently tracking a right thumbstick and this touch is on the right
				// half of the screen, start tracking this touch as our right stick
				if (earliestTouch.position.x >= Screen.width / 2)
				{
					rightTouch = earliestTouch;
					continue;
				}
			}
		}

		// if we have a left touch
		if (leftTouch.HasValue)
		{
			// if we have no center, this position is our center
			if (!LeftThumbstickCenter.HasValue)
				LeftThumbstickCenter = leftTouch.Value.position;

			// save the position of the touch
			leftPosition = leftTouch.Value.position;

			// save the ID of the touch
			leftId = leftTouch.Value.fingerId;
		}
		else
		{
			// otherwise reset our values to not track any touches
			// for the left thumbstick
			LeftThumbstickCenter = null;
			leftId = -1;
		}

		// if we have a right touch
		if (rightTouch.HasValue)
		{
			// if we have no center, this position is our center
			if (!RightThumbstickCenter.HasValue)
				RightThumbstickCenter = rightTouch.Value.position;

			// save the position of the touch
			rightPosition = rightTouch.Value.position;

			// save the ID of the touch
			rightId = rightTouch.Value.fingerId;
		}
		else
		{
			// otherwise reset our values to not track any touches
			// for the right thumbstick
			RightThumbstickCenter = null;
			rightId = -1;
		}

		oldTouches = touches;
	}

	private static bool TryGetPreviousLocation(Touch touch, out Touch earliestTouch)
	{
		//if (!TryGetPreviousLocation(touch, out earliestTouch))
		//    earliestTouch = touch;

		if (oldTouches == null || oldTouches.Length == 0) { earliestTouch = touch; return false; }

		foreach (Touch t in oldTouches)
		{
			if (t.fingerId == touch.fingerId)
			{
				earliestTouch = t;
				return true;
			}
		}

		earliestTouch = touch;
		return true;
	}


	/// <summary>
	/// Gets the value of the left thumbstick.
	/// </summary>
	public static Vector2 LeftThumbstick
	{
		get
		{
			// if there is no left thumbstick center, return a value of (0, 0)
			if (!LeftThumbstickCenter.HasValue)
				return Vector2.zero;

			// calculate the scaled vector from the touch position to the center,
			// scaled by the maximum thumbstick distance
			Vector2 l = (leftPosition - LeftThumbstickCenter.Value) / maxThumbstickDistance;

			// if the length is more than 1, normalize the vector
			if (l.magnitude > 1f) //r.LengthSquared();
				l.Normalize();

			return l;
		}
	}

	/// <summary>
	/// Gets the value of the right thumbstick.
	/// </summary>
	public static Vector2 RightThumbstick
	{
		get
		{
			// if there is no left thumbstick center, return a value of (0, 0)
			if (!RightThumbstickCenter.HasValue)
				return Vector2.zero;

			// calculate the scaled vector from the touch position to the center,
			// scaled by the maximum thumbstick distance
			Vector2 r = (rightPosition - RightThumbstickCenter.Value) / maxThumbstickDistance;

			// if the length is more than 1, normalize the vector
			if (r.magnitude > 1f) //r.LengthSquared();
				r.Normalize();

			return r;

		}
	}
}