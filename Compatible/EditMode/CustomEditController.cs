using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
namespace CustomEdit {
    [ExecuteInEditMode]
    public class CustomEditController : MonoBehaviour {
        public List<GameObject> InitChilds { get => _initChilds; private set => _initChilds = value; }
        [SerializeField] private List<GameObject> _initChilds = new List<GameObject>();

        private void Update() {
            if (transform.childCount == _initChilds.Count) {
                return;
            }
            if (transform.childCount > _initChilds.Count) {
                FindUnprosessedGameObjects();
            }
            if (transform.childCount < _initChilds.Count) {
                var newList = _initChilds.Intersect(GetCurrentChilds()).ToList();
                _initChilds = new List<GameObject>();
                _initChilds = newList;
            }
        }

        private void FindUnprosessedGameObjects() {
            for (int i = 0; i < transform.childCount; i++) {
                if (!_initChilds.Contains(transform.GetChild(i).gameObject)) {
                    _initChilds.Add(transform.GetChild(i).gameObject);
                    InitChild(transform.GetChild(i).gameObject);
                }
            }
        }

        private void InitChild(GameObject child) {
            if (!child.TryGetComponent(out AutoAnchores autoAnchores)) {
                child.AddComponent<AutoAnchores>();
            }
            if (!child.TryGetComponent(out CustomEditController customEditController)) {
                child.AddComponent<CustomEditController>();
            }
        }
        private List<GameObject> GetCurrentChilds() {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++) {
                list.Add(transform.GetChild(i).gameObject);
            }
            return list;
        }
    }
}
#endif