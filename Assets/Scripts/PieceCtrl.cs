using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Constants;

public class PieceCtrl : MonoBehaviour {

	public PieceType Type;

	public Color CurrentColor;
	public Color FullColor;
	private PieceState _state;
	private bool _rotated;

	private bool _falling;
	private float _fallStart;
	private float _fallOrigin;

	void Awake () {
		_rotated = false;
		MakeCurrent();
	}

	void Update() {
		if (_falling) {
			var timeElapsed = Time.time - _fallStart;
			if (timeElapsed >= 0.2f) {
				_falling = false;
			}
			var newPosition = transform.position;
			newPosition.y = Mathf.Lerp(_fallOrigin, _fallOrigin - Height(), timeElapsed / 0.2f);
			transform.position = newPosition;
		}
	}

	public bool CanFall(Rect boundaries, Func<Vector3, bool> isCoordFree) {
		foreach (Transform piecePart in transform) {
			var nextPosition = piecePart.position + (Vector3.down * Height());
			
			if (!boundaries.Contains(nextPosition) || !isCoordFree(nextPosition)) {
				return false;
			}
		}
		return true;
	}

	public List<Vector3> PartPositions() {
		var result = new List<Vector3>();
		foreach (Transform part in transform) {
			result.Add(part.position);
		}
		return result;
	}

	public void Fall() {
		//_falling = true;
		//_fallStart = Time.time;
		//_fallOrigin = transform.position.y;
		transform.Translate(Vector3.down * Height(), Space.World);
	}

	public void MakeFull() {
		Make(PieceState.Full);
	}

	public void MakeCurrent() {
		Make(PieceState.Current);
	}

	public void Make(PieceState state) {
		_state = state;
		UpdateMaterial();
	}

	public bool Is(PieceState state) {
		return _state == state;
	}

	public bool IsCurrent() {
		return Is(PieceState.Current);
	}

	public bool IsFull() {
		return Is(PieceState.Full);
	}


	public int Columns() {
		if (_rotated) {
			return Size().Second;
		} else {
			return Size().First;
		}
	}

	public int Rows() {
		if (_rotated) {
			return Size().First;
		} else {
			return Size().Second;
		}
	}

	public Pair<int, int> Size() {
		var rowValues = new List<float>();
		var colValues = new List<float>();
		foreach (Transform child in transform) {
			var col = child.localPosition.x;
			var row = child.localPosition.y;

			if (colValues.All(value => Mathf.Abs(value - col) > child.localScale.x * 0.5)) {
				colValues.Add(col);
			}

			if (rowValues.All(value => Mathf.Abs(value - row) > child.localScale.y * 0.5)) {
				rowValues.Add(row);
			}
		}

		return new Pair<int, int>(colValues.Count, rowValues.Count);
	} 

	void UpdateMaterial() {
		var newColor = PieceColor();
		var newDarkenedColor = DarkenedPieceColor();

		foreach (Transform child in transform) {
			var renderers = child.GetComponentsInChildren<Renderer>();
			renderers[0].material.color = newColor;
			renderers[1].material.color = newDarkenedColor;
		}
	}

	Color PieceColor() {
		if (IsCurrent()) {
			return CurrentColor;
		} else {
			return FullColor;
		}
	}

	Color DarkenedPieceColor() {
		return PieceColor() - new Color(0.05f, 0.05f, 0.05f);
	}

	public float Width() {
		if (_rotated) {
			return Mathf.Abs(transform.localScale.y);
		} else {
			return Mathf.Abs(transform.localScale.x);
		}
	}

	public float Height() {
		if (_rotated) {
			return Mathf.Abs(transform.localScale.x);
		} else {
			return Mathf.Abs(transform.localScale.y);
		}
	}

	public void Rotate(int offset = 0) {
		_rotated = !_rotated;
		transform.Rotate(0, 0, 90);
	}

	public void Unrotate(int offset = 0) {
		_rotated = !_rotated;
		transform.Rotate(0, 0, -90);
	}
}
