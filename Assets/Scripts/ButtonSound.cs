using UnityEngine;
using System.Collections;

public class ButtonSound : MonoBehaviour {
	public void ConditionalPlay() {
    if (!PlayerPrefs.HasKey("soundEnabled") || PlayerPrefs.GetInt("soundEnabled") > 0) {
      GetComponent<AudioSource>().Play();
    }
	}
}
