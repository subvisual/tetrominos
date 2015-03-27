using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PieceFactory : MonoBehaviour {
	public GameObject[] Templates;

	private GridCtrl _gridCtrl;
	private GameObject _piecesHolder;
	private int _nextIndex;
	private int _nextRotation;
	private bool _nextTranspose;

	// Use this for initialization
	void Awake () {
		_piecesHolder = GameObject.Find("piecesHolder");
		_gridCtrl = GetComponent<GridCtrl>();
		RollNext();
	}

	void Start() {
		AddNext();
	}

	public void AddNext() {
		GameObject piece = Instantiate(Templates[_nextIndex], Vector3.zero, Quaternion.identity) as GameObject;
		var ctrl = piece.GetComponent<PieceCtrl>();

		var x = -_gridCtrl.PieceWidth * ctrl.Columns() / 2;
		if (ctrl.Columns() % 2 == 1) {
			x -= _gridCtrl.PieceWidth * 0.5f;
		}

		var y = _gridCtrl.Height * 0.5f - ctrl.Rows() * _gridCtrl.PieceHeight;

		piece.transform.Translate(new Vector3(x, y, 0));
		piece.transform.localScale = new Vector3(_gridCtrl.PieceWidth, _gridCtrl.PieceHeight, 1);
		piece.transform.parent = _piecesHolder.transform;

		if (_nextTranspose) {
			Debug.Log("transposing");
			var newScale = piece.transform.localScale;
			newScale.x *= -1;
			piece.transform.localScale = newScale;
		}

		RollNext();
	}

	void RollNext() {
		_nextIndex = Random.Range(0, Templates.GetLength(0));
		_nextRotation = Random.Range (0, 4);
		//_nextTranspose = Random.value >= 0.5f;
		var val = Random.value;
		_nextTranspose = val >= 0.5f;
		Debug.Log(val);
		Debug.Log(_nextTranspose);
	}
}
