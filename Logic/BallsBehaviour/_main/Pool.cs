using Character;
using DataClasses;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameRuntime {
    public class Pool : MonoBehaviour, IPool {
        public static Pool Instance { get; private set; }
        public List<PoolItem> PooledItems { get => _pooledItems; private set => _pooledItems = value; }
        private List<PoolItem> _pooledItems = new List<PoolItem>();
        private void Awake() => Instance = this;

        public List<PoolItem> GetActiveBalls() {
            return PooledItems.Where(i => i.Ball.activeSelf).ToList();
        }

        /// <summary>
        /// Need to SetFloat this object to Active
        /// </summary>
        /// <param name="ballSize"></param>
        /// <returns></returns>
        public GameObject Take(BallSize ballSize, Vector3 pos) {
            var objectFromPool = GetFromPool(ballSize, pos);
            Init(pos, objectFromPool);
            return objectFromPool;
        }

        public void Release(params GameObject[] objects) {
            foreach (var obj in objects) {
                obj.SetActive(false);
            }
        }

        public GameObject Create(BallSize ballSize, Vector3 pos) {
            var newGO = Spawner.Instance.InstantiateBall(ballSize, pos) as GameObject;
            PooledItems.Add(new PoolItem(newGO, ballSize));
            return newGO;
        }

        public void OnRestart() {
            foreach (var item in PooledItems) {
                item.Ball.SetActive(false);
            }
        }

        private static void Init(Vector3 pos, GameObject objectFromPool) {
            objectFromPool.transform.position = pos;
            if (!objectFromPool.activeSelf) {
                objectFromPool.SetActive(true);
            }
        }

        private GameObject GetFromPool(BallSize ballSize, Vector3 pos) {
            var item = PooledItems.FirstOrDefault(i => !i.Ball.activeSelf && i.Size == ballSize);
            if (item != null) {
                return item.Ball;
            }
            else {
                return Create(ballSize, pos);
            }
        }
    }
}

namespace DataClasses {
    public class PoolItem {
        public GameObject Ball;
        public BallSize Size;

        public PoolItem(GameObject prefab, BallSize ballSize) {
            Ball = prefab;
            Size = ballSize;
        }
    }
}