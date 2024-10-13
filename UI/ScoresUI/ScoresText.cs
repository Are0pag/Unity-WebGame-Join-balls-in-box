using TMPro;
using UnityEngine;

public class ScoresText : MonoBehaviour {
	public static ScoresText Instance {get; private set; }
    private void Awake() => Instance = this;

    [SerializeField] private TextMeshProUGUI _recordText, _currentScores;

    public void DisplayCurrentScores() => _currentScores.text = CurrencyManager.ScoresForOneGame.ToString();
    public void DisplauRecord(int scores) => _recordText.text = scores.ToString();

    private void OnEnable() => CurrencyManager.OnBalanceChange += DisplayCurrentScores;
    private void OnDisable() => CurrencyManager.OnBalanceChange -= DisplayCurrentScores;

}