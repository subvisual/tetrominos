using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundCtrl : MonoBehaviour {
  public Sprite OnSprite;
  public Sprite OffSprite;

  public void Awake() {
    SetMusic();
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
    SetMusic();
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

  private void SetMusic() {
    var gridCtrl = GameObject.FindGameObjectWithTag("Grid");

    if (!gridCtrl) {
      return;
    }

    var audio = gridCtrl.GetComponent<AudioSource>();

    if (IsSoundEnabled()) {
      audio.Play();
    } else {
      audio.Stop();
    }
  }
}
