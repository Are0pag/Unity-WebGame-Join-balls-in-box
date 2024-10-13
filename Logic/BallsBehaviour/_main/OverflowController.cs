using UnityEngine;

namespace GameRuntime {
	public class OverflowController : MonoBehaviour {
		public static OverflowController Instance { get; private set; }
		private void Awake() => Instance = this;

        private void NotifyAboutOverflow() {
            PauseManager.Instance.ShowToPlayer(PauseManager.MessageType.Lose);
        }


        private void OnEnable() {
            OverflowCollider.OnOverflow += NotifyAboutOverflow;
        }
        private void OnDisable() {
            OverflowCollider.OnOverflow -= NotifyAboutOverflow;
        }
    } 
}