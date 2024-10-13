using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoresDisplayManager : MonoBehaviour {
	public static ScoresDisplayManager Instance {get; private set; }
    [SerializeField] private GameObject _floatingTextPrefab;
    [SerializeField] private float _livetimeOfFloatingText, _finalScaleOfFloatingText, _deltaPosYUp;
    private List<FloatingText> _floatingItems = new List<FloatingText>();

    private void Awake() => Instance = this;

    public void SetFloatingText(Vector3 collisionPoint, int addingScores) {
        var floatingText = Take(collisionPoint);
        floatingText.InitTopDown(addingScores, _livetimeOfFloatingText,_finalScaleOfFloatingText,_deltaPosYUp);
    }

    private FloatingText Take(Vector3 collisionPoint) {
        var ft = _floatingItems.FirstOrDefault(i => !i.isActiveAndEnabled);
        if (ft != null) {
            ft.gameObject.transform.position = collisionPoint;  
            ft.gameObject.SetActive(true); 
            return ft;
        }
        else {
            return Create(collisionPoint);
        }
    }
    public void Release(FloatingText floatingText) {
        floatingText.gameObject.SetActive(false);
    }
    private FloatingText Create(Vector3 collisionPoint) {
        var go = Instantiate(_floatingTextPrefab, collisionPoint, Quaternion.identity, transform);
        var ft = go.GetComponent<FloatingText>();
        _floatingItems.Add(ft);
        return ft;
    }
}