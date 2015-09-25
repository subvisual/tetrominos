using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuCtrl : MonoBehaviour {
	public Color fadeColor;
	public Slider themeSlider;

	void Awake() {
		if (Preferences.theme == Preferences.Theme.Dark) {
			SetSlider(1);
		} else {
			SetSlider(0);
		}
	}

	public void BtnNewGame() {
        CameraFade.StartAlphaFade(Preferences.BgColor(), false, 2f, 0f, () => {
            Application.LoadLevel("game");
        });
    }

	public void BtnExit() {
		Application.Quit();
	}

	public void BtnLightTheme() {
		SetSlider(0);
	}

	public void BtnDarkTheme() {
		SetSlider(1);
	}
	
	void SetSlider(float value) {
		themeSlider.value = value;
	}

	public void SliderToggleTheme() {
		if (themeSlider.value < 0.5f) {
			SetTheme(Preferences.Theme.Light);
		} else {
			SetTheme(Preferences.Theme.Dark);
		}
	}

	void SetTheme(Preferences.Theme newTheme) {
		Preferences.theme = newTheme;
	}

}
