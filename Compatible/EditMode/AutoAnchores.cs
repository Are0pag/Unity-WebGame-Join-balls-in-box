#if UNITY_EDITOR

using UnityEditorInternal;
using UnityEngine;

namespace CustomEdit {
	[ExecuteInEditMode]
	public class AutoAnchores : MonoBehaviour {
		private RectTransform _rect;

        private void Update() {
            SetComponentPosition();
            SetUIObjectPosition();

        }

        private void SetUIObjectPosition() {
            _rect.localPosition = Vector3.zero;
            _rect.anchoredPosition = Vector3.zero;
            _rect.sizeDelta = Vector3.zero;
        }

        private void SetComponentPosition() => ComponentUtility.MoveComponentDown(this);
        private void OnValidate() => _rect = _rect != null ? _rect : GetComponent<RectTransform>();
    }
} 
#endif