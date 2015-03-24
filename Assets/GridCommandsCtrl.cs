using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GridCommandsCtrl : MonoBehaviour {

	public float LongTouchThreshold;
	public float SwipeDistanceThreshold;

	private bool _isTouching;
	private bool _isSwipe;
	private bool _isNewSwipe;
	private bool _isLongTouch;
	private bool _touchFinished;
	private float _touchBeginTime;
	private float _touchEndTime;
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
		var result = Input.GetKeyDown(KeyCode.RightArrow) || (IsInSwipe() && SwipeDirection() == Vector2.right);
		return UseSwipe(result);
	}

	public bool IsMovingLeft() {
		var result = Input.GetKeyDown(KeyCode.LeftArrow) || (IsInSwipe() && SwipeDirection() == - Vector2.right);
		return UseSwipe(result);
	}

	public bool IsRotating() {
		return Input.GetKeyDown(KeyCode.UpArrow) || HasShortTouch();
	}

	public bool IsInTurboMode() {
		return Input.GetKeyDown(KeyCode.DownArrow) || IsInLongTouch();
	}

	Vector2 SwipeDirection() {
		Vector2 delta = _touchEndCoords - _touchBeginCoords;

		// make the vector point to a straight direction (left, right, up and down)
		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
			delta.y = 0;
		} else {
			delta.x = 0;
		}
		Debug.Log(delta.normalized);

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

	bool HasShortTouch() {
		return _touchFinished && !_isSwipe && !_isLongTouch;
	}

	void StartTouch() {
		_isTouching = true;
		_touchBeginTime = Time.time;
		_touchBeginCoords = Input.mousePosition;
		_isNewSwipe = true;
		DuringTouch();
	}

	void DuringTouch() {
		_touchEndTime = Time.time;
		_touchEndCoords = Input.mousePosition;
		DetectTouchType();
	}

	void EndTouch() {
		_isTouching = false;
		_touchEndTime = Time.time;
		_touchEndCoords = Input.mousePosition;
		_touchFinished = true;
		DetectTouchType();
	}

	void DetectTouchType() {
		if (TouchDistance() > SwipeDistanceThreshold) {
			Debug.Log("in swipe");
			Debug.Log(SwipeDirection());
			_isSwipe = true;
		} else if (TouchDuration() > LongTouchThreshold) {
			_isLongTouch = true;
		}
	}

	bool UseSwipe(bool use) {
		if (use) {
			_isNewSwipe = false;
		}
		return use;
	}
}
