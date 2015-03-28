using UnityEngine;

public class GridCtrl : GridBehaviour {

	public int Columns;
	public int Rows;
	public float Width { get; private set; }
	public float Height { get; private set; }
	public float PieceSize { get; private set; }
	public Rect Boundaries { get; private set; }

	void Awake() {
		Height = Camera.main.orthographicSize * 2f;
		PieceSize = Height / (float) Rows;
		Width = PieceSize * Columns;
		Boundaries = new Rect(- Width * 0.5f, - Height * 0.5f, Width, Height );
	}
}
