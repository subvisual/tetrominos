using UnityEngine;
using System.Collections;

public class AspectRatioEnforcer : MonoBehaviour {

	public Vector2 aspectRatio;

	// Use this for initialization
	void Start () {
		// set the desired aspect ratio
		float targetAspect = aspectRatio.x / aspectRatio.y;

		// determine the game window's current aspect ratio
		float windowAspect = (float) Screen.width / (float) Screen.height;

		// current viewport height should be scaled by this amount
		float scaleHeight = windowAspect / targetAspect;

		// obtain camera component so we can modify its viewport
		Camera camera = GetComponent<Camera>();

		Rect rect = camera.rect;
		// if scaled height is less than current height, add letterbox
		if (scaleHeight < 1.0f) {
			rect.width = 1.0f;
			rect.height = scaleHeight;
			rect.x = 0;
			rect.y = (1.0f - scaleHeight) / 2.0f;
		} else { // add pillarbox
			float scaleWidth = 1.0f / scaleHeight;

			rect.width = scaleWidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scaleWidth) / 2.0f;
			rect.y = 0;
		}
		camera.rect = rect;
	}
}
