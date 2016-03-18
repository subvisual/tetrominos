using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundCtrl : MonoBehaviour {
  public Sprite OnSprite;
  public Sprite OffSprite;

  public void Awake() {
    SetSprite();
  }

  private bool IsSoundEnabled() {
    if (!PlayerPrefs.HasKey("soundEnabled") || PlayerPrefs.GetInt("soundEnabled") > 0) {
      return true;
    }
    return false;
  }

  public void ToggleSound() {
    var image = transform.GetChild(1).GetComponent<Image>();

    if (IsSoundEnabled()) {
      PlayerPrefs.SetInt("soundEnabled", 0);
    } else {
      PlayerPrefs.SetInt("soundEnabled", 1);
    }
    SetSprite();
  }

  private void SetSprite() {
    var image = transform.GetChild(1).GetComponent<Image>();

    if (IsSoundEnabled()) {
      image.sprite = OnSprite;
    } else {
      image.sprite = OffSprite;
    }
  }

}
