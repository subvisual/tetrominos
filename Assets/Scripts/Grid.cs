using UnityEngine;
using System.Collections.Generic;
using Constants;
using System.Linq;

public class Grid {

	readonly GameObject _piecePrefab;
	GameObject[,] _objects;

	readonly int _columns, _rows;
	readonly float _width, _height;
	readonly float _pieceWidth, _pieceHeight;

	public int Columns { get { return _columns; } }
	public int Rows    { get { return _rows; } }

	public Grid (GameObject piecePrefab, Transform parent, int columns, int rows, float width, float height) {
		_piecePrefab = piecePrefab;
		_columns = columns;
		_rows = rows;
		_width = width;
		_height = height;
		_pieceWidth = width / columns;
		_pieceHeight = height / rows;
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
		while (Move(PieceState.Full, 0, -1)) {
		}
	}

	public void FinishPiece() {
		ForEachPiece((x, y, currentPiece) => {
			if (currentPiece.IsCurrent()) {
				currentPiece.MakeFull();
			}
			return true;
		});
	}

	public void AddCurrent(PieceType type, int x, int y) {
		var pieceCtrl = GetPieceCtrl(x, y);
		pieceCtrl.MakeCurrent();
		pieceCtrl.SetType(type);
	}

	public int DestroyFullRows() {
		int fullCount = 0;

		for (int y = 0; y < _rows; ++y) {
			bool isFull = true;
			for (int x = 0; x < _columns; ++x) {
				PieceCtrl currentPiece = GetPieceCtrl(x, y);
				if (!currentPiece.IsFull()) {
					isFull = false;
					break;
				}
			}
			if (isFull) {
				fullCount++;
				for (int x = 0; x < _columns; ++x) {
					PieceCtrl currentPiece = GetPieceCtrl(x, y);
					currentPiece.MakeEmpty();
				}
			}
		}

		return fullCount;
	}
	
	// Iterates all pieces
	// If one of the delegates returns false, all subsequent iterations are cancelled
	delegate bool PieceFunc(int x, int y, PieceCtrl piece);
	bool ForEachPiece(PieceFunc func) {
		for (var y = 0; y < _rows; ++y) {
			for (var x = 0; x < _columns; ++x) {
				var currentPiece = GetPieceCtrl(x, y);

				if (!func(x, y, currentPiece)) {
					Debug.Log("canceling");
					return false;
				}
			}
		}
		return true;
	}
	
	bool Move(PieceState state, int offX, int offY) {
		var oldCoords = new List<Triple<int, int, PieceType>>();
		var newCoords = new List<Triple<int, int, PieceType>>();

		var moveAllowed = ForEachPiece((x, y, currentPiece) => {
			// if we're not interested in this piece
			if (currentPiece.State != state) {
				return true;
			}

			int nextX = x + offX;
			int nextY = y + offY;

			// check if nextPiece is at the boundaries. if not, the move can't happen
			if (nextX < 0 || nextX >= _columns || nextY < 0 || nextY >= _rows) {
				// If we're moving the current piece, we cancel. It's all or nothing
				// The Full pieces can move independently, so we just skip this one
				return (state != PieceState.Current);
			}

			// check if nextPiece is Full
			PieceCtrl nextPiece = GetPieceCtrl(nextX, nextY);
			if (nextPiece.State == PieceState.Full) {
				// If we're moving the current piece, we cancel. It's all or nothing
				// The Full pieces can move independently, so we just skip this one
				return (state != PieceState.Current);
			}

			// otherwise, use the new coords
			oldCoords.Add(new Triple<int, int, PieceType>(x, y, PieceType.Empty));
			newCoords.Add(new Triple<int, int, PieceType>(nextX, nextY, currentPiece.Type));

			return true;
		});

		if (!moveAllowed) {
			return false;
		}

		// diff both lists

		var coordsToAdd    = newCoords.Where(newCoord => !oldCoords.Any(oldCoord => newCoord.PairXY().Equals(oldCoord.PairXY()))).ToList();
		var coordsToRemove = oldCoords.Where(oldCoord => !newCoords.Any(newCoord => newCoord.PairXY().Equals(oldCoord.PairXY()))).ToList();

		if (!coordsToAdd.Any()) {
			return false;
		}

		// apply both diffs
		SetCoords(coordsToAdd,    state);
		SetCoords(coordsToRemove, PieceState.Empty);
		return true;
	}

	void SetCoords(IEnumerable<Triple<int, int, PieceType>> coordsInfo, PieceState state) {
		foreach(var coordInfo in coordsInfo) {
			var piece = GetPieceCtrl(coordInfo.First, coordInfo.Second);
			piece.SetType(coordInfo.Third);
			piece.Make(state);
		}
	}

	void SetupGrid(Transform parent) {
		_objects = new GameObject[Columns, Rows];

		for (var y = 0; y < _rows; ++y) {
			for (var x = 0; x < _columns; ++x) {
				var coords = new Vector3((x + 0.5f) * _pieceWidth - 0.5f * _width,
																 (y + 0.5f) * _pieceHeight - 0.5f * _height,
																	0);
				var piece = GameObject.Instantiate(_piecePrefab, coords, Quaternion.identity) as GameObject;
				piece.transform.localScale = new Vector3(_width / _columns, _height / _rows, 1);
				piece.transform.parent = parent;
				_objects[x, y] = piece;
			}
		}
	}

	PieceCtrl GetPieceCtrl(int x, int y) {
		return _objects[x, y].GetComponent<PieceCtrl>();
	}
}
