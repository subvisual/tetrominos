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
		_piece.transform.localPosition = Vector3.zero;


		if (transpose) {
			var newScale = _piece.transform.localScale;
			newScale.x *= -1;
			_piece.transform.localScale = newScale;
		}
		if (_piece.GetComponent<PieceCtrl>().Rows() % 2 == 0) {
			Debug.Log(PieceHeight());
			Debug.Log(_piece.transform.position);
			//_piece.transform.Translate(Vector3.up * PieceHeight() * 0.5f);
			_piece.transform.localPosition = new Vector3(0f, PieceHeight() * 0.5f, 0f);
			Debug.Log(_piece.transform.position);
		}
	}

	float PieceHeight() {
		return _piece.GetComponent<PieceCtrl>().Height();
	}
}
