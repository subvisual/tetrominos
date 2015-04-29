using UnityEngine;
using System.Collections;

public class Preferences {
	public enum Theme { Dark, Light };

	public static Theme theme = Theme.Dark;

	public static Color LightBgColor = new Color32(225, 234, 239, 255);
	public static Color LightCurrentColor = new Color32(166, 194, 209, 255);

	public static Color DarkBgColor = new Color32(54, 66, 73, 255);
	public static Color DarkCurrentColor = new Color32(22, 36, 43, 255);

	public static Color LightTextColor = DarkBgColor;
	public static Color DarkTextColor = LightBgColor;
	public static Color LightScoreColor = DarkCurrentColor;
	public static Color DarkScoreColor = LightCurrentColor;

	public static Color CurrentColor() {
		if (theme == Theme.Dark) {
			return DarkCurrentColor;
		} else {
			return LightCurrentColor;
		}
	}

	public static Color BgColor() {
		if (theme == Theme.Dark) {
			return DarkBgColor;
		} else {
			return LightBgColor;
		}
	}

	public static Color TextColor() {
		if (theme == Theme.Dark) {
			return DarkTextColor;
		} else {
			return LightTextColor;
		}
	}

	public static Color ScoreColor() {
		if (theme == Theme.Dark) {
			return DarkScoreColor;
		} else {
			return LightScoreColor;
		}
	}
}
