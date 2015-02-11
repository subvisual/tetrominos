using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Constants;

public class PieceCtrl : MonoBehaviour {

	public PieceState state;
	public Color emptyColor;
	public Color currentColor;
	public Color fullColor;

	// Use this for initialization
	void Awake () {
		state = PieceState.Empty;
		UpdateMaterial();
	}

	public void Fall() {
		if (state != PieceState.Current) {
			return;
		}

		transform.Translate (Vector3.down * transform.localScale.y);
	}

	public void UpdateState(PieceState newState) {
		state = newState;
		UpdateMaterial();
	}

	void UpdateMaterial() {
		switch (state) {
			case PieceState.Current:
				renderer.material.color = currentColor;
				break;
			case PieceState.Full:
				renderer.material.color = fullColor;
				break;
			case PieceState.Empty:
				renderer.material.color = emptyColor;
				break;
		}
	}
}
