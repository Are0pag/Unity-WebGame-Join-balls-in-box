#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditorInternal;

namespace CustomEdit {
    [ExecuteInEditMode]
    [RequireComponent(typeof(CustomEditController))]
    public class CanvasSetter : MonoBehaviour {
        public ScreenSizesDictionaty.ScreenSizes Size { get => _size; private set => _size = value; }
        [SerializeField] private ScreenSizesDictionaty.ScreenSizes _size;
        private Canvas _canvas;
        private CanvasScaler _canvasScaler;

        private void Update() => ComponentUtility.MoveComponentUp(this);

        private void OnValidate() {
            InitCanvas();
            InitCanvasScaler();
        }

        private void InitCanvas() {
            _canvas = _canvas != null ? _canvas : GetComponent<Canvas>();
            if (_canvas != null) {
                _canvas.vertexColorAlwaysGammaSpace = true;
                _canvas.worldCamera = FindAnyObjectByType<Camera>();
            }
        }

        private void InitCanvasScaler() {
            _canvasScaler = _canvasScaler != null ? _canvasScaler : GetComponent<CanvasScaler>();
            if (_canvasScaler != null) {
                var alreadyExisted = FindObjectsOfType<CanvasSetter>().ToList();
                alreadyExisted.Remove(this);
                if (alreadyExisted.Count > 0) {
                    _size = alreadyExisted.First().Size;
                    SetCanvasScaler(new ScreenSizesDictionaty().Sizes[alreadyExisted.First().Size]);
                }
                else {
                    SetCanvasScaler(new ScreenSizesDictionaty().Sizes[_size]);
                }
            }
        }

        private void SetCanvasScaler(Vector2 size) {
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = size;
            _canvasScaler.matchWidthOrHeight = 0.5f;
        }
    }

    [System.Serializable]
    public class ScreenSizesDictionaty {
        public enum ScreenSizes {
            DeckTop_1366x768,
            Mobile_720x1280
        }
        public Dictionary<ScreenSizes, Vector2> Sizes = new Dictionary<ScreenSizes, Vector2>();
        public ScreenSizesDictionaty() {
            Sizes = new Dictionary<ScreenSizes, Vector2> {
                {ScreenSizes.DeckTop_1366x768, new Vector2(1366, 768) },
                {ScreenSizes.Mobile_720x1280, new Vector2(720, 1280) }
            };
        }
    }
}
#endif