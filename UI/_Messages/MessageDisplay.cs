using UnityEditor;
using UnityEngine;

namespace UI_Messages {
    public interface IReceiver {
        public UI_Messages.Links Links { get; }
    }

    public class MessageDisplay : MonoBehaviour, IReceiver {
        public static MessageDisplay Instance { get; private set; }
        public Links Links { get { return _links; } private set { _links = value; } }
        [SerializeField] private Links _links;
        [SerializeField] private AnimPanel[] _animPanels;

        private void Awake() {
            Instance = this;
        }
        public void OpenMessage(PauseManager.MessageType message) {
            Links.Panel.SetActive(true);
            _animPanels[0].OnPanelEnable += () => { GameManager.Instance.ChancheGameState(GameState.Pause); };
            SetButtonAction();
            ActivateCorrespondUI(message);
            Links.Text.text = Links.GetDataByMessageType(message).MessageText;
        }

        private void ActivateCorrespondUI(PauseManager.MessageType message) {
            if (message == PauseManager.MessageType.Settings) {
                foreach (var item in Links.Settings) {
                    item.SetActive(true);
                }
            }
            else {
                foreach (var item in Links.Settings) {
                    item.SetActive(false);
                }
            }
        }

        private void SetButtonAction() {
            PauseManager.ButtonAction += () => {
                AudioManager.PlayButtonSound();
                DisableWithAnimation();
            };
            Links.Button.onClick.RemoveAllListeners();
            Links.Button.onClick.AddListener(PauseManager.ButtonAction);
        }

        private void DisableWithAnimation() {
            foreach (var item in _animPanels) {
                if (item.isActiveAndEnabled) {
                    if (item.gameObject.activeSelf) {
                        item.DisablePanel(() => {
                            Links.Panel.SetActive(false);
                        });
                    } 
                }
            }
        }

        private void OnValidate() => _links = _links != null ? _links : GetComponent<Links>();
    }

}