using UnityEngine;
using System.Collections;

public class FallRoutine : MonoBehaviour {

	public float FallDelay;

	private GameObject _piecesHolder;
	private GridCtrl _gridCtrl;
	private IEnumerator _fallRoutine;

	void Awake() {
		_gridCtrl = GetComponent<GridCtrl>();
		_piecesHolder = GameObject.Find("piecesHolder");
		_fallRoutine = FallAndWait();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(_fallRoutine);
	}

	PieceCtrl CurrentPiece() {
		GameObject result;
		foreach (Transform child in _piecesHolder.transform) {
			var pieceCtrl = child.GetComponent<PieceCtrl>();
			if (pieceCtrl.IsCurrent()) {
				return pieceCtrl;
			}
		}
		return null;
	}

	IEnumerator FallAndWait() {
		while (true) {
			PieceCtrl current = CurrentPiece();
			if (current) {
				if (current.CanFall(_gridCtrl.Boundaries)) {
					current.Fall();
				} else {
					current.MakeFull();
				}
			}

			yield return new WaitForSeconds(FallDelay);
		}
	}
}
