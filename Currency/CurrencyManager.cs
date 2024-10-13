using Game_Bonuses;
using Character;
using GameRuntime;
using UnityEngine;
using UnityEngine.Events;

public  class CurrencyManager : MonoBehaviour {
	public static CurrencyManager Instance;
	public static UnityAction OnBalanceChange;
    public static int ScoresToBuyBonuses { get ; private set ; }
	public static int ScoresForOneGame { get ; private set ; }

    private void Awake() => Instance = this;

    public void Subtract(int amount) {
		ScoresToBuyBonuses -= amount;
		SavesProvider.Instance.SaveScores();
        OnBalanceChange?.Invoke();
    }

	public void Add(int amount) {
		ScoresToBuyBonuses += amount;
		ScoresForOneGame += amount;

		LeaderboardProvider.Instance.AddScoresForAllTime(amount);
		if (ScoresForOneGame > LeaderboardProvider.Instance.MaxScores) {
			LeaderboardProvider.Instance.SetRecord(ScoresForOneGame);
		}

        SavesProvider.Instance.SaveScores();
        OnBalanceChange?.Invoke();
    }
	public void SetScores(int balance, int gameScores) {
		ScoresToBuyBonuses = balance;
		ScoresForOneGame = gameScores;
        OnBalanceChange?.Invoke();
    }

	public void SetScoresForBall(BallSize ballSize, Vector3 collisionPoint) {
		int scoresForBall = GetScoresForBall((int)ballSize);
		Add(scoresForBall);
		ScoresDisplayManager.Instance.SetFloatingText(collisionPoint, scoresForBall);
	}
	private int GetScoresForBall(int index) {
		if (index <= 5) {
			return index * 10;
		}
		else if ( index <= 8 ) {
			return index * 20;
		}
		else {
			return index * 50;
		}
	}
    private void OnEnable() {
        BallsController.OnBiggerOneSpawn += SetScoresForBall;
		Lithness.OnLigthnessDectroyBall += SetScoresForBall;
    }

    private void OnDisable() {
        BallsController.OnBiggerOneSpawn -= SetScoresForBall;
		Lithness.OnLigthnessDectroyBall -= SetScoresForBall;
    }
}


