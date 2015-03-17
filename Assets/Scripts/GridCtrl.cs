using UnityEngine;
using System.Collections;
using Constants;

public class GridCtrl : MonoBehaviour {

	public int Columns;
	public int Rows;
	public float FallDelay, FallTurbo;
	public GameObject PiecePrefab;
	public Grid Grid;
	

	void Awake () {
		var height = Camera.main.orthographicSize * 2;
		var width = height / Screen.height * Screen.width;
		Grid = new Grid(PiecePrefab, transform, Columns, Rows, width, height);

	}

	void Start() {
		GetComponent<PieceFactory>().AddNext(Grid);
		StartCoroutine(Fall());
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			Grid.MoveRight();
		} else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			Grid.MoveLeft();
		} else if (Input.GetKeyUp(KeyCode.UpArrow)) {
			Grid.Rotate();
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			FallDelay /= FallTurbo;
		}

		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			FallDelay *= FallTurbo;
		}

	}

	IEnumerator Fall() {
		while (true) {
			// refactor this out of here
			if (!Grid.Fall(PieceState.Current)) {
				Grid.FinishPiece();
				GetComponent<PieceFactory>().AddNext(Grid);
			}
			
			var destroyedRows = Grid.DestroyFullRows();
			if (destroyedRows > 0)
				Grid.CollapseFull();

			yield return new WaitForSeconds(FallDelay);
		}
		yield return true;
	}
}
