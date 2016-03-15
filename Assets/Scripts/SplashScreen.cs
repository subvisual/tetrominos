using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	public float secondsToWait;
	public string nextLevel;

	IEnumerator Start() {
        yield return new WaitForSeconds(secondsToWait);

        CameraFade.StartAlphaFade(Preferences.BgColor(), false, 2f, 0f, () => {
            SceneManager.LoadScene(nextLevel);
        });
    }
}
