using UnityEngine;
using System.Collections;

public class FallRoutine : GridBehaviour {

	public float FallDelay;

	private GridCtrl _gridCtrl;
	private IEnumerator _fallRoutine;

	void Awake() {
		_gridCtrl = GetComponent<GridCtrl>();
		_fallRoutine = FallAndWait();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(_fallRoutine);
	}

	IEnumerator FallAndWait() {
		while (true) {
			PieceCtrl current = CurrentPiece();
			if (current) {
				if (current.CanFall(_gridCtrl.Boundaries, IsCoordFree)) {
					current.Fall();
				} else {
					current.MakeFull();
					GetComponent<PieceFactory>().AddNext();
				}
			}

			yield return new WaitForSeconds(FallDelay);
		}
	}
}
