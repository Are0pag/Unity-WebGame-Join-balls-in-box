
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game_Bonuses {
    public class SkillHint : MonoBehaviour {
        public static SkillHint Instance;
        [SerializeField] private float _activeTime;
        [SerializeField] private List<HintText> _hintText;
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        private void Awake() {
            Instance = this;
        }

        public List<HintText> HintText { get => _hintText; private set => _hintText = value; }
        public void InitTopDown(Bonuses bonuse) {
            _textMeshPro.text = _hintText.First(t => t.Bonuses == bonuse).TextOfHint;
        }

        private void OnEnable() {
            Invoke(nameof(Diasable), _activeTime);
        }

        private void Diasable() {
            gameObject.SetActive(false);
        }
    }
    

    [System.Serializable]
    public class HintText {
        public Bonuses Bonuses;
        [TextArea] public string TextOfHint;
    } 
}