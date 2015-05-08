using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class FallRoutine : GridBehaviour {

	public float RespawnDelay;
	public float FallDelay;
	public float FallTurbo;

	private bool _playing;
	private GridCtrl _gridCtrl;
	private IEnumerator _fallRoutine;
	private InputCtrl _inputCtrl;
	private RowRemover _rowRemover;
	private bool _isInTurbo;

	private Text _scoreView;
	private int _score;

	void Awake() {
		_playing = true;
		_isInTurbo = false;
		_gridCtrl = GetComponent<GridCtrl>();
		_inputCtrl = GetComponent<InputCtrl>();
		_rowRemover = GetComponent<RowRemover>();
		_fallRoutine = FallAndWait();
		_scoreView = GameObject.Find("text_ScoreValue").GetComponent<Text>();
	}

	void Start () {
		_score = 0;
		IncreaseScore(0);
		StartCoroutine(_fallRoutine);
	}

	void Update() {
		if (_inputCtrl.IsInTurboMode() && !_isInTurbo) {
			_isInTurbo = true;
			FallDelay /= FallTurbo;
			ResetCoroutine();
		} else if (!_inputCtrl.IsInTurboMode() && _isInTurbo) {
			_isInTurbo = false;
			FallDelay *= FallTurbo;
			ResetCoroutine();
		}
	}

	IEnumerator FallAndWait() {
		while (_playing) {
			PieceCtrl current = CurrentPiece();
			if (current) {
				if (current.CanFall(_gridCtrl.SpawnBoundaries, IsCoordFree)) {
					current.Fall();
				} else {
					current.MakeFull();
					var destroyedRows = _rowRemover.Run();
					IncreaseScore(destroyedRows);
					GetComponent<PieceFactory>().AddNext();
					if (IsGameLost()) {
						StopAllCoroutines();
						GetComponent<GridCtrl>().FinishGame();
					}
					yield return new WaitForSeconds(RespawnDelay);
				}
			}

			yield return new WaitForSeconds(FallDelay);
		}
	}

	bool IsGameLost() {
		var fullCoords = FullCoords();
		var currentCoords = CurrentPiece().PartPositions();

		foreach (var currentCoord in currentCoords) {
			if (fullCoords.Any((fullCoord) => {
				return Vector3.Distance(fullCoord, currentCoord) < CollisionThreshold();
			})) {
				return true;
			}
		}
		return false;
	}

	void ResetCoroutine() {
		StopCoroutine(_fallRoutine);
		StartCoroutine(_fallRoutine);
	}

	void IncreaseScore(int inc) {
		_score += Factorial(inc);
		_scoreView.text = _score.ToString();
	}

	private int Factorial(int num) {
		var result = num;
		for (var i = 2; i < num; i++) {
			result *= i;
		}
		return result;
	}
}
