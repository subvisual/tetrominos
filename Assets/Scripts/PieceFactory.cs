using UnityEngine;
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
		var position = new Vector3(
			- _gridCtrl.PieceWidth,
			(_gridCtrl.Height - _gridCtrl.PieceHeight) * 0.5f,
			0);
		var scale = new Vector3(_gridCtrl.PieceWidth, _gridCtrl.PieceHeight, 1);

		GameObject piece = Instantiate(Templates[_nextIndex], position, Quaternion.identity) as GameObject;
		piece.transform.localScale = scale;
		piece.transform.parent = _piecesHolder.transform;

		RollNext();
	}

	void RollNext() {
		_nextIndex = Random.Range(0, Templates.GetLength(0));
		_nextRotation = Random.Range (0, 4);
		_nextTranspose = Random.Range(0, 1) == 1;
	}
}
