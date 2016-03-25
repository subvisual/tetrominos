using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCtrl : MonoBehaviour {
  private int _currentScore;
  private Text _scoreView;
  private Text _scoreAnimText;
  private Animator _scoreAnimAnimator;
  public int[] scores;

  // Use this for initialization
  void Start() {
    _currentScore = 0;
    _scoreView = GameObject.Find("text_ScoreValue").GetComponent<Text>();
    _scoreAnimText = GameObject.Find("text_ScoreAnimation").GetComponent<Text>();
    _scoreAnimAnimator = GameObject.Find("text_ScoreAnimation").GetComponent<Animator>();
  }

  void OnGUI() {
    _scoreView.text = _currentScore.ToString();
    _scoreAnimText.text = _currentScore.ToString();
  }

  public void IncrementCurrentScore(int inc) {
    _currentScore += scores[inc - 1];
    UpdateHighscore();
    _scoreAnimAnimator.SetTrigger("Scored");
  }

  private void UpdateHighscore() {
    PlayerPrefs.SetInt("lastScore", _currentScore);
    if (_currentScore > this.CurrentHighScore()) {
      PlayerPrefs.SetInt("highscore", _currentScore );
    }
  }

  private int CurrentHighScore() {
    return PlayerPrefs.GetInt("highscore");
  }
}
