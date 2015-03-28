using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RowRemover : GridBehaviour {

	private int _maxRows;
	private float _threshold;

	void Start() {
		_maxRows = GetComponent<GridCtrl>().Columns;
		_threshold = GetComponent<GridCtrl>().PieceSize * 0.5f;
	}

	public void Run() {
		var rows = FullCoords().Select(coord => coord.y).OrderBy(value => value);

		var destroyedRows = new List<float>();

		var count = 0;
		float previousRow = 0.0f;
		foreach (float row in rows) {
			Debug.Log(Mathf.Abs(previousRow - row) + " " + _threshold);
			if (count == 0 || Mathf.Abs(previousRow - row) < _threshold) {
				count++;
				Debug.Log("here");
			} else {
				count = 1;
			}
			previousRow = row;

			if (count == _maxRows) {
				DestroyRow(row);
				destroyedRows.Add(row);
			}
		}

		CollapseParts(destroyedRows);
	}

	void DestroyRow(float y) {
		foreach (Transform child in PiecesHolder().transform) {
			foreach (Transform part in child) {
				if (Mathf.Abs(part.position.y - y) < _threshold) {
					Destroy(part.gameObject);
				}
			}
			if (child.childCount == 0) {
				Destroy(child.gameObject);
			}
		}
	}

	void CollapseParts(List<float> rows) {
		foreach (Transform child in PiecesHolder().transform) {
			foreach (Transform part in child) {
				// count how many rows were destroyed below this part
				var collapseMultiplier = rows.Count(row => row < part.position.y);
				Debug.Log(collapseMultiplier);
				part.Translate(Vector3.down * part.lossyScale.y * collapseMultiplier, Space.World);
			}
		}
	}
}
