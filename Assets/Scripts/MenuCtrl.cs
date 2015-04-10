using UnityEngine;
using System.Collections;

public class MenuCtrl : MonoBehaviour {

	public Color fadeColor;

	public void BtnNewGame() {
		AutoFade.LoadLevel("game", 1, 1, fadeColor);
	}

	public void BtnExit() {
		Application.Quit();
	}
}
