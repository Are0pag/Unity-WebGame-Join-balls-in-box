using UI_Messages;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour {
	public static PauseManager Instance {get; private set; }
    public static UnityAction ButtonAction { get; set; }
    private void Awake() => Instance = this;

    public MessageType DisplayedMessage { get; private set; }
    public enum MessageType {
        Nothing,
        Pause,
        Settings,
        Lose,
        Win
    }

    public void ShowToPlayer(PauseManager.MessageType message) {        
        DisplayedMessage = message;
        InitCloseButton(message);
        MessageDisplay.Instance.OpenMessage(message);
    }

    private static void InitCloseButton(MessageType message) {
        switch (message) {
            case MessageType.Nothing:
                break;
            case MessageType.Pause:
                ButtonAction += () => {
                    GameManager.Instance.ChancheGameState(GameState.Playing);
                    ButtonAction = null;
                };
                break;
            case MessageType.Settings:
                ButtonAction += () => {
                    GameManager.Instance.ChancheGameState(GameState.Playing);
                    ButtonAction = null;
                };
                break;
            case MessageType.Lose:
                ButtonAction += () => {
                    GameManager.Instance.OnLoose();
                    GameManager.Instance.ChancheGameState(GameState.Playing);
                    ButtonAction = null;
                };
                break;
            case MessageType.Win:
                ButtonAction += () => {
                    GameManager.Instance.ChancheGameState(GameState.Playing);
                    ButtonAction = null;
                };
                break;
        }
    }
}