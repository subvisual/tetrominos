using UnityEngine;
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
		Type = type;
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
		return State == state;
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
		Type = type;
		UpdateMaterial();
	}

	public void UpdateState(PieceState state) {
		State = state;
		UpdateMaterial();
	}

	void UpdateMaterial() {
		var newColor = PieceColor();
		var newDarkenedColor = DarkenedPieceColor();

		transform.Find("TopHalf").GetComponent<Renderer>().material.color = newColor;
		transform.Find("BottomHalf").GetComponent<Renderer>().material.color = newDarkenedColor;
	}

	Color PieceColor() {
		if (IsEmpty()) {
			return EmptyColor;
		} else if (IsCurrent()) {
			return CurrentColor;
		} else {
			return TypeColors[(int) Type];
		}
	}

	Color DarkenedPieceColor() {
		if (IsEmpty()) {
			return EmptyColor;
		} else {
			return PieceColor() - new Color(0.05f, 0.05f, 0.05f);
		}
	}
}
