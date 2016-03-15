using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCtrl : MonoBehaviour {
	public void BtnNewGame() {
    CameraFade.StartAlphaFade(Preferences.BgColor(), false, 0.5f, 0f, () => {
      SceneManager.LoadScene("game");
    });
  }

  public void BtnExit() {
    Debug.Log("Exiting");
		Application.Quit();
	}
}
