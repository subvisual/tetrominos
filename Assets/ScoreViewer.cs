using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreViewer : MonoBehaviour {

	void OnGUI () {
    GetComponent<Text>().text = CurrentHighScore().ToString();
  }

  public int CurrentHighScore() {
    return PlayerPrefs.GetInt("highscore");
  }
}
