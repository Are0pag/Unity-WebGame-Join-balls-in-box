using UnityEngine;
using YG;

public class LeaderboardProvider : MonoBehaviour {
    public static LeaderboardProvider Instance { get; private set; }
    public int MaxScores { get; private set; }
    public int ScoresForAllTime { get; private set; }
    [SerializeField] private string _recordScoresLB, _allScoresLB;

    private void Awake() => Instance = this;
    private void Start() => ScoresText.Instance.DisplauRecord(MaxScores);
    public void SetScores(int max, int all) {
        MaxScores = max;
        ScoresForAllTime = all;
    }

    public void AddScoresForAllTime(int amount) {
        ScoresForAllTime += amount;        
        YandexGame.NewLeaderboardScores(_allScoresLB, ScoresForAllTime);
    }
    public void SetRecord(int record) {
        MaxScores = record;
        SavesProvider.Instance.SaveRecord(record);
        YandexGame.NewLeaderboardScores(_recordScoresLB, MaxScores);
        ScoresText.Instance.DisplauRecord(record);
    }

    [ContextMenu("PrintScores")]
    private void PrintScores() {
        print(MaxScores.ToString());
    }
}