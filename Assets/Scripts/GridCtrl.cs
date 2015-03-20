using UnityEngine;
using System.Collections;
using Constants;

public class GridCtrl : MonoBehaviour {

	public int Columns;
	public int Rows;
	public float FallDelay, FallTurbo;
	public GameObject PiecePrefab;
	public GameObject SeparatorPrefab;
	public Grid Grid;

	private IEnumerator _fallRoutine;
	private GridCommandsCtrl _commandsCtrl;

	void Awake() {
		_commandsCtrl = GetComponent<GridCommandsCtrl>();
	}

	void Start() {
		var height = Camera.main.orthographicSize * 2f;
		var width = height * Camera.main.aspect;
		Grid = new Grid(PiecePrefab, SeparatorPrefab, transform, Columns, Rows, width, height);
		_fallRoutine = FallAndWait();
		GetComponent<PieceFactory>().AddNext(Grid);
		StartCoroutine(_fallRoutine);
	}

	void Update() {
		if (_commandsCtrl.IsMovingRight()) {
			Grid.MoveRight();
		} else if (_commandsCtrl.IsMovingLeft()) {
			Grid.MoveLeft();
		} else if (_commandsCtrl.IsRotating()) {
			Grid.Rotate();
		}

		if (_commandsCtrl.IsEnteringTurboMode()) {
			FallDelay /= FallTurbo;
			ResetFallCoroutine();
		}

		if (_commandsCtrl.IsExitingTurboMode()) {
			FallDelay *= FallTurbo;
			ResetFallCoroutine();
		}
	}

	IEnumerator FallAndWait() {
		while (true) {
			// refactor this out of here
			if (!Grid.Fall(PieceState.Current)) {
				Grid.FinishPiece();
				GetComponent<PieceFactory>().AddNext(Grid);
			}
			
			Grid.DestroyFullRows();

			yield return new WaitForSeconds(FallDelay);
		}
		yield return true;
	}

	void ResetFallCoroutine() {
		StopCoroutine(_fallRoutine);
		StartCoroutine(_fallRoutine);
	}
}
