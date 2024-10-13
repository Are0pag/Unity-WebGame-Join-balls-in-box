using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour {
    public Coroutine WorkingCoroutine { get; set; }
    [SerializeField] private TextMeshProUGUI _textField;
    private readonly float _fullTransparenty = 1f, _startScale = 1f;
    private void OnEnable() => SetDefaultProperties();

    public void InitTopDown(int displayNumber, float timeOfEffect, float finalScale, float deltaPosYUp) {
        _textField.text = "+ " + displayNumber.ToString();
        WorkingCoroutine = StartCoroutine(SetFloatingText(timeOfEffect, finalScale, deltaPosYUp));
    }

    private void SetDefaultProperties() {
        _textField.alpha = _fullTransparenty;
        transform.localScale = new Vector3(_startScale, _startScale, _startScale);
    }

    private IEnumerator SetFloatingText(float timeOfEffect, float finalScale, float deltaPosYUp) {
        float startPosY = transform.position.y;
        float finalPosY = transform.position.y + deltaPosYUp;

        float elapsedTime = 0f;
        while (elapsedTime < timeOfEffect) {
            elapsedTime += Time.deltaTime;

            float transparensyEffectScale = Mathf.Lerp(_fullTransparenty, 0f, elapsedTime / timeOfEffect);
            _textField.alpha = transparensyEffectScale;

            float scaleEffectScale = Mathf.Lerp(_startScale, finalScale, elapsedTime / timeOfEffect);
            transform.localScale = new Vector3(scaleEffectScale, scaleEffectScale, scaleEffectScale);

            float positionEffectScale = Mathf.Lerp(startPosY, finalPosY, elapsedTime / timeOfEffect);
            transform.position = new Vector3(transform.position.x, positionEffectScale, transform.position.z);

            yield return null;
        }
        StopCoroutine(WorkingCoroutine);
        WorkingCoroutine = null;
        ScoresDisplayManager.Instance.Release(this);
    }

    private void OnValidate() {
        _textField = _textField != null ? _textField : transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
}