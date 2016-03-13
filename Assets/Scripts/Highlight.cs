using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Highlight : GridBehaviour {
  public GameObject HighlightTemplate;
  public GameObject HighlightsHolder;

  private GridCtrl _grid;
  private Dictionary<float, List<GameObject>> _pieces;

  void Awake() {
    _grid = GetComponent<GridCtrl>();
    _pieces = new Dictionary<float, List<GameObject>>();
  }
	
	// Update is called once per frame
	void Update () {
    HideAll();
    CurrentColumns().ForEach((column) => HighlightColumn(column));
	}

  void Start() {
    int x, y;
    for(x = 0; x < _grid.Columns; ++x) {
      for(y = 0; y < _grid.Rows; ++y) {
        InstantiateHighlightAt(x, y);
      }
    }
  }

  private void InstantiateHighlightAt(int col, int row) {
    GameObject piece = Instantiate(HighlightTemplate, Vector3.zero, Quaternion.identity) as GameObject;

    var x = _grid.PieceSize * col + (_grid.PieceSize - _grid.Width ) * 0.5f;
    var y = _grid.PieceSize * row + (- _grid.PieceSize - _grid.Height ) * 0.5f + _grid.PieceSize;

    piece.transform.parent = HighlightsHolder.transform;
    piece.transform.localPosition = new Vector3(x, y, 0f);
    piece.transform.localScale = new Vector3(0, 0, 1);

    if (!_pieces.ContainsKey(x)) {
      _pieces.Add(x, new List<GameObject>());
    }

    _pieces[x].Add(piece);
  }

  private List<float> CurrentColumns() {
    PieceCtrl _piece = CurrentPiece();
    List<Vector3> coords;

    if (_piece) {
      coords = _piece.PartPositions();
    } else {
      coords = new List<Vector3>();
    }

    return coords.Select((coord) => coord.x).ToList();
  }

  private void HideAll() {
    foreach(KeyValuePair<float, List<GameObject>> column in _pieces) {
      foreach(var piece in column.Value) {
        piece.transform.localScale = Vector3.zero;
      }
    }
  }

  private void HighlightColumn(float x) {
    var visibleScale = new Vector3(_grid.PieceSize, _grid.PieceSize, 1);

    var column = _pieces.First((elem) => {
      // fuzzy match between elem.Key and x. margin of error equal to half piece size
      return Mathf.Abs(elem.Key - x) < _grid.PieceSize * 0.5;
    }).Value;

    foreach(var piece in column) {
      piece.transform.localScale = visibleScale;
    }
  }
}
