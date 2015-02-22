using UnityEngine;
using System.Collections;
using Constants;

class PieceDescription {
	public int width;
	public int height;
	bool[,] pieceGrid;

	public PieceDescription(int width, int height) {
		this.width  = width;
		this.height = height;
		pieceGrid = new bool[width, height];
		for (int x = 0; x < width; ++x) {
			for(int y = 0; y < height; ++y) {
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
		bool[,] newGrid = new bool[height, width];
		for (int x = 0; x < height; ++x) {
			for(int y = 0; y < width; ++y) {
				newGrid[x, y] = pieceGrid[y, x];
			}
		}

		int newWidth = height;
		height = width;
		width = newWidth;
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

	public void AddNext(GameObject[,] grid) {
		PieceDescription current = Next ();
		int xStart = (grid.GetLength (1) / 2) - (current.width / 2);

		while (nextRotation > 0) {
			current.Rotate();
			nextRotation--;
		}

		for (int x = 0; x < current.width; ++x) {
			for (int y = 0; y < current.height; ++y) {
				if (current.CheckFilled(x, y)) {
					int xBoard = x + xStart;
					int yBoard = grid.GetLength (1)-1 - y;

					GameObject newPiece = grid[xBoard, yBoard];
					newPiece.GetComponent<PieceCtrl>().UpdateState(PieceState.Current);
				}
			}
		}

		RollNext();
	}

	void SetupTemplates() {
		PieceDescription smallSquare = new PieceDescription (1, 1).Fill (0, 0);
		PieceDescription largeSquare = new PieceDescription (2, 2).Fill (0, 0).Fill (0, 1).Fill (1, 0).Fill (1, 1);
		PieceDescription lLeft       = new PieceDescription (3, 2).Fill (0, 0).Fill (1, 0).Fill (2, 0).Fill (0, 1);
		PieceDescription lRight      = new PieceDescription (3, 2).Fill (0, 0).Fill (1, 0).Fill (2, 0).Fill (2, 1);
		PieceDescription t           = new PieceDescription (3, 3).Fill (0, 0).Fill (1, 0).Fill (2, 0).Fill (1, 1).Fill (1, 2);
		PieceDescription i           = new PieceDescription (1, 3).Fill (0, 0).Fill (0, 1).Fill (0, 2);

		templates = new PieceDescription[] { smallSquare, largeSquare, lLeft, lRight, t, i };
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
