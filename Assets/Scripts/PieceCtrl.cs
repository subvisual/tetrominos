using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Constants;

public class PieceCtrl : MonoBehaviour {

	public PieceType type;
	public PieceState state;

	public Color emptyColor, currentColor;
	public Color[] typeColors;

	// Use this for initialization
	void Awake () {
		state = PieceState.Empty;
		UpdateMaterial();
	}

	public void SetType(PieceType type) {
		this.type = type;
	}

	public void Fall() {
		if (state != PieceState.Current) {
			return;
		}

		transform.Translate (Vector3.down * transform.localScale.y);
	}

	public void MakeEmpty() {
		type = PieceType.Empty;
		UpdateState(PieceState.Empty);
	}

	public void MakeFull() {
		UpdateState(PieceState.Full);
	}

	public void MakeCurrent() {
		UpdateState(PieceState.Current);
	}

	public bool IsEmpty() {
		return state == PieceState.Empty;
	}

	public bool IsCurrent() {
		return state == PieceState.Current;
	}

	public bool IsFull() {
		return state == PieceState.Full;
	}

	public void Replace(PieceCtrl previousPiece) {
		UpdateState(previousPiece.state);
		UpdateType(previousPiece.type);
		previousPiece.MakeEmpty();
	}

	public void UpdateType(PieceType type) {
		this.type = type;
		UpdateMaterial();
	}

	public void UpdateState(PieceState state) {
		this.state = state;
		UpdateMaterial();
	}

	void UpdateMaterial() {
		renderer.material.color = PieceColor();
	}

	Color PieceColor() {
		if (IsCurrent()) {
			return currentColor;
		} else if (IsEmpty()) {
			return emptyColor;
		} else {
			if (state == PieceState.Full) Debug.Log(type);
			return typeColors[(int) type];
		}
	}
}
