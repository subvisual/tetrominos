using UnityEngine;
using System.Collections;

public class screenScaler : MonoBehaviour {

  void Start() {
    var y = Camera.main.orthographicSize * 2.0f;
    var x = y * Screen.width / Screen.height;
    transform.localScale = new Vector3(x, y, 1);
  }
}
