using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCtrl : MonoBehaviour {
  private int _currentScore;
  private Text _scoreView;

  // Use this for initialization
  void Start() {
    _currentScore = 0;
    _scoreView = GameObject.Find("text_ScoreValue").GetComponent<Text>();
  }

  void OnGUI() {
    _scoreView.text = CurrentHighScore().ToString();
  }

  public void IncrementCurrentScore(int inc) {
    _currentScore += inc;
    UpdateHighscore();
  }

  private void UpdateHighscore() {
    if (_currentScore > this.CurrentHighScore()) {
      PlayerPrefs.SetInt("highscore", _currentScore);
    }
  }

  private int CurrentHighScore() {
    return PlayerPrefs.GetInt("highscore");
  }
}
