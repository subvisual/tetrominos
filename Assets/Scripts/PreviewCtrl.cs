using UnityEngine;
using System.Collections;

public class PreviewCtrl : MonoBehaviour {

	private GameObject _piece;

	public void ChangePiece(GameObject prefab, bool transpose, int rotations) {
		if (_piece) {
			Destroy(_piece);
		}
		_piece = Instantiate(prefab) as GameObject;
		_piece.transform.parent = transform;
		_piece.transform.localScale *= 0.2f;
		_piece.transform.localPosition = Vector3.down * PieceHeight() * 0.5f + Vector3.left;


		if (transpose) {
			var newScale = _piece.transform.localScale;
			newScale.x *= -1;
			_piece.transform.localScale = newScale;
		}
		if (_piece.GetComponent<PieceCtrl>().Rows() % 2 == 0) {
			_piece.transform.Translate(Vector3.up * PieceHeight() * 0.5f);
		}
	}

	float PieceHeight() {
		return _piece.GetComponent<PieceCtrl>().Height();
	}
}
