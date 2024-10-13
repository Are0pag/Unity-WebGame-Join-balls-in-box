using UnityEngine;

namespace Character {
    public abstract class State {
        public abstract void EnterState(Ball ball);

        public abstract void UpdateState(Ball ball);

        public abstract void ExitState(Ball ball, State state);
    }
}