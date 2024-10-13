using UnityEngine;

public class OrtographicSizeSetter : MonoBehaviour {
    [SerializeField] private Transform bottomL, topR;

    private void Update() {
        SetOrtograpficSize();
    }

    private void SetOrtograpficSize() {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = (topR.position.x - bottomL.position.x) / (topR.position.y - bottomL.position.y);

        if (screenRatio >= targetRatio) {
            Camera.main.orthographicSize = (topR.position.y - bottomL.position.y) / 2;
        }
        else {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = (topR.position.y - bottomL.position.y) / 2 * differenceInSize;
        }
    }
}