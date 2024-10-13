using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LigthnessShader : MonoBehaviour {
    public static LigthnessShader Instance {  get; private set; }
    public Coroutine WorkingCoroutine { get; set; }
    public static UnityAction OnGrapficHaveCorrespondMoment {  get; set; }
    public static UnityAction OnDisable {  get; set; }

    [SerializeField][Range(0f, 5f)] private float _appearTime, _dissapierTime;
    [SerializeField] private float _startDistortionAmount, _endDistortionAmount, _startNoiseScale, _endNoiseScale;
    [SerializeField] private string _rectangleHeigth, _distortionAmount, _noiseScale;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Material _material;

    private void Awake() {
        Instance = this;
        _material = _spriteRenderer.material;
    }

    public void Disable() => WorkingCoroutine = StartCoroutine(Dissapear(_dissapierTime));
    private void OnEnable() => WorkingCoroutine = StartCoroutine(Appear(_appearTime));

    private IEnumerator Appear(float appearTime) {
        _material.SetFloat(_rectangleHeigth, 0f);
        _material.SetFloat(_noiseScale, _startNoiseScale);
        _material.SetFloat(_noiseScale, _endNoiseScale);
        _material.SetFloat(_distortionAmount, _startDistortionAmount);

        float elapsedTime = 0f;
        while (elapsedTime < appearTime) {  
            elapsedTime += Time.deltaTime; 

            float heithEffectAmount = Mathf.Lerp(0f,1f, elapsedTime / appearTime);
            _material.SetFloat(_rectangleHeigth, heithEffectAmount);

            float noiseScaleEffectAmount = Mathf.Lerp(_startNoiseScale, _endNoiseScale, elapsedTime / appearTime);
            _material.SetFloat(_noiseScale, noiseScaleEffectAmount);

            yield return null;
        }
        StopCoroutine(WorkingCoroutine);
        WorkingCoroutine = null;
        OnGrapficHaveCorrespondMoment?.Invoke();
    }

    private IEnumerator Dissapear(float dissapiarTime) {
        _material.SetFloat(_noiseScale, _startNoiseScale);

        float elapsedTime = 0f;
        while (elapsedTime < dissapiarTime) {
            elapsedTime += Time.deltaTime;
            float effectAmount = Mathf.Lerp(_startDistortionAmount, _endDistortionAmount, elapsedTime / dissapiarTime);
            _material.SetFloat(_distortionAmount, effectAmount);
            yield return null;
        }
        StopCoroutine(WorkingCoroutine);
        WorkingCoroutine = null;
        OnDisable?.Invoke();
    }

    private void OnValidate() => _spriteRenderer = _spriteRenderer != null ? _spriteRenderer : GetComponent<SpriteRenderer>();
}