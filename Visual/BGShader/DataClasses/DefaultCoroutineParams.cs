using UnityEngine;

[System.Serializable]
public class DefaultCoroutineParams<T> {
    public float TimeOfWorking;
    public virtual T StartValue { get => _startValue; set => _startValue = value; }
    [SerializeField] private T _startValue;

    public virtual T FinalValue { get => _finalValue; set => _finalValue = value; }
    [SerializeField] private T _finalValue;

}
