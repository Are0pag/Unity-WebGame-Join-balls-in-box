using Character;
using DataClasses;
using UnityEngine;
using UnityEngine.Events;

namespace GameRuntime {
    public class CollisionController : MonoBehaviour {
        public static CollisionController Instance { get; private set; }
        public static UnityAction<RegisteredCollision> OnSameBallsCollide { get; set; }

        private RegisteredCollision _previousCollision;

        private void Awake() => Instance = this;

        public void ManageCollision(Ball sender, Collision2D collision2D) {
            var facingBall = GetBallFromCollision(ref collision2D);
            // при столкновении именно с шаром
            if (facingBall != null) {
                if (sender.Size == facingBall.Size || sender.Size == BallSize.Universal || facingBall.Size == BallSize.Universal) {                    
                    RegisterCollision(new RegisteredCollision(sender, facingBall, collision2D));
                }
            }
        }
        private void Notify(ref RegisteredCollision data) {
            OnSameBallsCollide?.Invoke(data);
            _previousCollision = null;
        }

        private void RegisterCollision(RegisteredCollision data) {
            if (_previousCollision == null) {
                _previousCollision = data;
                return;
            }
            if (data.Collision2D.Equals(_previousCollision.Collision2D)) {
                FindContactPoint(ref data);
            }
        }

        private void FindContactPoint(ref RegisteredCollision data) {
            for (int i = 0; i < _previousCollision.Collision2D.contacts.Length; i++) {
                for (int j = 0; j < data.Collision2D.contacts.Length; j++) {
                    if (data.Collision2D.contacts[j].point == _previousCollision.Collision2D.contacts[i].point) {
                        data.CollisionPoint = data.Collision2D.contacts[j].point;
                        Notify(ref data);
                        return;
                    }
                }
            }
        }


        private Ball GetBallFromCollision(ref Collision2D collision2D) {
            if (collision2D.gameObject.TryGetComponent(out Ball facingBall)) {
                return facingBall;
            }
            return null;
        }
    }
}

namespace DataClasses {
    public class RegisteredCollision {
        public Ball sender, facingBall;
        public Collision2D Collision2D;
        public Vector2 CollisionPoint;
        public RegisteredCollision(Ball sender, Ball facingBall, Collision2D collision2D) {
            this.sender = sender;
            this.facingBall = facingBall;
            Collision2D = collision2D;
        }
    }
}