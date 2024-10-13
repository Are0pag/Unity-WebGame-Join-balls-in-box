using TakeAim;
using UnityEngine;

namespace Character {
    public class StateTakeAim : State {
        public override void EnterState(Ball ball) {
            ball.Selection.enabled = false;
            ball.CollisionBehaviour.enabled = false;
            ball.CircleCollider.enabled = false;
        }

        public override void UpdateState(Ball ball) {
            ball.RB.transform.position = PositionController.Instance.gameObject.transform.position;
            ball.RB.inertia = 0f;
            ball.RB.velocity = Vector3.zero;
        }
        public override void ExitState(Ball ball, State state) {
            ball.SwitchState(state);
        }
    }
}