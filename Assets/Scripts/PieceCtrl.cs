using System;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class PieceCtrl : MonoBehaviour {

	public PieceType Type;

	public Color CurrentColor;
	public Color FullColor;
	private PieceState _state;

	void Awake () {
		MakeCurrent();
	}

	public bool CanFall(Rect boundaries, Func<Vector3, bool> isCoordFree) {
		foreach (Transform piecePart in transform) {
			var nextPosition = piecePart.position + (Vector3.down * transform.localScale.y);
			
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
		transform.Translate(Vector3.down * transform.localScale.y);
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
}
