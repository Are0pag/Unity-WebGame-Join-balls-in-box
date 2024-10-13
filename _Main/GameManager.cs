using GameRuntime;
using TakeAim;
using UnityEngine;

public enum GameState {
	Pause,
	Playing,
    BonusFreese
}

public class GameManager : MonoBehaviour {
	public static GameManager Instance {get; private set; }
	public GameState State { get; private set; }

    private void Awake() => Instance = this;
    private void Start() {
        State = GameState.Playing;
        SetTakeAimManager();
    }    

    public void ChancheGameState(GameState gameState) {
		State = gameState;
        switch (State) {
            case GameState.Pause:
                Time.timeScale = 0f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                SetTakeAimManager();
                AudioManager.Instanse.ChacheBGMusic();
                break;
            case GameState.BonusFreese:
                AudioManager.Instanse.ChacheBGMusic();
                break;
        }
    }

    public void OnLoose() {
        Pool.Instance.OnRestart();
        CurrencyManager.Instance.SetScores(0, 0);
        TakeAimManager.Instance.EnterNewBall();
    }

    private static void SetTakeAimManager() {
        if (TakeAimManager.Instance.BallOnDrag == null && TakeAimManager.Instance.State != TakeAimManager.InputState.WaitingForFirstCollision) {
            TakeAimManager.Instance.EnterNewBall();
        }
    }
}