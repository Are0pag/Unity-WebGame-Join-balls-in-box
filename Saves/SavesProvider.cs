using Character;
using GameRuntime;
using System;
using UnityEngine;
using YG;

public class SavesProvider : MonoBehaviour {
    public static SavesProvider Instance { get; private set; }
    [SerializeField] private float _ballsSavingRate;
    [SerializeField] private Transform _overflow;

    private void Awake() => Instance = this;
    private void Start() => InvokeRepeating(nameof(SaveBalls), _ballsSavingRate, _ballsSavingRate);
    private void OnEnable() => YandexGame.GetDataEvent += GetLoad;
    private void OnDisable() => YandexGame.GetDataEvent -= GetLoad;


    public void GetLoad() {
        LoadScores();
        LoadBalls();
        LoadSettings();
    }

    public void SaveSettings(float musicVolume, float soundsVolume) {
        YandexGame.savesData.MusicVolume = musicVolume;
        YandexGame.savesData.SoundsVolume = soundsVolume;
        YandexGame.SaveProgress();
    }
    public void LoadSettings() {
        if (YandexGame.savesData.BallsSizesNames != null) {
            AudioSettings.Instance.LoadSettings(YandexGame.savesData.MusicVolume, YandexGame.savesData.SoundsVolume); 
        }
        else {
            AudioSettings.Instance.LoadSettings();
        }
    }

    public void SaveRecord(int record) {
        YandexGame.savesData.MaxScores = record;
        YandexGame.SaveProgress();
    }

    public void SaveScores() {
        YandexGame.savesData.ScoresToBuyBonuses = CurrencyManager.ScoresToBuyBonuses;
        YandexGame.savesData.ScoresForOneGame = CurrencyManager.ScoresForOneGame;
        YandexGame.savesData.ScoresForAllTime = LeaderboardProvider.Instance.ScoresForAllTime;
        YandexGame.SaveProgress();
    }

    private void LoadScores() {
        CurrencyManager.Instance.SetScores(YandexGame.savesData.ScoresToBuyBonuses, YandexGame.savesData.ScoresForOneGame);
        LeaderboardProvider.Instance.SetScores(YandexGame.savesData.MaxScores, YandexGame.savesData.ScoresForAllTime);
    }

    private void SaveBalls() {
        if (YandexGame.savesData.BallsSizesNames != null) {
            ResetBalls();
            var activeBalls = Pool.Instance.GetActiveBalls();

            YandexGame.savesData.BallsSizesNames = new string[activeBalls.Count];
            YandexGame.savesData.BallsPosX = new float[activeBalls.Count];
            YandexGame.savesData.BallsPosY = new float[activeBalls.Count];

            for (int i = 0; i < activeBalls.Count; i++) {
                if (activeBalls[i].Ball.transform.position.y < _overflow.transform.position.y) {
                    YandexGame.savesData.BallsSizesNames[i] = activeBalls[i].Size.ToString();
                    YandexGame.savesData.BallsPosX[i] = activeBalls[i].Ball.transform.position.x;
                    YandexGame.savesData.BallsPosY[i] = activeBalls[i].Ball.transform.position.y; 
                }
            }
            YandexGame.SaveProgress(); 
        }
    }

    private void LoadBalls() {
        for (int i = 0; i < YandexGame.savesData.BallsSizesNames.Length; i++) {
            Vector3 ballPosition = new Vector3(YandexGame.savesData.BallsPosX[i], YandexGame.savesData.BallsPosY[i], 0f);

            if (Enum.TryParse(YandexGame.savesData.BallsSizesNames[i], out BallSize savedBallSize)) {
                Pool.Instance.Take(savedBallSize, ballPosition);
            }
        }
    }

    private void ResetBalls() {
        YandexGame.savesData.BallsSizesNames = null;
        YandexGame.savesData.BallsPosX = null;
        YandexGame.savesData.BallsPosY = null;
    }

    [ContextMenu("DeletePrefs")]
    private void DeletePrefs() {
        ResetBalls();
        YandexGame.savesData.MaxScores = 0;
        YandexGame.SaveProgress();
    }
}