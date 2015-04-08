using UnityEngine;
using System.Collections;

public class MenuCtrl : MonoBehaviour {

	public void BtnNewGame() {
		Application.LoadLevel("game");
	}

	public void BtnExit() {
		Application.Quit();
	}
}
