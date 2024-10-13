using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Messages {
    public class Links : MonoBehaviour {
        public List<Variants> Variants { get => _variants; private set => _variants = value; }
        [SerializeField] private List<Variants> _variants;

        public TextMeshProUGUI Text { get => _text; private set => _text = value; }
        public Button Button { get => _button; private set => _button = value; }
        public GameObject Panel { get => _panel; private set => _panel = value; }
        public GameObject[] Settings { get => _settings; private set => _settings = value; }

        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private GameObject[] _settings;

        public Variants GetDataByMessageType(PauseManager.MessageType type) {
            return Variants.Where(v => v.MessageType == type).FirstOrDefault();
        }
    }

    [System.Serializable]
    public class Variants {
        public PauseManager.MessageType MessageType;
        [TextArea] public string MessageText;
    }
}