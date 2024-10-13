using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour {
    [Header("Hint")]
    [SerializeField] private float _timeOfActive;
    [SerializeField][TextArea] private string _text;

    [Header("Referenses")]
    [SerializeField] private Button _but;
    [SerializeField] private GameObject _hint;
    [SerializeField] private AnimPanel[] _animPanel;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    private void OnEnable() { 
        _hint.SetActive(false);
        _but.onClick.AddListener(() => {
            _textMeshProUGUI.text = null;
            var anim = _hint.GetComponent<AnimPanel>();
            if (anim.OnPanelEnable ==  null) {
                anim.OnPanelEnable += () => {
                    _textMeshProUGUI.text = _text;
                };
            }
            _hint.SetActive(true);            
            Invoke(nameof(DisableByTime), _timeOfActive);
        });
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            _hint.SetActive(false);
        }
    }

    private void DisableByTime() {
        _textMeshProUGUI.text = null;
        foreach (var panel in _animPanel) {
            panel.DisablePanel(() => {
                _hint.SetActive(false);
            });
        }
    }
}