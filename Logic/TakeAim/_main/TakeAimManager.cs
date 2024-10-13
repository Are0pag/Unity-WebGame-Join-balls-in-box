using Game_Bonuses;
using Character;
using GameRuntime;
using UnityEngine;
using UnityEngine.Events;

namespace TakeAim {
    public class TakeAimManager : MonoBehaviour {
        public enum InputState {
            TakeAim,
            WaitingForFirstCollision
        }

        public static TakeAimManager Instance { get; private set; }
        private BallSize _sizeOfEnteringBall;
        public InputState State { get; private set; }
        public Ball BallOnDrag { get; private set; }
        public UnityAction ForceToBallOnDrag { get; set; }
        private Transform _toggle => PositionController.Instance.gameObject.transform;

        private void Awake() => Instance = this;

        public void EnterNewBall() {
            State = InputState.TakeAim;            
            _sizeOfEnteringBall = EnteringBalls.Instance.GetEnteringBall();
            PositionController.Instance.Coll.enabled = true;
            GetBall(_sizeOfEnteringBall, true);
        }

        public void ChangeBall(bool isUniversal = false) {
            BallsController.Instance.SetBallCallback(BallOnDrag, false);
            Pool.Instance.Release(BallOnDrag.gameObject);
            if (!isUniversal) {
                _sizeOfEnteringBall = EnteringBalls.Instance.ChanheBall(_sizeOfEnteringBall); 
            }
            else {
                _sizeOfEnteringBall = BallSize.Universal;
            }
            GetBall(_sizeOfEnteringBall, true);
        }

        private void GetBall(BallSize size, bool notifyAboutFirsCollision) {
            var pos = new Vector3(_toggle.position.x, _toggle.position.y, 0f);
            BallOnDrag = null;
            BallOnDrag = BallsController.Instance.EnterNewBallOnScene(size, pos, notifyAboutFirsCollision);
        }

        private void Notify() {
            State = InputState.WaitingForFirstCollision;
            TrajectoryRay.Instance.EraseLine();
            PositionController.Instance.Coll.enabled = false;
            BallOnDrag.SwitchState(BallOnDrag.StateBox);
            ForceToBallOnDrag?.Invoke();
            BallOnDrag = null;
        }

        private void OnEnable() {
            PositionController.OnNewBallReadyToEnter += Notify;
        }
        private void OnDisable() {
            PositionController.OnNewBallReadyToEnter -= Notify;
        }
    }
}