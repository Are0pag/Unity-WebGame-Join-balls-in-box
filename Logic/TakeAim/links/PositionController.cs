using UnityEngine;
using UnityEngine.Events;

namespace TakeAim {
    [RequireComponent (typeof(CircleCollider2D))]
	public class PositionController : MonoBehaviour {
		public static PositionController Instance { get; private set; }
        public static UnityAction OnNewBallReadyToEnter;        
        public CircleCollider2D Coll { get => _coll; private set => _coll = value; }
        [SerializeField] private CircleCollider2D _coll;

        [SerializeField] private Transform _rigthBound, _leftBound;

		private void Awake() => Instance = this;

        private void OnMouseDrag() {
            if (IsGameStateAllows()) {
                transform.position = new Vector3(GetPositionX(), transform.position.y, 0f);
            }
        }
        private void OnMouseUp() {
            if (IsGameStateAllows()) {
                OnNewBallReadyToEnter?.Invoke(); 
            }
        }

        private float GetPositionX() {
            var mousePosX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            if (mousePosX > _rigthBound.position.x) {
                return _rigthBound.position.x;
            }
            if (mousePosX < _leftBound.position.x) {
                return _leftBound.position.x;
            }
            return mousePosX;
        }

        private bool IsGameStateAllows() {
            if (GameManager.Instance.State == GameState.Playing) {
                if (TakeAimManager.Instance.State == TakeAimManager.InputState.TakeAim) {
                    return true;
                }
            }
            return false;
        }
    } 
}