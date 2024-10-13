using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UniversalCoroutine : MonoBehaviour {
    public static UniversalCoroutine Instance { get; private set; }
    private void Awake() => Instance = this;

    public IEnumerator MaterialTargeted_Several_Generic<T>(Material material, Dictionary<string, DefaultCoroutineParams<T>> propertiesData, CallbackCoroutine call) {
        float elapsedTime = 0f;
        float parallelEffectTime = propertiesData.First().Value.TimeOfWorking;

        while (elapsedTime < parallelEffectTime) {
            elapsedTime += Time.deltaTime;
            foreach (var item in propertiesData.Keys) {
                if (typeof(T) == typeof(float)) {
                    var value = propertiesData[item] as DefaultCoroutineParams<float>;
                    SetFloat(material, item, value, elapsedTime, parallelEffectTime);
                } else if (typeof(T) == typeof(Color)) {
                    var value = propertiesData[item] as DefaultCoroutineParams<Color>;
                    SetColor(material, item, value, elapsedTime, parallelEffectTime);
                }
            }
            yield return null;
        }
        call?.Invoke();
    }

    private static void SetColor(Material material, string name, DefaultCoroutineParams<Color> coroutineParams, float elapsedTime, float parallelEffectTime) {
        var currentValue = Color.Lerp(coroutineParams.StartValue, coroutineParams.FinalValue, elapsedTime / parallelEffectTime);
        material.SetColor(name, currentValue);
    }

    private static void SetFloat(Material material, string name, DefaultCoroutineParams<float> coroutineParams, float elapsedTime, float parallelEffectTime) {
        var currentValue = Mathf.Lerp(coroutineParams.StartValue, coroutineParams.FinalValue, elapsedTime / parallelEffectTime);
        material.SetFloat(name, currentValue);
    }

}
