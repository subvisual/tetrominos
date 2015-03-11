using UnityEngine;
using System.Collections;
using Constants;

public class GridCtrl : MonoBehaviour {

	public int columns;
	public int rows;
	public float fallDelay, fallTurbo;
	public GameObject piecePrefab;
	public Grid grid;
	

	void Awake () {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		float height = Camera.main.orthographicSize * 2;
		float width = height / Screen.height * Screen.width;
		grid = new Grid(piecePrefab, transform, columns, rows, width, height);

	}

	void Start() {
		GetComponent<PieceFactory>().AddNext(grid);
		StartCoroutine(Fall());
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			grid.MoveRight();
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			grid.MoveLeft();
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			fallDelay /= fallTurbo;
		}

		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			fallDelay *= fallTurbo;
		}
	}

	IEnumerator Fall() {
		while (true) {
			// refactor this out of here
			if (!grid.Fall(PieceState.Current)) {
				grid.FinishPiece();
				GetComponent<PieceFactory>().AddNext(grid);
			}
			
			var destroyedRows = grid.DestroyFullRows();
			if (destroyedRows > 0)
				grid.CollapseFull();

			yield return new WaitForSeconds(fallDelay);
		}
		yield return true;
	}
}
