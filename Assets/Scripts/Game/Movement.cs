using UnityEngine;
using System.Linq;

public class Movement : GridBehaviour {

	private InputCtrl _input;
	private PieceCtrl _piece;

	void Awake() {
		_input = GetComponent<InputCtrl>();
	}
	
	void Update() {
		_piece = CurrentPiece();

		if (_input.IsMovingLeft()) {
			MoveLeft();
		} else if (_input.IsMovingRight()) {
			MoveRight();
		}

		if (_input.IsRotating()) {
			Rotate();
		}
	}

	void MoveLeft() {
		if (CanMoveLeft()) {
			_piece.transform.Translate(Vector3.left * _piece.Width(), Space.World);
		}
	}

	void MoveRight() {
		if (CanMoveRight()) {
			_piece.transform.Translate(Vector3.right * _piece.Width(), Space.World);
		}
	}

	void Rotate() {
		if (CanRotate()) {
			_piece.Rotate();
		} else if (CanRotate(-1)) {
			_piece.Rotate(-1);
		} else if (CanRotate(1)) {
			_piece.Rotate(1);
		}
	}

	bool CanMoveLeft() {
		var currentCoords = _piece.PartPositions();
		var nextCoords = currentCoords.Select(coord => coord + (Vector3.left * _piece.Width())).ToList();

		return nextCoords.All(IsWithinSpawnBounds) && nextCoords.All(IsCoordFree);
	}

	bool CanMoveRight() {
		var currentCoords = _piece.PartPositions();
		var nextCoords = currentCoords.Select(coord => coord + (Vector3.right * _piece.Width())).ToList();

		return nextCoords.All(IsWithinSpawnBounds) && nextCoords.All(IsCoordFree);
	}

	bool CanRotate(int offset = 0) {
		_piece.Rotate(offset);
		var nextCoords = _piece.PartPositions();
		_piece.Unrotate(offset);

		return nextCoords.All(IsWithinSpawnBounds) && nextCoords.All(IsCoordFree);
	}
}
