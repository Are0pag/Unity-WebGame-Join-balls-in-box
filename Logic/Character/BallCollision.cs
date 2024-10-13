using UnityEngine;
using UnityEngine.Events;

namespace Character {
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallCollision : MonoBehaviour {
        public UnityAction<Collision2D> OnCollision {  get; set; }
        [SerializeField] private Rigidbody2D _rb;

        private void OnCollisionEnter2D(Collision2D collision) => OnCollision?.Invoke(collision);

        private void OnValidate() => _rb = _rb != null ? _rb : GetComponent<Rigidbody2D>();
    }  
}
