using UnityEngine;
using System.Collections;

public class FallRoutine : GridBehaviour {

	public float FallDelay;
	public float FallTurbo;

	private GridCtrl _gridCtrl;
	private IEnumerator _fallRoutine;
	private InputCtrl _inputCtrl;
	private RowRemover _rowRemover;
	private bool _isInTurbo;

	void Awake() {
		_isInTurbo = false;
		_gridCtrl = GetComponent<GridCtrl>();
		_inputCtrl = GetComponent<InputCtrl>();
		_rowRemover = GetComponent<RowRemover>();
		_fallRoutine = FallAndWait();
	}

	void Start () {
		StartCoroutine(_fallRoutine);
	}

	void Update() {
		if (_inputCtrl.IsInTurboMode() && !_isInTurbo) {
			_isInTurbo = true;
			FallDelay /= FallTurbo;
			ResetCoroutine();
		} else if (!_inputCtrl.IsInTurboMode() && _isInTurbo) {
			_isInTurbo = false;
			FallDelay *= FallTurbo;
			ResetCoroutine();
		}
	}

	IEnumerator FallAndWait() {
		while (true) {
			PieceCtrl current = CurrentPiece();
			if (current) {
				if (current.CanFall(_gridCtrl.Boundaries, IsCoordFree)) {
					current.Fall();
				} else {
					current.MakeFull();
					_rowRemover.Run();
					//CollapseFullPieces();
					GetComponent<PieceFactory>().AddNext();
				}
			}

			yield return new WaitForSeconds(FallDelay);
		}
	}

	//void CollapseFullPieces() {
	//	foreach (Transform child in PiecesHolder().transform) {
	//		var ctrl = child.GetComponent<PieceCtrl>();
	//		if (ctrl.IsFull()) {
	//			ctrl.Fall();
	//		}
	//	}
	//}

	void ResetCoroutine() {
		StopCoroutine(_fallRoutine);
		StartCoroutine(_fallRoutine);
	}
}
