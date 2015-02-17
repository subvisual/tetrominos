using UnityEngine;
using System.Collections;
using Constants;

public class PieceFactory : MonoBehaviour {

  /* THIS DOESN'T WORK LIKE THIS */
	private static readonly PieceState[,,] templates = {
				{
					 { PieceState.Current }
				},
				{
					{ PieceState.Current, PieceState.Current },
					{ PieceState.Current, PieceState.Current }
				}
	};

	private int nextIndex;

	// Use this for initialization
	void Start () {
		RollNext();
	}

	public void AddNext(GameObject[,] grid) {
		GameObject newPiece = grid[grid.GetLength(0)/2, grid.GetLength(1)-1];
		newPiece.GetComponent<PieceCtrl>().UpdateState(PieceState.Current);

		RollNext();
	}

	void RollNext() {
		nextIndex = Random.Range(0, templates.GetLength(0));
	}
}
