using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuCtrl : MonoBehaviour {
	public void BtnNewGame() {
    CameraFade.StartAlphaFade(Preferences.BgColor(), false, 0.5f, 0f, () => {
      Application.LoadLevel("game");
    });
  }

  public void BtnExit() {
    Debug.Log("Exiting");
		Application.Quit();
	}
}
