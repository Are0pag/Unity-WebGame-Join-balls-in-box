using UnityEngine;
using DataClasses;
using Character;
using TakeAim;
using UnityEngine.Events;

namespace GameRuntime {
    public class BallsController : MonoBehaviour {
        public static BallsController Instance { get; private set; }
        public static UnityAction<BallSize, Vector3> OnBiggerOneSpawn { get; set; }
        private void Awake() => Instance = this;

        public Ball EnterNewBallOnScene(BallSize size, Vector3 posEqualToTakeAim, bool notifyAboutFirsCollision) {
            var ball = Pool.Instance.Take(size, posEqualToTakeAim).GetComponent<Ball>();
            SetBallState(ref ball, ball.StateTakeAim);
            SetBallCallback(ball, notifyAboutFirsCollision);
            return ball;
        }

        public void OnSameBallsCollide(RegisteredCollision data) {
            Pool.Instance.Release(data.sender.gameObject, data.facingBall.gameObject);
            var collisionPoint = new Vector3(data.CollisionPoint.x, data.CollisionPoint.y, 0f);
            BallSize nextSize = ManageInfo(ref data);

            OnBiggerOneSpawn?.Invoke(nextSize, collisionPoint);

            if (nextSize != BallSize.Dissapear) {
                var ball = Pool.Instance.Take(nextSize, collisionPoint).GetComponent<Ball>();
                SetBallState(ref ball, ball.StateBox);
                AudioManager.Instanse.PlaySound(AudioManager.Instanse.MagicClip);

                if (nextSize == BallSize.Gigant) {
                    PauseManager.Instance.ShowToPlayer(PauseManager.MessageType.Win);
                }
            }
        }

        public void SetBallState(ref Ball ball, Character.State state) => ball.SwitchState(state);
        public void SetBallCallback(Ball ball, bool notifyAboutFirsCollision) {
            if (notifyAboutFirsCollision) {
                ball.Refresh();
                ball.OnFirstCollision += () => {
                    TakeAimManager.Instance.EnterNewBall();
                    ball.OnFirstCollision = null;
                };
            }
            else {
                ball.OnFirstCollision = null;
            }
        }

        private BallSize ManageInfo(ref RegisteredCollision data) {
            if (data.sender.Size == BallSize.Universal) {
                return GetNextSize(data.facingBall.Size);
            }
            else if (data.facingBall.Size == BallSize.Universal) {
                return GetNextSize(data.sender.Size);
            }
            else {
                return GetNextSize(data.facingBall.Size);
            }
        }
        private BallSize GetNextSize(BallSize previousSize) => (BallSize)(1 + ((int)previousSize));

        private void OnEnable() => CollisionController.OnSameBallsCollide += OnSameBallsCollide;
        private void OnDisable() => CollisionController.OnSameBallsCollide -= OnSameBallsCollide;
    }
}