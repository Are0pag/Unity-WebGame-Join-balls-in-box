#if UNITY_EDITOR

using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace CustomEdit {
    [ExecuteInEditMode]
    public class ComponentsSetterByNames : MonoBehaviour {
        [SerializeField] private Component _component;
        [SerializeField] private bool isComponentAlreadyAdded;

        private void Update() {
            string thisName = transform.gameObject.name;
            if (thisName.Contains("Image", System.StringComparison.CurrentCultureIgnoreCase)) {
                AddAutomatically<Image>();
            }
            if (thisName.Contains("text", System.StringComparison.CurrentCultureIgnoreCase)) {
                AddAutomatically<TextMeshProUGUI>();
            }
        }

        private void AddAutomatically<C>() where C : Component {
            if (_component == null) {
                _component = transform.gameObject.AddComponent<C>();
                ComponentUtility.CopyComponent(_component);
            }

            if (_component != null && !isComponentAlreadyAdded) {
                ComponentUtility.PasteComponentAsNew(transform.gameObject);
                isComponentAlreadyAdded = true;
                // this.enabled = false;
            }
        }
    }
}
#endif