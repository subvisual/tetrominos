using UnityEngine;
using System.Collections;

public class ShowWhenGridReached : MonoBehaviour {

	private GridCtrl _grid;
	private GameObject _piecesHolder;

	// Use this for initialization
	void Start () {
		_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridCtrl>();
		_piecesHolder = GameObject.FindGameObjectWithTag("piecesHolder");
		
		if (transform.parent.parent == _piecesHolder.transform) {
			Hide();
		} else {
			Show();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_grid.GridBoundaries.Contains(transform.position)) {
			Show();
		}
	}

	void Hide() {
		foreach (Transform child in transform) {
			child.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	void Show() {
		foreach (Transform child in transform) {
			MeshRenderer renderer = child.GetComponent<MeshRenderer>();
			if (renderer) {
				renderer.enabled = true;
			}
		}
		this.enabled = false;
	}
}
