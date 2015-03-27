using UnityEngine;
using System.Linq;

public class Movement : GridBehaviour {

	private InputCtrl _input;
	private PieceCtrl _piece;

	// Use this for initialization
	void Awake() {
		_input = GetComponent<InputCtrl>();
	}
	
	// Update is called once per frame
	void Update () {
		_piece = CurrentPiece();

		if (_input.IsMovingLeft()) {
			MoveLeft();
		} else if (_input.IsMovingRight()) {
			MoveRight();
		}
	}

	void MoveLeft() {
		if (CanMoveLeft()) {
			_piece.transform.Translate(Vector3.left * _piece.transform.localScale.x);
		}
	}

	void MoveRight() {
		if (CanMoveRight()) {
			_piece.transform.Translate(Vector3.right * _piece.transform.localScale.x);
		}
	}

	bool CanMoveLeft() {
		var currentCoords = _piece.PartPositions();
		var nextCoords = currentCoords.Select(coord => coord + (Vector3.left * _piece.transform.localScale.x)).ToList();

		return nextCoords.All(IsWithinBounds) && nextCoords.All(IsCoordFree);
	}

	bool CanMoveRight() {
		var currentCoords = _piece.PartPositions();
		var nextCoords = currentCoords.Select(coord => coord + (Vector3.right * _piece.transform.localScale.x));

		return nextCoords.All(IsWithinBounds) && nextCoords.All(IsCoordFree);
	}
}
