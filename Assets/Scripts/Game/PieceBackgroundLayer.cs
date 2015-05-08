using UnityEngine;
using System.Collections;

public class PieceBackgroundLayer : MonoBehaviour {

	public GameObject Prefab;
	public Color Color;
	private GameObject _background;

	// Use this for initialization
	void Start () {
		_background = Instantiate(Prefab, Vector3.zero, Quaternion.identity) as GameObject;
		_background.transform.parent = gameObject.transform;
		_background.transform.Rotate(90, 0, 0);
		_background.transform.Translate(0, 0, 2);
	}
	
	// Update is called once per frame
	void Update () {
		Bounds bounds = GetPieceBounds();
		_background.transform.position = bounds.center;
		_background.transform.localScale = new Vector3(bounds.extents.x * 0.5f, 1, 100);
	}

	Bounds GetPieceBounds() {
		var result = new Bounds();

		foreach (Transform child in transform) {
			if (child.gameObject.tag == "pieceBackground") {
				continue;
			}
			var renderers = child.GetComponentsInChildren<Renderer>();

			result.Encapsulate(renderers[0].bounds);
			result.Encapsulate(renderers[1].bounds);
		}

		return result;
	}
}
