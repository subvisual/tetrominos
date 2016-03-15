using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreViewer : MonoBehaviour {

  public string key = "highscore";

	void OnGUI () {
    GetComponent<Text>().text = CurrentScore().ToString();
  }

  public int CurrentScore() {
    return PlayerPrefs.GetInt(key);
  }
}
