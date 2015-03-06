using UnityEngine;
using System.Collections;
using Constants;

class PieceDescription {
	public int columns;
	public int rows;
	public PieceType type;
	bool[,] pieceGrid;

	public PieceDescription(PieceType type, int columns, int rows) {
		this.type = type;
		this.columns  = columns;
		this.rows = rows;
		pieceGrid = new bool[columns, rows];
		for (int x = 0; x < columns; ++x) {
			for(int y = 0; y < rows; ++y) {
				pieceGrid[x, y] = false;
			}
		}
	}

	public PieceDescription Fill(int x, int y) {
		pieceGrid[x, y] = true;
		return this;
	}

	public bool CheckFilled(int x, int y) {
		return pieceGrid [x, y];
	}

	public PieceDescription Rotate() {
		bool[,] newGrid = new bool[rows, columns];
		for (int x = 0; x < rows; ++x) {
			for(int y = 0; y < columns; ++y) {
				newGrid[x, y] = pieceGrid[y, x];
			}
		}

		int tmp = rows;
		rows = columns;
		columns = tmp;
		pieceGrid = newGrid;

		return this;
	}
}

public class PieceFactory : MonoBehaviour {
	private PieceDescription[] templates;

	private int nextIndex;
	private int nextRotation;

	// Use this for initialization
	void Awake () {
		SetupTemplates ();
		RollNext();
	}

	public void AddNext(Grid grid) {
		PieceDescription current = Next ();
		int xStart = (grid.columns / 2) - (current.columns/ 2);

		while (nextRotation > 0) {
			current.Rotate();
			nextRotation--;
		}

		for (int x = 0; x < current.columns; ++x) {
			for (int y = 0; y < current.rows; ++y) {
				if (current.CheckFilled(x, y)) {
					int xBoard = x + xStart;
					int yBoard = grid.rows - 1 - y;

					grid.AddCurrent(current.type, xBoard, yBoard);
				}
			}
		}

		RollNext();
	}

	void SetupTemplates() {
		//PieceDescription smallSquare = new PieceDescription(PieceType.SmallSquare, 1, 1).Fill (0, 0);
		PieceDescription largeSquare = new PieceDescription(PieceType.LargeSquare, 2, 2).Fill(0, 0).Fill(0, 1).Fill(1, 0).Fill(1, 1);
		PieceDescription lLeft       = new PieceDescription(PieceType.LLeft,       3, 2).Fill(0, 0).Fill(1, 0).Fill(2, 0).Fill(0, 1);
		PieceDescription lRight      = new PieceDescription(PieceType.LRight,      3, 2).Fill(0, 0).Fill(1, 0).Fill(2, 0).Fill(2, 1);
		PieceDescription t           = new PieceDescription(PieceType.T,           3, 2).Fill(0, 0).Fill(1, 0).Fill(2, 0).Fill(1, 1);
		PieceDescription i           = new PieceDescription(PieceType.I,           1, 4).Fill(0, 0).Fill(0, 1).Fill(0, 2).Fill(0, 3);
		PieceDescription s           = new PieceDescription(PieceType.S,           3, 2).Fill(0, 1).Fill(0, 1).Fill(1, 1).Fill(2, 0);

		templates = new PieceDescription[] { /*smallSquare, */largeSquare, lLeft, lRight, t, i };
	}

	PieceDescription Next() {
		return templates[nextIndex];
	}

	void RollNext() {
		nextIndex = Random.Range(0, templates.GetLength(0));
		nextRotation = Random.Range (0, 4);
	}

	void Coord(int x, int y) {
		new ArrayList (new[] { x, y });
	}
}
