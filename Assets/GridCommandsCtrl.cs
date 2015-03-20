using UnityEngine;
using System.Collections;

public class GridCommandsCtrl : MonoBehaviour {


	public bool IsMovingRight() {
		return Input.GetKeyDown(KeyCode.RightArrow);
		return Input.GetKeyDown(KeyCode.RightArrow) || (SwipeDirection() == Vector2.right);
	}

	public bool IsMovingLeft() {
		return Input.GetKeyDown(KeyCode.LeftArrow) || (SwipeDirection() == - Vector2.right);
	}

	public bool IsRotating() {
		return Input.GetKeyDown(KeyCode.UpArrow));
	}

	public bool IsEnteringTurboMode() {
		return Input.GetKeyDown(KeyCode.DownArrow);
	}

	public bool IsExitingTurboMode() {
		return Input.GetKeyUp(KeyCode.DownArrow);
	}

	Vector2 SwipeDirection() {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			Vector2 delta = Input.GetTouch(0).deltaPosition;

			// make the vector point to a straight direction (left, right, up and down)
			if (delta.x > delta.y) {
				delta.y = 0;
			} else {
				delta.x = 0;
			}

			return delta.normalized;
		} else {
			return Vector2.zero;
		}
	}
}
