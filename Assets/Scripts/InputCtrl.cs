using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputCtrl : GridBehaviour {

	public float LongTouchThreshold;
	public float SwipeDistanceThreshold;

	private bool _isSwipe;
	private bool _isNewSwipe;
	private bool _isLongTouch;
	private bool _touchFinished;
	private float _touchBeginTime;
	private Vector3 _touchBeginCoords;
	private Vector3 _touchEndCoords;

	void Update() {
		_isLongTouch = false;
		_isSwipe = false;
		_touchFinished = false;

		if (Input.GetMouseButtonDown(0)) {
			StartTouch();
		} else if (Input.GetMouseButtonUp(0)) {
			EndTouch();
		} else if (Input.GetMouseButton(0)) {
			DuringTouch();
		}
	}

	public bool IsMovingRight() {
		Debug.Log(IsInShortTouch());
		return Input.GetKeyDown(KeyCode.RightArrow) ||
		       (IsInShortTouch() &&
		        Input.mousePosition.x > Camera.main.pixelWidth * 0.5 &&
		        Input.mousePosition.y < Camera.main.pixelHeight * 0.5) ||
					(IsInSwipe() && SwipeDirection() == Vector3.right);
	}

	public bool IsMovingLeft() {
		return Input.GetKeyDown(KeyCode.LeftArrow) ||
		       (IsInShortTouch() &&
		        Input.mousePosition.x < Camera.main.pixelWidth * 0.5 &&
		        Input.mousePosition.y < Camera.main.pixelHeight * 0.5) ||
					(IsInSwipe() && SwipeDirection() == - Vector3.right);
	}

	public bool IsRotating() {
		return Input.GetKeyDown(KeyCode.UpArrow) ||
		       (IsInShortTouch() &&
		        Input.mousePosition.y > Camera.main.pixelHeight * 0.4);
	}

	public bool IsInTurboMode() {
		return Input.GetKey(KeyCode.DownArrow) || (IsInSwipe() && SwipeDirection() == Vector3.down);
	}

	Vector3 SwipeDirection() {
		Vector3 delta = _touchEndCoords - _touchBeginCoords;

		// make the vector point to a straight direction (left, right, up and down)
		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
			delta.y = 0;
		} else {
			delta.x = 0;
		}

		return delta.normalized;
	}

	bool IsInLongTouch() {
		return _isLongTouch;
	}

	bool IsInSwipe() {
		return _isSwipe && _isNewSwipe;
	}

	float TouchDuration() {
		return Time.time - _touchBeginTime;
	}

	float TouchDistance() {
		return (Input.mousePosition - _touchBeginCoords).magnitude;
	}

	public bool IsInShortTouch() {
		return Input.GetMouseButtonUp(0) && !IsInLongTouch() && !IsInSwipe();
	}

	void StartTouch() {
		_touchBeginTime = Time.time;
		_touchBeginCoords = Input.mousePosition;
		_isNewSwipe = true;
		DuringTouch();
	}

	void DuringTouch() {
		_touchEndCoords = Input.mousePosition;
		DetectTouchType();
	}

	void EndTouch() {
		_touchEndCoords = Input.mousePosition;
		_touchFinished = true;
		DetectTouchType();
	}

	void DetectTouchType() {
		if (TouchDistance() > SwipeDistanceThreshold) {
			_isSwipe = true;
		} else if (TouchDuration() > LongTouchThreshold) {
			_isLongTouch = true;
		}
	}
}
