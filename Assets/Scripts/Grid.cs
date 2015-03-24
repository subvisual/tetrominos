using System;
using UnityEngine;
using System.Collections.Generic;
using Constants;
using System.Linq;
using System.Linq.Expressions;

public class Grid {

	readonly GameObject _piecePrefab, _separatorPrefab;
	private GameObject[,] _objects;
	private GameObject[] _lines;

	readonly int _columns, _rows;
	readonly float _width, _height;
	readonly float _pieceWidth, _pieceHeight;

	public int Columns { get { return _columns; } }
	public int Rows    { get { return _rows; } }

	public Grid (GameObject piecePrefab, GameObject separatorPrefab, Transform parent, int columns, int rows, float width, float height) {
		_piecePrefab = piecePrefab;
		_separatorPrefab = separatorPrefab;
		_columns = columns;
		_rows = rows;
		_width = width;
		_height = height;
		_pieceWidth = width / (float) columns;
		_pieceHeight = height / (float) rows;
		SetupGrid(parent.Find("Pieces"));
		SetupLines(parent.Find("Separators"));
	}

	public bool MoveRight() {
		return Move(PieceState.Current, 1, 0);
	}

	public bool MoveLeft() {
		return Move(PieceState.Current, -1, 0);
	}

	public bool Rotate() {
		var attempts = new int[][] {
			new int[] { 0, 0},
			new int[] {-1, 0},
			new int[] { 1, 0},
		};

		foreach (var attempt in attempts) {
			if (RotateCurrentLeft(attempt[0], attempt[1])) {
				return true;
			}
		}

		return false;
	}

	public bool Fall(PieceState state) {
		return Move(state, 0, -1);
	}

	public void FinishPiece() {
		ForEachPiece((x, y, currentPiece) => {
			if (currentPiece.IsCurrent()) {
				currentPiece.MakeFull();
			}
			return true;
		});
	}

	public void AddCurrent(PieceType type, int rotation, int x, int y) {
		var pieceCtrl = GetPieceCtrl(x, y);
		pieceCtrl.MakeCurrent();
		pieceCtrl.SetType(type);
		for (; rotation > 0; --rotation) {
			pieceCtrl.Rotate();
		}
	} 
	
	public int DestroyFullRows() {
		var fullCount = 0;

		for (var y = 0; y < _rows; ++y) {
			var isFull = true;
			for (var x = 0; x < _columns; ++x) {
				var currentPiece = GetPieceCtrl(x, y);
				if (!currentPiece.IsFull()) {
					isFull = false;
					break;
				}
			}

			if (isFull) { // if this row is full, empty it
				fullCount++;
				for (var x = 0; x < _columns; ++x) {
					var currentPiece = GetPieceCtrl(x, y);
					currentPiece.MakeEmpty();
				}

			} else if (fullCount > 0) { //otherwise, collapse all full pieces by `fullCount` rows
				for (var x = 0; x < _columns; ++x) {
					var currentPiece = GetPieceCtrl(x, y);
					if (currentPiece.IsFull()) {
						var newPiece = GetPieceCtrl(x, y - fullCount);
						newPiece.Replace(currentPiece);
					}
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
					return false;
				}
			}
		}
		return true;
	}
	
	bool Move(PieceState state, int offX, int offY) {
		var oldCoords = new List<Triple<int, int, PieceType>>();
		var newCoords = new List<Triple<int, int, PieceType>>();
		int rotation = 0;

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
			rotation = currentPiece.Rotation;

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
		SetCoords(coordsToAdd,    state, rotation);
		SetCoords(coordsToRemove, PieceState.Empty, 0);
		return true;
	}

