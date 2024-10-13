using System.Collections;
using UnityEngine;

namespace Game_Bonuses {
    public class LithnessDisplay : MonoBehaviour {
        public static LithnessDisplay Instance { get; private set; }
        private void Awake() => Instance = this;
        public Coroutine WorkingCoroutine { get; set; }

        [SerializeField][Tooltip("Equals to LigthnessShader appear time")] private float _effectTime;
        [SerializeField] private float _returnTime, _startRotationZ, _endRotationZ;
        [SerializeField] private GameObject _ligthness;
        [SerializeField] private RectTransform _ligthnessBonusIconTr;

        public void Activate(bool active) => _ligthness.SetActive(active);
        public void SetLigthnessIconMovement() {
            WorkingCoroutine = StartCoroutine(
                SetLigthnessIconMovement(_effectTime, _ligthnessBonusIconTr, _startRotationZ, _endRotationZ));
        }
        public void ReturnLigthnessIconRotation() {
            StopCoroutine(WorkingCoroutine);
            WorkingCoroutine = null;
            WorkingCoroutine = StartCoroutine(
                SetLigthnessIconMovement(_returnTime, _ligthnessBonusIconTr, _endRotationZ, _startRotationZ));
        }

        private IEnumerator SetLigthnessIconMovement(float timeOfEffect, RectTransform rect, float startRotZ, float finalRotZ) {
            float elpsedTime = 0f;
            while (elpsedTime < timeOfEffect) {
                elpsedTime += Time.deltaTime;
                float effectScale = Mathf.Lerp(startRotZ, finalRotZ, elpsedTime/timeOfEffect);
                rect.localEulerAngles = new Vector3(rect.localEulerAngles.x, rect.localEulerAngles.y, effectScale);
                yield return null;
            }
            StopCoroutine(WorkingCoroutine);
            WorkingCoroutine = null;
        }
    }
}