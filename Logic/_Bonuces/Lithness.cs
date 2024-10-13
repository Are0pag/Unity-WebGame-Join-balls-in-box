using UnityEngine;
using GameRuntime;
using UnityEngine.Events;
using TakeAim;
using Character;

namespace Game_Bonuses {
	public class Lithness : MonoBehaviour {
		public static Lithness Instance { get; private set; }        
        public static UnityAction OnBonusEnds { get; set; }
        public static UnityAction<BallSize, Vector3> OnLigthnessDectroyBall { get; set; }

        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private RectTransform _origin;
        private void Awake() => Instance = this;
        public Vector3 StartPos => Camera.main.ScreenToWorldPoint(_origin.position);

        [ContextMenu("CastLithness")]
        public void CastLithness() {
            RaycastHit2D[] hits = Physics2D.RaycastAll(StartPos, Vector2.down, Mathf.Infinity, _layerMask);
            if (hits.Length > 0) {
                foreach (var hit in hits) {
                    var go = hit.collider.gameObject;
                    if (go != TakeAimManager.Instance.BallOnDrag.gameObject) {
                        var size = go.GetComponent<Ball>().Size;
                        OnLigthnessDectroyBall?.Invoke(size, hit.point);
                        Pool.Instance.Release(go); 
                    }
                }
            }
            OnBonusEnds?.Invoke();
        }
    } 
}