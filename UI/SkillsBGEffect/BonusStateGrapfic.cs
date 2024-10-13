using System.Collections;
using UnityEngine;

namespace Game_Bonuses {
    public class BonusStateGrapfic : MonoBehaviour {
        public static BonusStateGrapfic Instance { get; private set; }
        public Coroutine WorkingCoroutine { get; set; }
        private void Awake() => Instance = this;

        [SerializeField][Range(0f, 1f)] private float _bgFinalAlfa, _fgFinalAlfa;
        [SerializeField] private float _enableTime, _disableTime;
        [SerializeField] private SpriteRenderer _bg, _fg;

        public void EnableBG() {
            _bg.gameObject.SetActive(true);
            _fg.gameObject.SetActive(true);
            WorkingCoroutine = StartCoroutine(Appear(_enableTime));
        }
        public void DisableBG() {
            WorkingCoroutine = StartCoroutine(Dissapear(_disableTime));
        }

        private IEnumerator Appear(float effectTime) {
            float elapsedTime = 0f;
            while (elapsedTime < effectTime) {
                elapsedTime += Time.deltaTime;

                float effectAmountForBG = Mathf.Lerp(0f, _bgFinalAlfa, elapsedTime / effectTime);
                _bg.color = new Color(_bg.color.r, _bg.color.g, _bg.color.b, effectAmountForBG);

                float effectAmountForFG = Mathf.Lerp(0f, _fgFinalAlfa, elapsedTime / effectTime);
                _fg.color = new Color(_fg.color.r, _fg.color.g, _fg.color.b, effectAmountForFG);

                yield return null;
            }
            StopCoroutine(WorkingCoroutine);
            WorkingCoroutine = null;
        }

        private IEnumerator Dissapear(float effectTime) {
            float elapsedTime = 0f;
            while (elapsedTime < effectTime) {
                elapsedTime += Time.deltaTime;

                float effectAmountForBG = Mathf.Lerp(_bgFinalAlfa, 0f, elapsedTime / effectTime);
                _bg.color = new Color(_bg.color.r, _bg.color.g, _bg.color.b, effectAmountForBG);

                float effectAmountForFG = Mathf.Lerp(_fgFinalAlfa, 0f, elapsedTime / effectTime);
                _fg.color = new Color(_fg.color.r, _fg.color.g, _fg.color.b, effectAmountForFG);

                yield return null;
            }
            StopCoroutine(WorkingCoroutine);
            WorkingCoroutine = null;
            _bg.gameObject.SetActive(false);
            _fg.gameObject.SetActive(false);
        }
    } 
}