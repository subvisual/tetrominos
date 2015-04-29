using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextColorSelector : MonoBehaviour {

	// Use this for initialization
	void Start() {
		gameObject.GetComponent<Text>().color = Preferences.TextColor();
	}
}
