using UnityEngine;
using System.Collections;
using Constants;

public class GridCtrl : MonoBehaviour {

	public int columns;
	public int rows;
	public float width, height;
	public float fallDelay;
	public GameObject piecePrefab;

	float pieceWidth, pieceHeight;
	GameObject[,] grid;

	// Use this for initialization
	void Awake () {
		pieceWidth = width / columns;
		pieceHeight = height / rows;
		SetupGrid();
	}

	void Start() {
		AddPiece(1, rows-1);
		StartCoroutine(Fall());
	}

	void SetupGrid() {
		grid = new GameObject[columns, rows];
		for(int y = 0; y < rows; ++y) {
			for(int x = 0; x < columns; ++x) {
				Vector3 coords = new Vector3 (x * pieceWidth + pieceWidth / 2 - width / 2, y * pieceHeight + pieceHeight / 2 - width / 2, 0);
				GameObject piece = Instantiate (piecePrefab, coords, Quaternion.identity) as GameObject;
				piece.transform.localScale = new Vector3 (width / columns, height / rows, 1);
				piece.transform.parent = transform;
				grid[x, y] = piece;
			}
		}
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			MoveRight();
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			MoveLeft();
		}
	}

	void MoveRight() {
		for(int y = 0; y < rows; ++y) {
			if (grid[columns-1, y].GetComponent<PieceCtrl>().state == PieceState.Current) {
				return;
			}
		}

		for(int y = 0; y < rows; ++y) {
			for(int x = columns - 2; x >= 0; --x) {
				PieceCtrl currentPiece = grid[x, y].GetComponent<PieceCtrl>();
				Debug.Log(x);
				PieceCtrl nextPiece = grid[x+1, y].GetComponent<PieceCtrl>();
				if (currentPiece.state == PieceState.Current) {
					currentPiece.UpdateState(PieceState.Empty);
					nextPiece.UpdateState(PieceState.Current);
				}
			}
		}
	}

	void MoveLeft() {
		for(int y = 0; y < rows; ++y) {
			if (grid[0, y].GetComponent<PieceCtrl>().state == PieceState.Current) {
				return;
			}
		}

		for(int y = 0; y < rows; ++y) {
			for(int x = 1; x < columns; ++x) {
				PieceCtrl currentPiece = grid[x, y].GetComponent<PieceCtrl>();
				PieceCtrl nextPiece = grid[x-1, y].GetComponent<PieceCtrl>();
				if (currentPiece.state == PieceState.Current) {
					currentPiece.UpdateState(PieceState.Empty);
					nextPiece.UpdateState(PieceState.Current);
				}
			}
		}
	}
		/*if (fallingPieceX == 0) {
			return;
		}
		if (grid[fallingPieceX - 1, fallingPieceY] == null) {
			grid[fallingPieceX - 1, fallingPieceY] = grid[fallingPieceX, fallingPieceY];
			grid[fallingPieceX, fallingPieceY] = null;
			grid[fallingPieceX - 1, fallingPieceY].transform.Translate(Vector3.left * pieceWidth);
			fallingPieceX--;
		}*/

	void AddPiece(int x, int y) {
		grid[x, y].GetComponent<PieceCtrl>().UpdateState(PieceState.Current);
	}

	IEnumerator Fall() {
		while (true) {
			yield return new WaitForSeconds (fallDelay);
			// refactor this out of here
			if (CanFall()) {
				FallPieces(PieceState.Current);
			} else {
				FinishPiece();
			}
		}
		yield return true;
	}

	public GameObject Get(int x, int y) {
		return grid[x, y];
	}

	bool CanFall() {
		for(int y = 1; y < rows; ++y) {
			for(int x = 0; x < columns; ++x) {
				PieceCtrl currentPiece = grid[x, y].GetComponent<PieceCtrl>();
				PieceCtrl nextPiece = grid[x, y-1].GetComponent<PieceCtrl>();
				if (currentPiece.state == PieceState.Current && nextPiece.state == PieceState.Full) {
					return false;
				}
			}
		}
		return true;
	}

	void FinishPiece() {
		for(int y = 0; y < rows; ++y) {
			for(int x = 0; x < columns; ++x) {
				PieceCtrl currentPiece = grid[x, y].GetComponent<PieceCtrl>();
				if (currentPiece.state == PieceState.Current) {
					currentPiece.UpdateState(PieceState.Full);
				}
			}
		}
	}

	void FallPieces(PieceState state) {
		for(int y = 1; y < rows; ++y) {
			for(int x = 0; x < columns; ++x) {
				PieceCtrl currentPiece = grid[x, y].GetComponent<PieceCtrl>();
				PieceCtrl nextPiece = grid[x, y-1].GetComponent<PieceCtrl>();
				if (currentPiece.state == state) {
					currentPiece.UpdateState(PieceState.Empty);
					nextPiece.UpdateState(state);
				}
			}
		}
	}
}
