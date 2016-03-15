using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class FallRoutine : GridBehaviour {

  public float CurrentSpeed = 1;
  public float SpeedIncrement = 0.3f;
  public float AudioSpeedIncrement = 0.05f;
  public int SpeedUpDelay = 10;
  public int SpeedUpDelayIncrement = 10;
	public float RespawnDelay;
	public float FallDelay;
	public float FallTurbo;

	private bool _playing;
	private GridCtrl _gridCtrl;
  private IEnumerator _fallRoutine;
  private IEnumerator _speedUpRoutine;
	private InputCtrl _inputCtrl;
	private RowRemover _rowRemover;
	private bool _isInTurbo;

	private Text _scoreView;

	void Awake() {
		_playing = true;
		_isInTurbo = false;
		_gridCtrl = GetComponent<GridCtrl>();
		_inputCtrl = GetComponent<InputCtrl>();
    _rowRemover = GetComponent<RowRemover>();
		_fallRoutine = FallAndWait();
    _speedUpRoutine = SpeedUpOverTime();
	}

	void Start () {
		StartCoroutine(_fallRoutine);
    StartCoroutine(_speedUpRoutine);
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
					yield return new WaitForSeconds(this.CurrentRespawnDelay());
				}
      }

      yield return new WaitForSeconds(this.CurrentFallDelay());
		}
	}

  IEnumerator SpeedUpOverTime() {
    while (_playing) {
      yield return new WaitForSeconds(this.SpeedUpDelay);
      this.CurrentSpeed += this.SpeedIncrement;
      this.SpeedUpDelay += this.SpeedUpDelayIncrement;
      this.GetComponent<AudioSource>().pitch += this.AudioSpeedIncrement;
    }
    yield return new WaitForSeconds(this.SpeedUpDelay);
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
	  GetComponent<ScoreCtrl>().IncrementCurrentScore(inc * 2 - 1);
	}

  private float CurrentFallDelay() {
    return this.FallDelay / this.CurrentSpeed;
  }

  private float CurrentRespawnDelay() {
    return this.RespawnDelay / this.CurrentSpeed;
  }
}
