using UnityEngine;
using System.Collections;
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
}

public class PieceFactory : MonoBehaviour {
	private PieceDescription[] _templates;
	private int _nextIndex;
	private int _nextRotation;

	// Use this for initialization
	void Awake () {
		SetupTemplates ();
		RollNext();
	}

	public void AddNext(Grid grid) {
		var current = Next ();
		var xStart = (grid.Columns / 2) - (current.Columns / 2);

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
		//PieceDescription smallSquare = new PieceDescription(PieceType.SmallSquare, 1, 1).Fill (0, 0);
		var largeSquare = new PieceDescription(PieceType.LargeSquare, 2, 2).Fill(0, 0).Fill(0, 1).Fill(1, 0).Fill(1, 1);
		var lLeft       = new PieceDescription(PieceType.LLeft,       3, 2).Fill(0, 0).Fill(1, 0).Fill(2, 0).Fill(0, 1);
		var lRight      = new PieceDescription(PieceType.LRight,      3, 2).Fill(0, 0).Fill(1, 0).Fill(2, 0).Fill(2, 1);
		var t           = new PieceDescription(PieceType.T,           3, 2).Fill(0, 0).Fill(1, 0).Fill(2, 0).Fill(1, 1);
		var i           = new PieceDescription(PieceType.I,           1, 4).Fill(0, 0).Fill(0, 1).Fill(0, 2).Fill(0, 3);
		var sLeft       = new PieceDescription(PieceType.S,           2, 3).Fill(0, 0).Fill(0, 1).Fill(1, 1).Fill(1, 2);
		var sRight      = new PieceDescription(PieceType.S,           2, 3).Fill(1, 0).Fill(1, 1).Fill(0, 1).Fill(0, 2);

		_templates = new []
		{
			/*smallSquare, */
			largeSquare,
			lLeft,
			//lRight,
			t,
			i,
			sLeft,
			//sRight
		};
		
	}

	PieceDescription Next() {
		return _templates[_nextIndex];
	}

	void RollNext() {
		_nextIndex = Random.Range(0, _templates.GetLength(0));
		_nextRotation = Random.Range (0, 4);
	}
}
