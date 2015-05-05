using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public float secondsToWait;
	public string nextLevel;

	IEnumerator Start() {
		Debug.Log("asd");
		yield return new WaitForSeconds(secondsToWait);
		AutoFade.LoadLevel(nextLevel, 1, 1, Preferences.BgColor());
	}
}
