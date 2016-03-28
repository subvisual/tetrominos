using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class PieceFactory : MonoBehaviour {
	public GameObject[] Templates;

	private GridCtrl _gridCtrl;
	private GameObject _piecesHolder;
	private GameObject _previewHolder;
	private int _nextIndex;
	private int _nextRotation;
  private bool _nextTranspose;
  private System.Random _pieceRng;
  private System.Random _rotationRng;
  public List<int> _repeated;

	// Use this for initialization
	void Awake() {
    _repeated = new List<int>();
		_piecesHolder = GameObject.Find("piecesHolder");
		_previewHolder = GameObject.Find("previewHolder");
		_gridCtrl = GetComponent<GridCtrl>();
    _pieceRng = new System.Random(Guid.NewGuid().GetHashCode());
    _rotationRng = new System.Random(Guid.NewGuid().GetHashCode());
		RollNext();
	}

	void Start() {
		AddNext();
	}

	public void AddNext() {
		GameObject piece = Instantiate(Templates[_nextIndex], Vector3.zero, Quaternion.identity) as GameObject;
		var ctrl = piece.GetComponent<PieceCtrl>();

		var x = -_gridCtrl.PieceSize * (ctrl.Columns()/ 2 - 1);
		if (ctrl.Columns() % 2 == 1) {
			x -= _gridCtrl.PieceSize;
		}
		var y = _gridCtrl.Height * 0.5f - 0.5f;

    piece.transform.parent = _piecesHolder.transform;
		piece.transform.localPosition = new Vector3(x, y, 0);
		piece.transform.localScale = new Vector3(_gridCtrl.PieceSize, _gridCtrl.PieceSize, 1);
    ctrl.MakeCurrent();

    if (_nextTranspose) {
      var newScale = piece.transform.localScale;
      newScale.x *= -1;
      piece.transform.localScale = newScale;
    }

		RollNext();

		_previewHolder.GetComponent<PreviewCtrl>().ChangePiece(Templates[_nextIndex], _nextTranspose, _nextRotation);
	}

	void RollNext() {
		_nextIndex = _pieceRng.Next(0, Templates.GetLength(0));
    _repeated.Add(_nextIndex);
		_nextRotation = _rotationRng.Next(0, 4);
    _nextTranspose = _rotationRng.NextDouble() >= 0.5f;

    _repeated.Add(_nextIndex);
    if (_repeated.Distinct().ToList().Count > 1) {

      _repeated.Clear();
    } else if (_repeated.Count >= 3) {

      RollNext();
    }
	}
}
