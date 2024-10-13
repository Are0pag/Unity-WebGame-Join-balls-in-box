using System.Collections.Generic;
using UnityEngine;

public class BgShaderSetter : UniversalCoroutineUser {
    public static BgShaderSetter Instance { get; private set; }
    private void Awake() => Instance = this;

    [SerializeField] private string _colorName;
    [SerializeField] DefaultCoroutineParamsForHDRColor<Color> _colorParams;
    [SerializeField] DefaultCoroutineParamsForHDRColor<Color> _colorRevertParams;

    protected override void Start() {
        base.Start();
        SetColor();
    }

    private void SetColor() {
        _initNewCoroutine = null;
        _initNewCoroutine = delegate () {
            return StartCoroutine(UniversalCoroutine.Instance.MaterialTargeted_Several_Generic
                (_material, new Dictionary<string, DefaultCoroutineParams<Color>> { { _colorName, _colorParams }, },
                () => {
                    base.StopCallback();
                    SetRevertColor();
                }
            ));
        };
        SetNewCoroutine();
    }
    private void SetRevertColor() {
        _initNewCoroutine = null;
        _initNewCoroutine = delegate () {
            return StartCoroutine(UniversalCoroutine.Instance.MaterialTargeted_Several_Generic(
               _material, new Dictionary<string, DefaultCoroutineParams<Color>> { { _colorName, _colorRevertParams } },
               () => {
                   base.StopCallback();
                   SetColor();
               }
            ));
        };
        SetNewCoroutine();
    }
}