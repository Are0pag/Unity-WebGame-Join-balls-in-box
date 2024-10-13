using UnityEngine;

[System.Serializable]
public class DefaultCoroutineParamsForHDRColor<T> : DefaultCoroutineParams<T> {
    public override T StartValue { get => _startHDRValue; set => _startHDRValue = value; }
    [ColorUsage(true, true)] public T _startHDRValue;

    public override T FinalValue { get => _finalHDRValue; set => _finalHDRValue = value; }
    [ColorUsage(true, true)] public T _finalHDRValue;

}