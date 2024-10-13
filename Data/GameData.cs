using Character;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour {
    public static GameData Instance { get; private set; }
    public List<ScriptableBall> ScriptableBalls { get => _scriptableBalls; private set => _scriptableBalls = value; }
    [SerializeField] private List<ScriptableBall> _scriptableBalls = new List<ScriptableBall>();

    [ContextMenu("SaveSerializeList")]
    private void Awake() {
        Instance = this;
        if (_scriptableBalls == null || _scriptableBalls.Count == 0) {
            _scriptableBalls = Resources.LoadAll<ScriptableBall>("Balls").ToList();
        }
    }

    public ScriptableBall GetBallBySize(BallSize size) {
        var sb = _scriptableBalls.FirstOrDefault(b => b.BallSize == size);
        return sb;
    }
}