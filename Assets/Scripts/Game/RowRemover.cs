using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class RowRemover : GridBehaviour {

  public float ExitAnimationDuration = 0.3f;
  public float ExitAnimationOffset = 0.05f;

	private int _maxRows;
	private float _threshold;

	void Start() {
		_maxRows = GetComponent<GridCtrl>().Columns;
		_threshold = GetComponent<GridCtrl>().PieceSize * 0.5f;
	}

	public int Run() {
		var rows = FullCoords().Select(coord => coord.y).OrderBy(value => value);

		var destroyedRows = new List<float>();

		var count = 0;
		float previousRow = 0.0f;
		foreach (float row in rows) {
			if (count == 0 || Mathf.Abs(previousRow - row) < _threshold) {
				count++;
			} else {
				count = 1;
			}
			previousRow = row;

			if (count == _maxRows) {
				DestroyRow(row);
				destroyedRows.Add(row);
			}
		}

    var fullAnimDuration = ExitAnimationDuration + ExitAnimationOffset * _maxRows;
		StartCoroutine(DelayedCollapseParts(destroyedRows, fullAnimDuration));
		return destroyedRows.Count;
	}

	void DestroyRow(float y) {
    float animDelay = 0f;
		foreach (Transform child in PiecesHolder().transform) {
			foreach (Transform part in child) {
				if (Mathf.Abs(part.position.y - y) < _threshold) {
          StartCoroutine(DestroyPart(part.gameObject, animDelay, ExitAnimationDuration));
          animDelay += ExitAnimationOffset;
				}
			}
			if (child.childCount == 0) {
				Destroy(child.gameObject);
			}
		}
	}

  IEnumerator DelayedCollapseParts(List<float> rows, float delay) {
    yield return new WaitForSeconds(delay);
    CollapseParts(rows);
  }

	void CollapseParts(List<float> rows) {
		foreach (Transform child in PiecesHolder().transform) {
			foreach (Transform part in child) {
				// count how many rows were destroyed below this part
				var collapseMultiplier = rows.Count(row => row < part.position.y);
				part.Translate(Vector3.down * part.lossyScale.y * collapseMultiplier, Space.World);
			}
		}
	}

  IEnumerator DestroyPart(GameObject obj, float animDelay, float killDelay) {
    yield return new WaitForSeconds(animDelay);

    Animator animator = obj.GetComponent<Animator>();
    Destroyer destroyer = obj.GetComponent<Destroyer>();

    if (animator) {
      animator.enabled = true;
      animator.SetTrigger("Exit");
    }

    if (destroyer) {
      destroyer.DestroyAfter(killDelay);
    } else {
      Destroy(obj);
    }
  }
}
