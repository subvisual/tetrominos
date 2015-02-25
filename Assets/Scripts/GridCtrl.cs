using UnityEngine;
using System.Collections;
using Constants;

public class GridCtrl : MonoBehaviour {

	public int columns;
	public int rows;
	public float fallDelay, fallTurbo;
	public GameObject piecePrefab;
	
	float width, height;
	float pieceWidth, pieceHeight;
	GameObject[,] grid;

	void Awake () {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		height = Camera.main.orthographicSize * 2;
		width = height / Screen.height * Screen.width;

		pieceWidth = width / columns;
		pieceHeight = height / rows;
		SetupGrid();
	}

	void Start() {
		GetComponent<PieceFactory>().AddNext(grid);
		StartCoroutine(Fall());
	}

	void SetupGrid() {
		grid = new GameObject[columns, rows];
		for(int y = 0; y < rows; ++y) {
			for(int x = 0; x < columns; ++x) {
				Vector3 coords = new Vector3 (x * pieceWidth + pieceWidth / 2 - width / 2,
				                              y * pieceHeight + pieceHeight / 2 - height / 2,
				                              0);
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

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			fallDelay /= fallTurbo;
		}

		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			fallDelay *= fallTurbo;
		}
	}

	void MoveRight() {
		for(int y = 0; y < rows; ++y) {
			if (grid[columns-1, y].GetComponent<PieceCtrl>().IsCurrent()) {
				return;
			}
		}

		if (CanMove(1)) {
			for(int y = 0; y < rows; ++y) {
				for(int x = columns - 2; x >= 0; --x) {
					PieceCtrl currentPiece = grid[x, y].GetComponent<PieceCtrl>();
					PieceCtrl nextPiece = grid[x+1, y].GetComponent<PieceCtrl>();
					if (currentPiece.IsCurrent()) {
						//currentPiece.UpdateState(PieceState.Empty);
						//nextPiece.UpdateState(PieceState.Current);
						nextPiece.Replace(currentPiece);
					}
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

		if (CanMove(-1)) {
			for(int y = 0; y < rows; ++y) {
				for(int x = 1; x < columns; ++x) {
					PieceCtrl currentPiece = grid[x, y].GetComponent<PieceCtrl>();
					PieceCtrl nextPiece = grid[x-1, y].GetComponent<PieceCtrl>();
					if (currentPiece.IsCurrent()) {
						//currentPiece.UpdateState(PieceState.Empty);
						//nextPiece.UpdateState(PieceState.Current);
						nextPiece.Replace(currentPiece);
					}
				}
			}
		}
	}

	bool CanMove(int direction) {
		for(int y = 0; y < rows; ++y) {
			for(int x = 1; x < columns - 1; ++x) {
				PieceCtrl currentPiece = grid[x, y].GetComponent<PieceCtrl>();
				PieceCtrl nextPiece = grid[x + direction, y].GetComponent<PieceCtrl>();
				if (currentPiece.IsCurrent() && nextPiece.IsFull()) {
					return false;
				}
			}
		}
		return true;
	}

	void AddPiece(int x, int y) {
		grid[x, y].GetComponent<PieceCtrl>().MakeCurrent();
	}

	IEnumerator Fall() {
		while (true) {
			// refactor this out of here
			if (CanFall()) {
				FallPieces(PieceState.Current);
			} else {
				FinishPiece();
				GetComponent<PieceFactory>().AddNext(grid);
			}
			yield return new WaitForSeconds (fallDelay);
		}
		yield return true;
	}

	public GameObject Get(int x, int y) {
		return grid[x, y];
	}

	bool CanFall() {
		for(int x = 0; x < columns; ++x) {
			if (grid[x, 0].GetComponent<PieceCtrl>().IsCurrent()) {
				return false;
			}
		}

		for(int y = 1; y < rows; ++y) {
			for(int x = 0; x < columns; ++x) {
				PieceCtrl currentPiece = grid[x, y].GetComponent<PieceCtrl>();
				PieceCtrl nextPiece = grid[x, y-1].GetComponent<PieceCtrl>();
				if (currentPiece.IsCurrent() && nextPiece.IsFull()) {
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
				if (currentPiece.IsCurrent()) {
					currentPiece.MakeFull();
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
					nextPiece.Replace(currentPiece);
				}
			}
		}
	}
}
