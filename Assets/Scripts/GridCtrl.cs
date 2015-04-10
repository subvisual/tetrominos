using UnityEngine;

public class GridCtrl : GridBehaviour {

	public int Columns;
	public int Rows;
	public float Width { get; private set; }
	public float Height { get; private set; }
	public float PieceSize { get; private set; }
	public Rect Boundaries { get; private set; }

	void Awake() {
		Resize();
		PlaceUI();
	}

	public void FinishGame() {
		GetComponent<FallRoutine>().enabled = false;
		GetComponent<PieceFactory>().enabled = false;
		CurrentPiece().enabled = false;
		AutoFade.LoadLevel("mainMenu", 2, 2, Camera.main.backgroundColor);
	} 

	private void Resize() {
		Height = Camera.main.orthographicSize * 2f - 1f;
		PieceSize = Height / (float) Rows;
		Width = PieceSize * Columns;
		transform.Translate(Vector3.down * 0.5f);
		Boundaries = new Rect(- Width * 0.5f, - Height * 0.5f - 0.5f, Width, Height );
	}

	private void PlaceUI() {
		var ui = GameObject.Find("previewHolder");
		ui.transform.position = new Vector3(Width * -0.25f, Height * 0.5f, 0);
	}
}