	void SetCoords(IEnumerable<Triple<int, int, PieceType>> coordsInfo, PieceState state, int rotation) {
		foreach(var coordInfo in coordsInfo) {
			var piece = GetPieceCtrl(coordInfo.First, coordInfo.Second);
			piece.SetType(coordInfo.Third);
			piece.Make(state);
			piece.ResetRotation();
			piece.Rotate(rotation);
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

	void SetupLines(Transform parent) {
		_lines = new GameObject[Columns + Rows + 2];

		for (var y = 0; y <= _rows; ++y) {
			var coords = new Vector3(0, y * _pieceHeight - (_height / 2), 0);
			var separator = GameObject.Instantiate(_separatorPrefab, coords, Quaternion.identity) as GameObject;
			separator.transform.localScale = new Vector3(1, _width, 1);
			separator.transform.Rotate(0, 0, 90);
			separator.transform.SetParent(parent, false);
			_lines[y] = separator;
		}

		for (var x = 0; x <= _columns; ++x) {
			var coords = new Vector3(x * _pieceWidth - (_width / 2), 0, 0);
			var separator = GameObject.Instantiate(_separatorPrefab, coords, Quaternion.identity) as GameObject;
			separator.transform.localScale = new Vector3(1, _height, 1);
			separator.transform.SetParent(parent, false);
			_lines[x + _rows] = separator;
		}
	}

	PieceCtrl GetPieceCtrl(int x, int y) {
		if (x < 0 || x >= _objects.GetLength(0) || y < 0 || y >= _objects.GetLength(1)) {
			return null;
		} else {
			return _objects[x, y].GetComponent<PieceCtrl>();
		}
	}

	PieceCtrl GetPieceCtrl(Pair<int, int> coords) {
		return GetPieceCtrl(coords.First, coords.Second);
	}

	private IEnumerable<Pair<int, int>> GetCurrentCoords() {
		var result = new List<Pair<int, int>>();

		ForEachPiece((x, y, piece) => {
			if (piece.IsCurrent()) {
				result.Add(new Pair<int, int>(x, y));
			}
			return true;
		});

		return result;
	}

	private bool RotateCurrentLeft(int offX, int offY) {
		var oldCoords = GetCurrentCoords().ToList();

		// find gravity center
		var gravityCenter = Vector3.zero;
		foreach (var coord in oldCoords) {
			gravityCenter.x += coord.First;
			gravityCenter.y += coord.Second;
		}
		gravityCenter = gravityCenter/(float) oldCoords.Count();

		// we want to rotate 90 degrees clockwise
		// this is equivalent to multiplying the vector with the matrix:
		//  | cos(90)    - sin(90) | 
		//  | sin(90)      cos(90) |
		// which is equivalent to
		//  | 0    -1 |
		//  | 1     0 |
		// so rotating (x, y) yields (-y, x)
		var rotatedCoords = oldCoords.Select(coord => {
			var rotationVector = new Vector3(coord.First, coord.Second, 0) - gravityCenter;
			var rotatedVector = new Vector3(-rotationVector.y, rotationVector.x, 0) + gravityCenter;
			var newX = offX + Math.Round(rotatedVector.x, MidpointRounding.AwayFromZero);
			var newY = offY + Math.Round(rotatedVector.y, MidpointRounding.AwayFromZero);
			return new Pair<int, int>((int) newX, (int) newY);
		}).ToList();

		// first we try the raw rotated coods
		if (!IsLegalRotation(rotatedCoords)) {
			return false;
		}

		ApplyRotation(oldCoords, rotatedCoords);
		return true;
	}

	bool IsLegalRotation(IEnumerable<Pair<int, int>> coords) {
		return !coords.Any(coord => {
			var ctrl = GetPieceCtrl(coord);
			return ctrl == null || ctrl.IsFull();
		});
	}

	void ApplyRotation(IEnumerable<Pair<int, int>> oldCoords, IEnumerable<Pair<int, int>> newCoords) {
		foreach(var coords in GetCurrentCoords()) {
			GetPieceCtrl(coords).Rotate();
		}

		// all pieces from oldCoords are the same type, we just need one of them
		var type = GetPieceCtrl(oldCoords.First()).Type;
		var rotation = GetPieceCtrl(oldCoords.First()).Rotation;

		foreach (var oldCoord in oldCoords) {
			GetPieceCtrl(oldCoord).MakeEmpty();
		}

		foreach (var newCoord in newCoords) {
			var ctrl = GetPieceCtrl(newCoord);
			ctrl.SetType(type);
			ctrl.ResetRotation();
			ctrl.Rotate(rotation);
			GetPieceCtrl(newCoord).MakeCurrent();
		}
	}
}
