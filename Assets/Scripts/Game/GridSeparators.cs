using UnityEngine;
using System.Collections;

public class GridSeparators : GridBehaviour {

	public GameObject SeparatorPrefab;

	private GridCtrl _gridCtrl;

	// Use this for initialization
	void Start () {
		_gridCtrl = GetComponent<GridCtrl>();
		SetupLines(transform.Find("Separators"));
	}

	void SetupLines(Transform parent) {
		for (var y = 0; y <= _gridCtrl.Rows; ++y) {
			var coords = new Vector3(0, y * _gridCtrl.PieceSize - (_gridCtrl.Height * 0.5f), 0);
			var separator = GameObject.Instantiate(SeparatorPrefab, coords, Quaternion.identity) as GameObject;
			separator.transform.localScale = new Vector3(1, _gridCtrl.Width, 1);
			separator.transform.Rotate(0, 0, 90);
			separator.transform.SetParent(parent, false);
		}

		for (var x = 0; x <= _gridCtrl.Columns; ++x) {
			var coords = new Vector3(x * _gridCtrl.PieceSize - (_gridCtrl.Width * 0.5f), 0, 0);
			var separator = GameObject.Instantiate(SeparatorPrefab, coords, Quaternion.identity) as GameObject;
			separator.transform.localScale = new Vector3(1, _gridCtrl.Height, 1);
			separator.transform.SetParent(parent, false);
		}
	}
}
