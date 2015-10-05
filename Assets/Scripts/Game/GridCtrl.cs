using UnityEngine;

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

	public void FinishGame() {
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
		GridBoundaries =  new Rect(- Width * 0.5f, - Height * 0.5f - 0.5f, Width, Height);
	}
}
