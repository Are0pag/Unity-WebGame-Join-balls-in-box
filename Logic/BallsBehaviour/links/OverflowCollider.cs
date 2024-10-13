using Character;
using TakeAim;
using UnityEngine;
using UnityEngine.Events;

namespace GameRuntime {
    public class OverflowCollider : MonoBehaviour {

        public static UnityAction OnOverflow { get; set; }

        //private void OnCollisionEnter2D(Collision2D collision) {
        //    if (collision.gameObject.TryGetComponent(out Ball ball)) {
        //        if (ball.CurrentState == ball.StateBox) {
        //            OnOverflow?.Invoke();
        //        }
        //    }
        //}

        private void OnTriggerEnter2D(Collider2D collision) {
            if (TakeAimManager.Instance.State != TakeAimManager.InputState.WaitingForFirstCollision) {
                if (collision.gameObject.TryGetComponent(out Ball ball)) {
                    if (ball.CurrentState == ball.StateBox) {
                        OnOverflow?.Invoke();
                    }
                } 
            }
        }
    } 
}