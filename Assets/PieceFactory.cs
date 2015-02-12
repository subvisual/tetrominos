using UnityEngine;
using System.Collections;
using Constants;

public class PieceFactory : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void AddNext(GameObject[,] grid) {
		GameObject newPiece = grid[grid.GetLength(0)/2, grid.GetLength(1)-1];
		newPiece.GetComponent<PieceCtrl>().UpdateState(PieceState.Current);
	}
}
