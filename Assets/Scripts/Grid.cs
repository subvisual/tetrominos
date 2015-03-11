using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Constants;
using System.Linq;

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

	public bool MoveRight() {
		return Move(PieceState.Current, 1, 0);
	}

	public bool MoveLeft() {
		return Move(PieceState.Current, -1, 0);
	}

	public bool Fall(PieceState state) {
		return Move(state, 0, -1);
	}

	public void CollapseFull() {
		while (Move(PieceState.Full, 0, -1))
			;
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

	public void AddCurrent(PieceType type, int x, int y) {
		GameObject newPiece = objects[x, y];
		PieceCtrl pieceCtrl = newPiece.GetComponent<PieceCtrl>();
		pieceCtrl.MakeCurrent();
		pieceCtrl.SetType(type);
	}

	public int DestroyFullRows() {
		int fullCount = 0;

		for (int y = 0; y < rows; ++y) {
			bool isFull = true;
			for (int x = 0; x < columns; ++x) {
				PieceCtrl currentPiece = objects[x, y].GetComponent<PieceCtrl>();
				if (!currentPiece.IsFull()) {
					isFull = false;
					break;
				}
			}
			if (isFull) {
				fullCount++;
				for (int x = 0; x < columns; ++x) {
					PieceCtrl currentPiece = objects[x, y].GetComponent<PieceCtrl>();
					currentPiece.MakeEmpty();
				}
			}
		}

		return fullCount;
	}

	bool Move(PieceState state, int offX, int offY) {
		var oldCoords = new List<Pair<int, int>>();
		var newCoords = new List<Pair<Pair<int, int>, PieceType>>();

		for (int y = 0; y < rows; ++y) {
			for (int x = 0; x < columns; ++x) {
				PieceCtrl currentPiece = objects[x, y].GetComponent<PieceCtrl>();

				// if we're not interested in this piece
				if (currentPiece.state != state) {
					continue;
				}

				int nextX = x + offX;
				int nextY = y + offY;

				// check if nextPiece is at the boundaries
				// if not, the move can't happen
				if ((nextX < 0 || nextX >= columns) ||
						(nextY < 0 || nextY >= rows)) {
					return false;
				}

				// check if nextPiece is Full
				PieceCtrl nextPiece = objects[nextX, nextY].GetComponent<PieceCtrl>();
				if (nextPiece.state == PieceState.Full) {
					if (state == PieceState.Current) {
						return false; // If we're moving the current piece, we cancel. It's all or nothing
					} else {
						continue; // The Full pieces can move independently, so we just skip this one
					}
				}

				// otherwise, use the new coords
				oldCoords.Add(new Pair<int, int>(x, y));
				newCoords.Add(new Pair<Pair<int, int>, PieceType>(new Pair<int, int>(nextX, nextY), currentPiece.type));
			}
		}

		var coordsToAdd    = newCoords.Where(newCoord => !oldCoords.Contains(newCoord.First));
		var coordsToRemove = oldCoords.Where(oldCoord => !newCoords.Any(newCoord => newCoord.First.Equals(oldCoord)));

		if (coordsToAdd.Count() == 0) {
			return false;
		}

		foreach(Pair<Pair<int, int>, PieceType> coord in coordsToAdd.ToList()) {
			var x = coord.First.First;
			var y = coord.First.Second;
			var type = coord.Second;
			var piece = objects[x, y].GetComponent<PieceCtrl>();
			piece.SetType(type);
			piece.Make(state);
		}

		foreach (Pair<int, int> coord in coordsToRemove.ToList()) {
			var piece = objects[coord.First, coord.Second].GetComponent<PieceCtrl>();
			piece.MakeEmpty();
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
}
