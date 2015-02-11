using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Constants;

public class PieceCtrl : MonoBehaviour {

	public PieceState state;
	public Material emptyMaterial;
	public Material currentMaterial;
	public Material fullMaterial;

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
				renderer.material = currentMaterial;
				break;
			case PieceState.Full:
				renderer.material = fullMaterial;
				break;
			case PieceState.Empty:
				renderer.material = emptyMaterial;
				break;
		}
	}
}
