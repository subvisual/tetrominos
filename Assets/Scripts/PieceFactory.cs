using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using Constants;

class PieceDescription {
	public int Columns;
	public int Rows;
	public PieceType Type;
	public int Rotation;
	bool[,] _pieceGrid;

	public PieceDescription(PieceType type, int columns, int rows) {
		Type = type;
		Columns  = columns;
		Rows = rows;
		Rotation = 0;
		_pieceGrid = new bool[columns, rows];
		for (int x = 0; x < columns; ++x) {
			for(int y = 0; y < rows; ++y) {
				_pieceGrid[x, y] = false;
			}
		}
	}

	public PieceDescription Fill(int x, int y) {
		_pieceGrid[x, y] = true;
		return this;
	}

	public bool CheckFilled(int x, int y) {
		return _pieceGrid [x, y];
	}

	public PieceDescription Rotate() {
		++Rotation; 
		bool[,] newGrid = new bool[Rows, Columns];
		for (int x = 0; x < Rows; ++x) {
			for(int y = 0; y < Columns; ++y) {
				newGrid[x, y] = _pieceGrid[y, x];
			}
		}

		var tmp = Rows;
		Rows = Columns;
		Columns = tmp;
		_pieceGrid = newGrid;

		return this;
	}

	public PieceDescription Transpose() {
		for (int x = 0; x < Rows; ++x) {
			for (int y = 0; y < Columns/2; ++y) {
				var otherY = Columns - 1 - y;
				var tmp = _pieceGrid[x, y];
				_pieceGrid[x, y] = _pieceGrid[x, otherY];
				_pieceGrid[x, otherY] = tmp;
			}
		}
		return this;
	}
}

public class PieceFactory : MonoBehaviour {
	private PieceDescription[] _templates;
	private int _nextIndex;
	private int _nextRotation;
	private bool _nextTranspose;

	// Use this for initialization
	void Awake () {
		SetupTemplates ();
		RollNext();
	}

	public void AddNext(Grid grid) {
		var current = Next ();
		var xStart = (grid.Columns / 2) - (current.Columns / 2);

		if (_nextTranspose) {
			current.Transpose();
		}

		for (var i = 0; i < _nextRotation; ++i) {
			current.Rotate();
		}

		for (var x = 0; x < current.Columns; ++x) {
			for (var y = 0; y < current.Rows; ++y) {
				if (current.CheckFilled(x, y)) {
					var xBoard = x + xStart;
					var yBoard = grid.Rows - 1 - y;
					grid.AddCurrent(current.Type, current.Rotation, xBoard, yBoard);
				}
			}
		}

		RollNext();
	}

	void SetupTemplates() {
		var square = new PieceDescription(PieceType.Square, 2, 2).Fill(0, 0).Fill(0, 1).Fill(1, 0).Fill(1, 1);
		var l      = new PieceDescription(PieceType.L,      3, 2).Fill(0, 0).Fill(1, 0).Fill(2, 0).Fill(0, 1);
		var t      = new PieceDescription(PieceType.T,      3, 2).Fill(0, 0).Fill(1, 0).Fill(2, 0).Fill(1, 1);
		var i      = new PieceDescription(PieceType.I,      1, 4).Fill(0, 0).Fill(0, 1).Fill(0, 2).Fill(0, 3);
		var s      = new PieceDescription(PieceType.S,      2, 3).Fill(0, 0).Fill(0, 1).Fill(1, 1).Fill(1, 2);

		_templates = new []
		{
			square,
			l,
			t,
			i,
			s,
		};
		
	}

	PieceDescription Next() {
		return _templates[_nextIndex];
	}

	void RollNext() {
		_nextIndex = Random.Range(0, _templates.GetLength(0));
		_nextRotation = Random.Range (0, 4);
		_nextTranspose = Random.Range(0, 1) == 1;
	}
}
