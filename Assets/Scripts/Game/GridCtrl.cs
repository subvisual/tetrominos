using UnityEngine;
using System.Collections;

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

	public void FinishGame() {
    Time.timeScale = 1;
		GetComponent<FallRoutine>().enabled = false;
		GetComponent<PieceFactory>().enabled = false;
		CurrentPiece().enabled = false;

    CameraFade.StartAlphaFade(Preferences.BgColor(), false, 2f, 0f, () => {
        Application.LoadLevel("mainMenu");
    });
	} 

	private void Resize() {
		Height = Camera.main.orthographicSize * 2f - 1f;
		PieceSize = Height / (float) Rows;
		Width = PieceSize * Columns;
    transform.Translate(Vector3.down * 0.5f);
		var extraHeight = Height * 0.5f;
		SpawnBoundaries = new Rect(- Width * 0.5f, - Height * 0.5f - 0.5f, Width, Height + extraHeight);
    GridBoundaries = new Rect(-Width * 0.5f, -Height * 0.5f - 0.5f, Width, Height);

    // Set background size
    var bg = GameObject.FindGameObjectWithTag("InnerBackground");
    var screenWidth = (float) Camera.main.orthographicSize * 2.0 * Screen.width / Screen.height;
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
