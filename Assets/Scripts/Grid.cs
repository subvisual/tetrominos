using UnityEngine;
using System.Collections;
using Constants;

public class Grid {

	GridCtrl gridCtrl;

	GameObject piecePrefab;
	GameObject[,] objects;

	public int columns, rows;
	public float width, height;
	float pieceWidth, pieceHeight;

	public Grid (GameObject piecePrefab, Transform parent, int columns, int rows, float width, float height) {
		this.piecePrefab = piecePrefab;
		this.columns = columns;
		this.rows = rows;
		this.width = width;
		this.height = height;
		this.pieceWidth = width / columns;
		this.pieceHeight = height / rows;
		SetupGrid(parent);
	}

	public void MoveRight() {
		for (int y = 0; y < rows; ++y) {
			if (objects[columns - 1, y].GetComponent<PieceCtrl>().IsCurrent()) {
				return;
			}
		}

		if (CanMove(1)) {
			for (int y = 0; y < rows; ++y) {
				for (int x = columns - 2; x >= 0; --x) {
					PieceCtrl currentPiece = objects[x, y].GetComponent<PieceCtrl>();
					PieceCtrl nextPiece = objects[x + 1, y].GetComponent<PieceCtrl>();
					if (currentPiece.IsCurrent()) {
						nextPiece.Replace(currentPiece);
					}
				}
			}
		}
	}

	public void MoveLeft() {
		for (int y = 0; y < rows; ++y) {
			if (objects[0, y].GetComponent<PieceCtrl>().state == PieceState.Current) {
				return;
			}
		}

		if (CanMove(-1)) {
			for (int y = 0; y < rows; ++y) {
				for (int x = 1; x < columns; ++x) {
					PieceCtrl currentPiece = objects[x, y].GetComponent<PieceCtrl>();
					PieceCtrl nextPiece = objects[x - 1, y].GetComponent<PieceCtrl>();
					if (currentPiece.IsCurrent()) {
						nextPiece.Replace(currentPiece);
					}
				}
			}
		}
	}

	bool CanMove(int direction) {
		for (int y = 0; y < rows; ++y) {
			for (int x = 1; x < columns - 1; ++x) {
				PieceCtrl currentPiece = objects[x, y].GetComponent<PieceCtrl>();
				PieceCtrl nextPiece = objects[x + direction, y].GetComponent<PieceCtrl>();
				if (currentPiece.IsCurrent() && nextPiece.IsFull()) {
					return false;
				}
			}
		}
		return true;
	}

	public bool CanFall() {
		for (int x = 0; x < columns; ++x) {
			if (objects[x, 0].GetComponent<PieceCtrl>().IsCurrent()) {
				return false;
			}
		}

		for (int y = 1; y < rows; ++y) {
			for (int x = 0; x < columns; ++x) {
				PieceCtrl currentPiece = objects[x, y].GetComponent<PieceCtrl>();
				PieceCtrl nextPiece = objects[x, y - 1].GetComponent<PieceCtrl>();
				if (currentPiece.IsCurrent() && nextPiece.IsFull()) {
					return false;
				}
			}
		}
		return true;
	}

	void SetupGrid(Transform parent) {
		objects = new GameObject[columns, rows];

		for (int y = 0; y < rows; ++y) {
			for (int x = 0; x < columns; ++x) {
				Vector3 coords = new Vector3(x * pieceWidth + pieceWidth / 2 - width / 2,
																			y * pieceHeight + pieceHeight / 2 - height / 2,
																			0);
				GameObject piece = GameObject.Instantiate(piecePrefab, coords, Quaternion.identity) as GameObject;
				piece.transform.localScale = new Vector3(width / columns, height / rows, 1);
				piece.transform.parent = parent;
				objects[x, y] = piece;
			}
		}
	}

	public void FinishPiece() {
		for (int y = 0; y < rows; ++y) {
			for (int x = 0; x < columns; ++x) {
				PieceCtrl currentPiece = objects[x, y].GetComponent<PieceCtrl>();
				if (currentPiece.IsCurrent()) {
					currentPiece.MakeFull();
				}
			}
		}
	}

	public void FallPieces(PieceState state) {
		for (int y = 1; y < rows; ++y) {
			for (int x = 0; x < columns; ++x) {
				PieceCtrl currentPiece = objects[x, y].GetComponent<PieceCtrl>();
				PieceCtrl nextPiece = objects[x, y - 1].GetComponent<PieceCtrl>();
				if (currentPiece.state == state) {
					nextPiece.Replace(currentPiece);
				}
			}
		}
	}

	public void AddCurrent(PieceType type, int x, int y) {
		GameObject newPiece = objects[x, y];
		PieceCtrl pieceCtrl = newPiece.GetComponent<PieceCtrl>();
		pieceCtrl.MakeCurrent();
		pieceCtrl.SetType(type);
	}
}
