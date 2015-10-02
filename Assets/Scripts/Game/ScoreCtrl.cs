using UnityEngine;
using System.Collections;

public class ScoreCtrl : MonoBehaviour {

  private int _score;

  // Use this for initialization
  void Start() {
    _currentScore = 0;
  }

  void OnGUI() {
    GameObject.FindGameObjectWithTag("scoreText");
  }

  public void UpdateHighscore() {
    if (_currentScore > this.CurrentHighScore()) {
      PlayerPrefs.SetInt("highscore", _currentScore);
    }
  }

  public int CurrentHighScore() {
    return PlayerPrefs.GetInt("highscore");
  }

}
