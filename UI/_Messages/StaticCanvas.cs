using UnityEngine;
using UnityEngine.UI;

public class StaticCanvas : MonoBehaviour {
    public static StaticCanvas Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }
    [SerializeField] private Button _pauseBut, _settingsBut;

    private void Start() {
        _pauseBut.onClick.AddListener(() => { 
            PauseManager.Instance.ShowToPlayer(PauseManager.MessageType.Pause); 
        });
        _settingsBut.onClick.AddListener(() => { 
            PauseManager.Instance.ShowToPlayer(PauseManager.MessageType.Settings); 
        });
    }

}