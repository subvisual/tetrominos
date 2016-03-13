using UnityEngine;
using System.Collections;

public class SceneFadeIn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CameraFade.StartAlphaFade(Preferences.BgColor(), true, 0.5f);
    }
}
