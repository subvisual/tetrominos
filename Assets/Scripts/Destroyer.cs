using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

  public void DestroyAfter(float seconds) {
    StartCoroutine(DestroyRoutine(seconds));
  }

  IEnumerator DestroyRoutine(float seconds) {
    yield return new WaitForSeconds(seconds);
    Destroy(gameObject);
  }
}
