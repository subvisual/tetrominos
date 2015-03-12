using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Constants;

public class PieceCtrl : MonoBehaviour {

	public PieceType Type;
	public PieceState State;

	public Color EmptyColor, CurrentColor;
	public Color[] TypeColors;

	// Use this for initialization
	void Awake () {
		State = PieceState.Empty;
		UpdateMaterial();
	}

	public void SetType(PieceType type) {
		this.Type = type;
	}

	public void Fall() {
		if (State != PieceState.Current) {
			return;
		}

		transform.Translate (Vector3.down * transform.localScale.y);
	}

	public void Make(PieceState state) {
		if (state == PieceState.Empty) {
			Type = PieceType.Empty;
		}
		UpdateState(state);
	}

	public void MakeEmpty() {
		Make(PieceState.Empty);
	}

	public void MakeFull() {
		Make(PieceState.Full);
	}

	public void MakeCurrent() {
		Make(PieceState.Current);
	}

	public bool Is(PieceState state) {
		return this.State == state;
	}

	public bool IsEmpty() {
		return Is(PieceState.Empty);
	}

	public bool IsCurrent() {
		return Is(PieceState.Current);
	}

	public bool IsFull() {
		return Is(PieceState.Full);
	}

	public void Replace(PieceCtrl previousPiece) {
		UpdateState(previousPiece.State);
		UpdateType(previousPiece.Type);
		previousPiece.MakeEmpty();
	}

	public void UpdateType(PieceType type) {
		this.Type = type;
		UpdateMaterial();
	}

	public void UpdateState(PieceState state) {
		this.State = state;
		UpdateMaterial();
	}

	void UpdateMaterial() {
		renderer.material.color = PieceColor();
	}

	Color PieceColor() {
		if (IsCurrent()) {
			return CurrentColor;
		} else if (IsEmpty()) {
			return EmptyColor;
		} else {
			return TypeColors[(int) Type];
		}
	}
}
