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

	/*void AddPiece(int x, int y) {
		grid[x, y].GetComponent<PieceCtrl>().MakeCurrent();
	}*/

	IEnumerator Fall() {
		while (true) {
			// refactor this out of here
			if (grid.CanFall()) {
				grid.FallPieces(PieceState.Current);
			} else {
				grid.FinishPiece();
				GetComponent<PieceFactory>().AddNext(grid);
			}
			yield return new WaitForSeconds (fallDelay);
		}
		yield return true;
	}

	/*public GameObject Get(int x, int y) {
		return grid[x, y];
	}*/

	

}
