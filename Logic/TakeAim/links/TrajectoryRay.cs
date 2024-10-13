using UnityEngine;

namespace TakeAim {
    public class TrajectoryRay : MonoBehaviour {
        public static TrajectoryRay Instance { get; private set; }
        const float DISTANCE = Mathf.Infinity;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField][Range(0.35f, 0.45f)] private float _offset;

        private void Awake() => Instance = this;

        private void FixedUpdate() {
            if (TakeAimManager.Instance.State == TakeAimManager.InputState.TakeAim) {
                var startPoint = new Vector2(transform.position.x, (transform.position.y - _offset));
                RaycastHit2D hit = Physics2D.Raycast(startPoint, Vector2.down, DISTANCE, _layerMask);
                if (hit.collider != null) {
                    Vector2[] points = { startPoint, hit.point };

                    for (int i = 0; i < points.Length; i++)
                        _lineRenderer.SetPosition(i, points[i]);
                } 
            }
        }

        // Стирает линию
        public void EraseLine() {
            _lineRenderer.SetPosition(0, Vector2.zero);
            _lineRenderer.SetPosition(1, Vector2.zero);
        }


        private void OnValidate() {
            _lineRenderer = _lineRenderer != null ? _lineRenderer : GetComponent<LineRenderer>();
        }
    } 
}