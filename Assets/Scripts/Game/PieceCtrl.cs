using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Constants;

public class PieceCtrl : MonoBehaviour {

	public PieceType Type;

	private PieceState _state;
	private bool _rotated;

  public Material LightMaterial;
  public Material DarkMaterial;
  public Material ActiveLightMaterial;
  public Material ActiveDarkMaterial;

	void Awake() {
		_rotated = false;
		MakeFull();
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
	  Material lightMaterial, darkMaterial;

	  if (IsCurrent()) {
	    lightMaterial = ActiveLightMaterial;
	    darkMaterial = ActiveDarkMaterial;
	  }
	  else {
      lightMaterial = LightMaterial;
      darkMaterial = DarkMaterial;
    }

		foreach (Transform child in transform) {
			var renderers = child.GetComponentsInChildren<Renderer>();
			renderers[0].material = lightMaterial;
			renderers[1].material = darkMaterial;
		}
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
