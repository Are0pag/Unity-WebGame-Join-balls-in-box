using Game_Bonuses;
using UnityEngine;
using UnityEngine.Events;

namespace Character {
    public class BallSelection : MonoBehaviour {

        public static UnityAction OnForceUpBonusCompleted;

        [SerializeField] private Ball _thisBall;
        [SerializeField] private GameObject _selectOutline;

        public void DisableOutline() => _selectOutline.SetActive(false);
        private bool IfCan() {
            return GameManager.Instance.State == GameState.BonusFreese && BonusManager.Instance.RunningBonus == Bonuses.AddForceUpToAnyBallInBox;
        }

        private void OnMouseUp() {
            if (IfCan()) {
                if (BonusManager.Instance != null) {
                    BonusManager.Instance.SetAddForceUpToAnyBallInBox(_thisBall);
                    OnForceUpBonusCompleted?.Invoke();
                } 
            }
        }
        private void OnMouseEnter() {
            if (IfCan()) {
                if (_selectOutline != null) {
                    _selectOutline.SetActive(true);
                } 
            }
        }
        private void OnMouseExit() {
            if (IfCan()) {
                if (_selectOutline != null) {
                    _selectOutline.SetActive(false);
                } 
            }
        }



        private void OnValidate() {
            _thisBall = _thisBall != null ? _thisBall : GetComponent<Ball>();
            if (transform.childCount > 0) {
                _selectOutline = _selectOutline != null ? _selectOutline : transform.GetChild(0).gameObject;
            }
        }
    }
}