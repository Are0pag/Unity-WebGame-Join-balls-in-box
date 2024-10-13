#if UNITY_EDITOR
using CustomEdit;
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AnimPanel : MonoBehaviour {
    public UnityAction OnPanelEnable { get; set; }
    public Coroutine PlayingCoroutine { get => _playingCoroutine; private set => _playingCoroutine = value; }
    private Coroutine _playingCoroutine;

    [SerializeField][Min(0f)] private float _timeOfPlaying;
    [SerializeField] private bool _isDisableSelf;
    private readonly float _defaultTimeOfPlaying = 1.0f;

    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;

#if UNITY_EDITOR
    [SerializeField] private AutoAnchores _autoAnchores; 
#endif

    #region SerializeFields
    [Header("Default Data")]
    [SerializeField] private Vector3 _defaultLocalPos;
    [SerializeField] private Quaternion _defaultLocalRot;
    [SerializeField] private Vector3 _defaultLocalScale;
    [SerializeField] private Vector2 _defaultSizeDelta;
    [SerializeField] private Vector2 _defaultAnchoredPosition;
    [SerializeField] private float _defaultTransparency;
    //[SerializeField] private Color _defaultColor;

    [Header("Delta Data")]
    [SerializeField] private Vector3 _deltaLocalPos;
    [SerializeField] private Quaternion _deltaLocalRot;
    [SerializeField] private Vector3 _deltaLocalScale;
    [SerializeField] private Vector2 _deltaSizeDelta;
    [SerializeField] private Vector2 _deltaAnchoredPosition;
    [SerializeField] private float _deltaTransparency;
    #endregion

    #region ContextMenuMethods
    public enum InputType {
        DefaultData,
        DeltaData,
        Return
    }

    private void ExecuteRequest(InputType type) {
#if UNITY_EDITOR
        if (TryGetComponent(out AutoAnchores autoAnchores)) {
            _autoAnchores = autoAnchores;
            _autoAnchores.enabled = false;
        } 
#endif

        switch (type) {
            case InputType.DefaultData:
                SetDefaultData();
                break;
            case InputType.DeltaData:
                SetDeltaData();
                break;
            case InputType.Return:
                SetDefaultPos();
                break;
        }

#if UNITY_EDITOR
        if (_autoAnchores != null) {
            _autoAnchores.enabled = true;
        } 
#endif
    }

    [ContextMenu("Remember Default Data")]
    public void SetDefaultDataInput() => ExecuteRequest(InputType.DefaultData);

    [ContextMenu("Remember Delta Data")]
    public void SetDeltaDataInput() => ExecuteRequest(InputType.DeltaData);

    [ContextMenu("Set Default Pos")]
    public void SetDefaultPosInput() => ExecuteRequest(InputType.Return);

    private void SetDefaultData() {
        ReadFromRectTransform();
        DetectOtherSettings();
    }
    private void SetDeltaData() {
        _deltaLocalPos = transform.localPosition;
        _deltaLocalRot = transform.localRotation;
        _deltaLocalScale = transform.localScale;
        _deltaSizeDelta = _rectTransform.sizeDelta;
        _deltaAnchoredPosition = _rectTransform.anchoredPosition;
        if (_canvasGroup != null) {
            _deltaTransparency = _canvasGroup.alpha;
        }
    }
    private void SetDefaultPos() {
        transform.localPosition = _defaultLocalPos;
        transform.localRotation = _defaultLocalRot;
        transform.localScale = _defaultLocalScale;
        _rectTransform.sizeDelta = _defaultSizeDelta;
        _rectTransform.anchoredPosition = _defaultAnchoredPosition;
        if (_canvasGroup != null) {
            _canvasGroup.alpha = _defaultTransparency;
        }
    }

    private void DetectOtherSettings() {
        if (TryGetComponent(out CanvasGroup canvasGroup)) {
            _canvasGroup = _canvasGroup != null ? _canvasGroup : canvasGroup;
            _defaultTransparency = _canvasGroup.alpha;
        }
    }
    private void ReadFromRectTransform() {
        _rectTransform = _rectTransform != null ? _rectTransform : GetComponent<RectTransform>();
        _defaultLocalPos = transform.localPosition;
        _defaultLocalRot = transform.localRotation;
        _defaultLocalScale = transform.localScale;
        _defaultSizeDelta = _rectTransform.sizeDelta;
        _defaultAnchoredPosition = _rectTransform.anchoredPosition;
    }

    #endregion

    private void OnEnable() {
        PlayingCoroutine = StartCoroutine(StartAnimation(true));
    }
    public void PlayEnableAnimation(CallbackVoid call) {
        StopPlaying();
        PlayingCoroutine = StartCoroutine(StartAnimation(true, call));
    }

    public void DisablePanel(CallbackVoid callback) {
        if (true) {
            if (gameObject.activeSelf) {
                StartCoroutine(StartAnimation(false, callback));
            }
        }
    }


    private IEnumerator StartAnimation(bool isItEnableAnim, CallbackVoid call = null) {
        float elapsedTime = 0f;
        while (elapsedTime < SetAnimTime()) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / SetAnimTime();
            if (isItEnableAnim) {
                SetPropertiesOnEnableAnimation(t);
            }
            else {
                SetPropertiesOnDisableAnimation(t);
            }
            yield return null;
        }

        if (isItEnableAnim) {
            OnPanelEnable?.Invoke();
        }
        call?.Invoke();
        StopPlaying();
    }

    private void StopPlaying() {
        if (PlayingCoroutine != null) {
            StopCoroutine(PlayingCoroutine);
            PlayingCoroutine = null;
        }
    }

    private void SetPropertiesOnDisableAnimation(float t) {
        transform.localPosition = Vector3.Lerp(_defaultLocalPos, _deltaLocalPos, t);
        transform.localRotation = Quaternion.Lerp(_defaultLocalRot, _deltaLocalRot, t);
        transform.localScale = Vector3.Lerp(_defaultLocalScale, _deltaLocalScale, t);
        _rectTransform.sizeDelta = Vector2.Lerp(_defaultSizeDelta, _deltaSizeDelta, t);
        _rectTransform.anchoredPosition = Vector2.Lerp(_defaultAnchoredPosition, _deltaAnchoredPosition, t);
        if (_canvasGroup != null) {
            _canvasGroup.alpha = Mathf.Lerp(_defaultTransparency, _deltaTransparency, t);
        }
    }

    private void SetPropertiesOnEnableAnimation(float t) {
        transform.localPosition = Vector3.Lerp(_deltaLocalPos, _defaultLocalPos, t);
        transform.localRotation = Quaternion.Lerp(_deltaLocalRot, _defaultLocalRot, t);
        transform.localScale = Vector3.Lerp(_deltaLocalScale, _defaultLocalScale, t);
        _rectTransform.sizeDelta = Vector2.Lerp(_deltaSizeDelta, _defaultSizeDelta, t);
        _rectTransform.anchoredPosition = Vector2.Lerp(_deltaAnchoredPosition, _defaultAnchoredPosition, t);
        if (_canvasGroup != null) {
            _canvasGroup.alpha = Mathf.Lerp(_deltaTransparency, _defaultTransparency, t);
        }
    }

    private float SetAnimTime() {
        if (_timeOfPlaying == 0) {
            return _defaultTimeOfPlaying;
        }
        else {
            return _timeOfPlaying;
        }
    }
}