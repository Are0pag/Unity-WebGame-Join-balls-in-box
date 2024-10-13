using UnityEngine;

namespace Character {
    public class StateForceUpBonuce : State {
        public override void EnterState(Ball ball) {
            ball.Selection.enabled = true;
        }

        public override void ExitState(Ball ball, State state) {
            ball.Selection.DisableOutline();
            ball.Selection.enabled = false;
        }

        public override void UpdateState(Ball ball) {
            if (GameManager.Instance.State == GameState.Playing) {
                ExitState(ball, ball.StateBox);
            }
        }
    }
}