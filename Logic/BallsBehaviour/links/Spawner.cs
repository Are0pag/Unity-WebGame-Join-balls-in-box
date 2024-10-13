using Character;
using UnityEngine;

namespace GameRuntime {
    public class Spawner : MonoBehaviour {
        public static Spawner Instance { get; private set; }

        [SerializeField] private Transform _parentTramsform;
        [SerializeField] private float _posZ;

        private void Awake() => Instance = this;

        public GameObject InstantiateBall(BallSize ballSize, Vector3 pos) {
            var go = Instantiate(GameData.Instance.GetBallBySize(ballSize).Prefab,
                pos, Quaternion.identity, _parentTramsform);
            return go;
        }
    }
}