using System.Collections;
using TMPro;
using UnityEngine;
using YG;

public class Advertisment : MonoBehaviour {
    public static Advertisment Instance { get; private set; }
    public Coroutine WorkingCoroutine { get; set; }
    private void Awake() => Instance = this;

    [SerializeField] private float _delayAfterErrorFullScreen;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameObject _panel;
    private const float ADV_SHOW_RATE = 60f;
    private const float REMAINING_TIME_BEFORE_ADV_WHEN_WARNING_TIMER_DISPLAY = 3.5f;

    private void Start() {
        StartTimer();
    }

    //private void OnEnable() {
    //    YandexGame.CloseFullAdEvent += StartTimer;
    //    YandexGame.ErrorFullAdEvent += OnError;
    //}

    //private void OnDisable() {
    //    YandexGame.CloseFullAdEvent -= StartTimer;
    //    YandexGame.ErrorFullAdEvent -= OnError;
    //}

    public void StartTimer() {
        _panel.SetActive(false);
        SetCoroutine();
    }

    private void SetCoroutine() {
        if (WorkingCoroutine != null) {
            StopCoroutine(WorkingCoroutine);
        }
        WorkingCoroutine = null;
        WorkingCoroutine = StartCoroutine(AdvTimer());
    }

    public void OnError() => Invoke(nameof(ShowFullScreenAdv), _delayAfterErrorFullScreen);

    private void ShowFullScreenAdv() {
        PauseManager.Instance.ShowToPlayer(PauseManager.MessageType.Pause);
        YandexGame.FullscreenShow();
    }

    private IEnumerator AdvTimer() {
        float elapsedTime = 0f;
        while (elapsedTime < ADV_SHOW_RATE) {
            elapsedTime += Time.deltaTime;
            if (ADV_SHOW_RATE - elapsedTime < REMAINING_TIME_BEFORE_ADV_WHEN_WARNING_TIMER_DISPLAY) {

                yield return new WaitWhile(() => GameManager.Instance.State == GameState.BonusFreese);

                _panel.SetActive(true);
                _timerText.text = (ADV_SHOW_RATE - elapsedTime).ToString("F0");
            }
            yield return null;
        }        
        ShowFullScreenAdv();
    }
}