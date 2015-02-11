using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceCtrl : MonoBehaviour {
	public enum State { Empty, Full, Current };

	public State state;
	public Vector2 coords;

	// Use this for initialization
	void Start () {
		state = State.Current;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetCoords(int x, int y) {
		coords = new Vector2 (x, y);
	}

	public void Fall(PieceList pieces) {
		if (state != State.Current) {
			return;
		}

		Vector2 destination = new Vector2 (coords.x, coords.y + 1);
		if (pieces.findByCoords (destination)) {
			return;
		}

		coords = destination;
		Debug.Log (coords);
		transform.Translate (Vector3.down * transform.localScale.y);
	}
}
