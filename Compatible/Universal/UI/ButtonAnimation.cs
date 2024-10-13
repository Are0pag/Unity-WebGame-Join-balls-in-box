using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


//[RequireComponent(typeof(ButtonAnimController))]
public class ButtonAnimation : EventTrigger {
    [SerializeField][Min(0f)] private float _animTime;
    private const float LOCALPOSZ = 1f;
    private float _enterLocalScale = 1f;
    private readonly float _exitLocalScale = 1.05f;
    private static readonly float _defaultEffectTime = 0.5f;
    private float _effectTime;

    private void OnEnable() => _effectTime = SetEffectTime();

    // Теперь через корутину, чтобы не ебаться в анимацией каждой кнопки
    public override void OnPointerEnter(PointerEventData data) {
        StartCoroutine(SetSmoothScale(true));        
    }


    public override void OnPointerExit(PointerEventData data) {
        StartCoroutine(SetSmoothScale(false));
    }

    private IEnumerator SetSmoothScale(bool isEnterAnim) {
        float elapsedTime = 0f;
        while (elapsedTime < _effectTime) {
            elapsedTime += Time.deltaTime;

            float effectScale = 0f;
            if (isEnterAnim) { effectScale = Mathf.Lerp(_enterLocalScale, _exitLocalScale, elapsedTime / _effectTime); }
            else { effectScale = Mathf.Lerp(_exitLocalScale, _enterLocalScale, elapsedTime / _effectTime); }

            //_buttonAnimController.GameObjectEffectTargetTransform
            gameObject.transform.localScale = new Vector3(effectScale, effectScale, LOCALPOSZ);
            yield return null;
        }
        StopCoroutine(nameof(SetSmoothScale));
    }

    private void OnValidate() {
        //_buttonAnimController = GetComponent<ButtonAnimController>();
    }
    private float SetEffectTime() {
        if (_animTime != 0) {
            return _animTime;
        }
        return _defaultEffectTime;
    }
    //private void print() {
    //    if (_buttonAnimController.EnableDebugPrint) {
    //        print("OnPointerEnter");
    //    }
    //}
}