using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreViewer : MonoBehaviour {

	void OnGUI () {
    var txtObject = GameObject.FindGameObjectWithTag("scoreText");
    txtObject.GetComponent<Text>().text = "HIGHSCORE: " + CurrentHighScore();
  }

  public int CurrentHighScore() {
    return PlayerPrefs.GetInt("highscore");
  }
}
