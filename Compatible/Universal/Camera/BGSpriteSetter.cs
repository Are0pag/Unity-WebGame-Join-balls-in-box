using UnityEngine;

public class BGSpriteSetter : MonoBehaviour {
    [Header("Empiric data about raatio between \n BG sptire LocalScale and Screen ratio: \n 1 : 7.285 by X and 1 : 12.84 by Y.")]
    [Header("Method, that set localScale by X \n works on Start, but Y don`t. \n Either methods works in Update.")]

    [Header("Ratio of BG sprite localScale.x \n to rendering screen size is 1 to: ")]
    [SerializeField][Range(7.2f, 7.3f)] private float _ratioX;  

    [Header("Ratio of BG sprite localScale.y \n to rendering screen size is 1 to: ")]
    [SerializeField][Range(12.8f, 12.9f)] private float _ratioY;

    [SerializeField] private SpriteRenderer[] _bgSprites;
    [SerializeField] private Camera _camera;

    private void Update() {
        var screenRatio = (float)Screen.width / (float)Screen.height;
        var spriteRatio = _bgSprites[0].bounds.size.x / _bgSprites[0].bounds.size.y;
        if (screenRatio != spriteRatio) {
            var topRigth = _camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
            var bottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
            if (screenRatio > spriteRatio) {
                // x > y
                SetLocalScaleX(topRigth, bottomLeft);
            }
            else {
                // y > x
                SetLocalScaleY(topRigth, bottomLeft);
            }
        }
    }

    private void SetLocalScaleY(Vector3 topRigth, Vector3 bottomLeft) {
        float renderingScreenSizey = (Mathf.Abs(topRigth.y) + Mathf.Abs(bottomLeft.y));
        foreach (var item in _bgSprites) {
            item.transform.localScale = new Vector3(item.transform.localScale.x, (renderingScreenSizey / _ratioY), 0f);
        }
    }

    private void SetLocalScaleX(Vector3 topRigth, Vector3 bottomLeft) {
        float renderingScreenSizeX = (Mathf.Abs(topRigth.x) + Mathf.Abs(bottomLeft.x));
        foreach (var item in _bgSprites) {
            item.transform.localScale = new Vector3((renderingScreenSizeX / _ratioX), item.transform.localScale.y, 0f);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("PrintWCoord")]
    private void PrintWCoord() {
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        var tr = camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        var bl = camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        print("topRigth = " + tr + " bottomLeft = " + bl);
    }
    private void OnValidate() {
        _camera = _camera != null ? _camera : GameObject.Find("Main Camera").GetComponent<Camera>();
    } 
#endif
}