using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GridCtrl : GridBehaviour {

	public int Columns;
	public int Rows;
	public float Width { get; private set; }
	public float Height { get; private set; }
	public float PieceSize { get; private set; }
	public Rect SpawnBoundaries { get; private set; }
	public Rect GridBoundaries { get; private set; }

	void Awake() {
		Resize();
	}

  void Update() {
    if (GetComponent<InputCtrl>().IsPausing()) {
      Pause();
    }
  }

  void OnApplicationPause(bool pauseStatus) {
    if (pauseStatus) {
      Pause();
    }
  }

	public void FinishGame() {
    Time.timeScale = 1;
    GetComponent<FallRoutine>().StopAllCoroutines();
    GetComponent<InputCtrl>().paused = true;
    GetComponent<FallRoutine>().enabled = false;
    GetComponent<PieceFactory>().enabled = false;
    CurrentPiece().enabled = false;

    CameraFade.StartAlphaFade(Preferences.BgColor(), false, 2f, 0f, () => {
      SceneManager.LoadScene("mainMenu");
    });
  } 

	private void Resize() {
		Height = Camera.main.orthographicSize * 2f;
		PieceSize = Height / (float) Rows;
		Width = PieceSize * Columns;
		var extraHeight = Height * 0.5f;
		SpawnBoundaries = new Rect(- Width * 0.5f, - Height * 0.5f, Width, Height + extraHeight);
    GridBoundaries = new Rect(-Width * 0.5f, -Height * 0.5f, Width, Height);

    // Set background size
    var bg = GameObject.FindGameObjectWithTag("InnerBackground");
    bg.transform.localScale = new Vector3(PieceSize, 1, PieceSize / Columns * Rows);
  }

  public void Pause() {
    Time.timeScale = 0;
    GameObject.FindGameObjectWithTag("pauseMenu").GetComponent<Canvas>().enabled = true;
    GetComponent<InputCtrl>().paused = true;
  }

  public void Unpause() {
    GameObject.FindGameObjectWithTag("pauseMenu").GetComponent<Canvas>().enabled = false;
    Time.timeScale = 1;
    StartCoroutine(EnableInput());
  }

  public IEnumerator EnableInput() {
    yield return new WaitForSeconds(0.1f);
    GetComponent<InputCtrl>().paused = false;
  }
}
