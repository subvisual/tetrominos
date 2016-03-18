using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	public bool done;
	public float secondsToWait;
	public string nextLevel;

	void Awake() {
		done = false;
	}

	void Update() {
				if(Application.isShowingSplashScreen || done) return;
				done = true;

				StartCoroutine(Splash());
  }

	IEnumerator Splash() {
		yield return new WaitForSeconds(secondsToWait);

		CameraFade.StartAlphaFade(Preferences.BgColor(), false, 2f, 0f, () => {
				SceneManager.LoadScene(nextLevel);
		});
	}
}
