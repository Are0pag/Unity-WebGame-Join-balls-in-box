using Game_Bonuses;
using UnityEngine;

namespace Character {
    public class StateBox : State {

        public override void EnterState(Ball ball) {
            ball.Selection.enabled = false;
            ball.CircleCollider.enabled = true;
            ball.CollisionBehaviour.enabled = true;

            ball.RB.bodyType = RigidbodyType2D.Dynamic;
            ball.RB.velocity = Vector3.zero;
            ball.RB.inertia = 0f;
        }

        public override void ExitState(Ball ball, State state) {
            ball.SwitchState(state);           
        }

        public override void UpdateState(Ball ball) {
            if (GameManager.Instance.State == GameState.BonusFreese) {
                if (BonusManager.Instance.RunningBonus != Bonuses.AddForceUpToAnyBallInBox) {
                    ball.RB.velocity = Vector3.zero;
                    ball.RB.inertia = 0f; 
                }
                else {
                    ExitState(ball, ball.StateForceUpBonuce);
                }
            }
        }
    }
}