using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceList {

	List<GameObject> list;

	public PieceList() {
		list = new List<GameObject>();
	}

	public void Add(GameObject obj) {
		list.Add (obj);
	}

	public GameObject findByCoords(Vector2 coords) {
		foreach (GameObject obj in list) {
			if (obj.GetComponent<PieceCtrl>().coords == coords) {
				return obj;
			}
		}
		return null;
	}

	public void Fall() {
		foreach (GameObject piece in list) {
			piece.GetComponent<PieceCtrl> ().Fall (this);
		}
	}
}
