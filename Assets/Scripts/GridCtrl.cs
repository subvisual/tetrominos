using UnityEngine;
using System.Collections;
using Constants;

public class GridCtrl : MonoBehaviour {

	public int Columns;
	public int Rows;
	public float Width { get; private set; }
	public float Height { get; private set; }
	public float PieceWidth { get; private set; }
	public float PieceHeight { get; private set; }
	public Rect Boundaries { get; private set; }
	//public float FallDelay, FallTurbo;
	//public GameObject PiecePrefab;
	//public GameObject SeparatorPrefab;
	//public Grid Grid;

	//private IEnumerator _fallRoutine;
	//private GridCommandsCtrl _commandsCtrl;
	//private bool _isInTurbo;

	void Awake() {
		Height = Camera.main.orthographicSize * 2f;
		Width = Height * Camera.main.aspect;
		PieceWidth = Width / (float) Columns;
		PieceHeight = Height / (float) Rows;
		Boundaries = new Rect(- Width * 0.5f, - Height * 0.5f, Width, Height );
		//_commandsCtrl = GetComponent<GridCommandsCtrl>();
		//_isInTurbo = false;
	}

	void Start() {
		//Grid = new Grid(PiecePrefab, SeparatorPrefab, transform, Columns, Rows, width, height);
		//_fallRoutine = FallAndWait();
		//GetComponent<PieceFactory>().AddNext(Grid);
		//StartCoroutine(_fallRoutine);
	}

	void Update() {
		//if (_commandsCtrl.IsMovingRight()) {
		//	Grid.MoveRight();
		//} else if (_commandsCtrl.IsMovingLeft()) {
		//	Grid.MoveLeft();
		//} else if (_commandsCtrl.IsRotating()) {
		//	Grid.Rotate();
		//}

		//if (_commandsCtrl.IsInTurboMode() && !_isInTurbo) {
		//	_isInTurbo = true;
		//	FallDelay /= FallTurbo;
		//	ResetFallCoroutine();
		//} else if (!_commandsCtrl.IsInTurboMode() && _isInTurbo) {
		//	_isInTurbo = false;
		//	FallDelay *= FallTurbo;
		//	ResetFallCoroutine();
		//}
	}

	//IEnumerator FallAndWait() {
		//while (true) {
		//	// refactor this out of here
		//	if (!Grid.Fall(PieceState.Current)) {
		//		Grid.FinishPiece();
		//		GetComponent<PieceFactory>().AddNext(Grid);
		//	}

		//	Grid.DestroyFullRows();

		//	yield return new WaitForSeconds(FallDelay);
		//}
		//yield return true;
	//}

	void ResetFallCoroutine() {
		//StopCoroutine(_fallRoutine);
		//StartCoroutine(_fallRoutine);
	}
}
