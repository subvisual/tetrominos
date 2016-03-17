using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundCtrl : MonoBehaviour {

  void OnGUI() {
    transform.GetChild(0).GetComponent<Text>().text = ButtonText();
  }

  private string ButtonText() {
    if (IsSoundEnabled()) {
      return "TURN MUSIC OFF";
    } else {
      return "TURN MUSIC ON";
    }
  }

  private bool IsSoundEnabled() {
    if (!PlayerPrefs.HasKey("soundEnabled") || PlayerPrefs.GetInt("soundEnabled") > 0) {
      return true;
    }
    return false;
  }

  public void ToggleSound() {
    if (IsSoundEnabled()) {
      PlayerPrefs.SetInt("soundEnabled", 0);
    } else {
      PlayerPrefs.SetInt("soundEnabled", 1);
    }
  }
}
