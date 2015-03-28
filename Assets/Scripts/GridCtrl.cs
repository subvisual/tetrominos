using UnityEngine;

public class GridCtrl : GridBehaviour {

	public int Columns;
	public int Rows;
	public float Width { get; private set; }
	public float Height { get; private set; }
	public float PieceSize { get; private set; }
	public Rect Boundaries { get; private set; }

	void Awake() {
		Height = Camera.main.orthographicSize * 2f;
		PieceSize = Height / (float) Rows;
		Width = PieceSize * Columns;
		Boundaries = new Rect(- Width * 0.5f, - Height * 0.5f, Width, Height );
	}

	void Start() {
		//Grid = new Grid(PiecePrefab, SeparatorPrefab, transform, Columns, Rows, width, height);
		//_fallRoutine = FallAndWait();
		//GetComponent<PieceFactory>().AddNext(Grid);
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
}
