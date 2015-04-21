using System.Xml;
using UnityEngine;

public class InputCtrl : GridBehaviour {

	public float LongTouchThreshold;
	public float SwipeDistanceThreshold;

	private bool _isTouch;
	private bool _isSwipe;
	private bool _wasSwipe;
	private bool _swipeMuted;
	private float _pieceWidth;
	private Vector3 _touchBeginCoords;
	private Vector3 _touchLastCoords;
	private Vector3 _touchEndCoords;

	void Start() {
		_pieceWidth = GetComponent<GridCtrl>().Columns;
		_isSwipe = false;
	}

	void Update() {
		_isTouch = false;
		_swipeMuted = true;
		_wasSwipe = false;

		if (Input.GetMouseButtonDown(0)) {
			StartTouch();
		} else if (Input.GetMouseButtonUp(0)) {
			EndTouch();
		} else if (Input.GetMouseButton(0)) {
			DuringTouch();
		}
	}

	public bool IsMovingRight() {
		var ret = Input.GetKeyDown(KeyCode.RightArrow) ||
					(IsInNewSwipe() && SwipeDirection() == Vector3.right);
		return UseSwipe(ret);
	}

	public bool IsMovingLeft() {
		var ret = Input.GetKeyDown(KeyCode.LeftArrow) ||
					(IsInNewSwipe() && SwipeDirection() == - Vector3.right);
		return UseSwipe(ret);
	}

	public bool IsRotating() {
		return Input.GetKeyDown(KeyCode.UpArrow) ||
					IsInShortTouch();
	}

	public bool IsInTurboMode() {
		return Input.GetKey(KeyCode.DownArrow) || (IsInSwipe() && SwipeDirection() == Vector3.down);
	}

	Vector3 SwipeDirection() {
		Vector3 delta = _touchEndCoords - _touchLastCoords;

		// make the vector point to a straight direction (left, right, up and down)
		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
			delta.y = 0;
		} else {
			delta.x = 0;
		}

		return delta.normalized;
	}

	public bool IsInShortTouch() {
		return Input.GetMouseButtonUp(0) && !_isSwipe && !_wasSwipe && _isTouch;
	}

	bool IsInSwipe() {
		return _isSwipe;
	}

	bool IsInNewSwipe() {
		return !_swipeMuted;
	}

	float TouchDistance() {
		return (Input.mousePosition - _touchLastCoords).magnitude;
	}

	void StartTouch() {
		_touchBeginCoords = Input.mousePosition;
		_touchLastCoords = _touchBeginCoords;
		DuringTouch();
	}

	void DuringTouch() {
		_isTouch = true;
		_touchEndCoords = Input.mousePosition;

		if (_isSwipe || TouchDistance() > SwipeDistanceThreshold) {
			_isSwipe = true;
			_wasSwipe = true;
			if (TouchDistance() > Screen.width / _pieceWidth) {
				_swipeMuted = false;
			}
		}
	}

	void EndTouch() {
		DuringTouch();
		_isSwipe = false;
	}

	bool UseSwipe(bool use) {
		if (use) {
			_touchLastCoords = Input.mousePosition;
		}
		return use;
	}
}
