using UnityEngine;
using GameRuntime;
using UnityEngine.Events;
using TakeAim;
using System;

namespace Character {
    public enum BallSize {
        Universal = 0,
        Little = 1,
        Tiny = 2,
        Smallest = 3,
        Small = 4,
        Middle = 5,
        Big = 6,
        Bigger = 7,
        Biggest = 8,
        Large = 9,
        Gigant = 10,
        Dissapear = 11
    }

    [RequireComponent(typeof(BallCollision), typeof(BallSelection))]
    public partial class Ball : MonoBehaviour {
        public UnityAction OnFirstCollision;        
        public BallSize Size { get => _size;}
        [SerializeField] private BallSize _size;

        public Rigidbody2D RB { get => _rigidbody;}
        [SerializeField] private Rigidbody2D _rigidbody;
        public CircleCollider2D CircleCollider { get => _circleCollider; private set => _circleCollider = value; }
        [SerializeField] private CircleCollider2D _circleCollider;
        public BallCollision CollisionBehaviour { get => _collision; private set => _collision = value; }

        public BallSelection Selection { get => _selection; private set => _selection = value; }
        [SerializeField] private BallSelection _selection;

        [SerializeField] private BallCollision _collision;
        private bool _isFirstCollisionHappen;

        public void Refresh() => _isFirstCollisionHappen = false;

        private void Notify(Collision2D collision) {
            if (!_isFirstCollisionHappen && IsItFirstCollision(ref collision)) {
                _isFirstCollisionHappen = true;
                OnFirstCollision?.Invoke();
            }
            CollisionController.Instance.ManageCollision(this, collision);
        }

        private bool IsItFirstCollision(ref Collision2D collision) {
            if (PositionController.Instance.Coll != collision.collider) {
                if (collision.gameObject.tag is "Ball" or "Box") {
                    return true;
                } 
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        private void OnEnable() => CollisionBehaviour.OnCollision += Notify;
        private void OnDisable() => CollisionBehaviour.OnCollision -= Notify;
    }



    public partial class Ball : MonoBehaviour {
        public State CurrentState { get; set; }

        public StateBox StateBox = new StateBox();
        public StateTakeAim StateTakeAim = new StateTakeAim();
        public StateForceUpBonuce StateForceUpBonuce = new StateForceUpBonuce();

        public void SwitchState(State state) {
            CurrentState = state;
            state.EnterState(this);
        }

        public void FixedUpdate() {
            CurrentState?.UpdateState(this);
        }


#if UNITY_EDITOR
        private void OnValidate() {
            CollisionBehaviour = CollisionBehaviour != null ? CollisionBehaviour : GetComponent<BallCollision>();
            _rigidbody = _rigidbody != null ? _rigidbody : GetComponent<Rigidbody2D>();
            CircleCollider = CircleCollider != null ? CircleCollider : GetComponent<CircleCollider2D>();
            _selection = _selection != null ? _selection : GetComponent<BallSelection>();
        }
#endif
    }
}