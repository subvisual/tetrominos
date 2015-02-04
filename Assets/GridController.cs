using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {

	public int x;
	public int y;
	public float w, h;
	public float fallDelay;
	public GameObject piecePrefab;

	private int fallingPieceX, fallingPieceY;
	private float pieceW, pieceH;
	private GameObject[,] grid;


	// Use this for initialization
	void Start () {
		pieceW = w / x;
		pieceH = h / y;
		grid = new GameObject[x, y];
		SetupPiece (1, y-1);
		fallingPieceX = 1;
		fallingPieceY = y - 1;
		StartCoroutine (Fall ());
	}

	void Update() {
		if (fallingPieceY == 0) {
			return;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			MoveRight();
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			MoveLeft();
		}
	}

	void MoveRight() {
		if (fallingPieceX == x - 1) {
			return;
		}
		if (grid[fallingPieceX + 1, fallingPieceY] == null) {
			grid[fallingPieceX + 1, fallingPieceY] = grid[fallingPieceX, fallingPieceY];
			grid[fallingPieceX, fallingPieceY] = null;
			grid[fallingPieceX + 1, fallingPieceY].transform.Translate(Vector3.right * pieceW);
			fallingPieceX++; 
		}
	}
	
	void MoveLeft() {
		if (fallingPieceX == 0) {
			return;
		}
		if (grid[fallingPieceX - 1, fallingPieceY] == null) {
			grid[fallingPieceX - 1, fallingPieceY] = grid[fallingPieceX, fallingPieceY];
			grid[fallingPieceX, fallingPieceY] = null;
			grid[fallingPieceX - 1, fallingPieceY].transform.Translate(Vector3.left * pieceW);
			fallingPieceX--; 
		}
	}

	void SetupPiece(int xi, int yi) {
		GameObject piece = Instantiate (piecePrefab, new Vector3 (xi * pieceW + pieceW/2 - w/2, yi * pieceH + pieceH/2 - h/2, 0), Quaternion.identity) as GameObject;
		piece.transform.localScale = new Vector3 (w / x, h / y, 1);	
		piece.transform.parent = transform;
		grid [xi, yi] = piece;
	}

	IEnumerator Fall() {
		while(true) {
			for (int xi = 0; xi < x; ++xi) {
				for (int yi = 1; yi < y; ++yi) {
					if (grid [xi, yi]) {
						if (grid[xi, yi-1] == null) {
							grid[xi, yi-1] = grid[xi, yi];
							grid[xi, yi] = null;
							grid[xi, yi-1].transform.Translate(Vector3.down * pieceH);
							fallingPieceY--; 
						}
					}
				}
			}
			yield return new WaitForSeconds(fallDelay);
		}
		yield return true;
	}
}
