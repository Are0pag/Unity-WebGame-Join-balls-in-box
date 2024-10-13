using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void CallbackCoroutine();
public delegate Coroutine InitNewCoroutine();

public class UniversalCoroutineUser : MonoBehaviour {
    public Dictionary<Guid, Coroutine> WorkingCoroutines { get; private set; }
    public CallbackCoroutine StopCallback { get; private set; }

    protected InitNewCoroutine _initNewCoroutine;
    protected Coroutine _currentCoroutine;
    protected Guid _currentId;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    protected Material _material;

    protected virtual void Start() {
        _material = _spriteRenderer.material;
    }

    protected void SetNewCoroutine() {
        InitCurrentId();
        InitCallBack();
        AddCoroutine(_initNewCoroutine?.Invoke());
    }

    protected virtual void InitCurrentId() {
        _currentId = Guid.NewGuid();
    }
    protected virtual void InitCallBack() {
        StopCallback = () => {
            var correspond =
                 this.WorkingCoroutines.FirstOrDefault(w => w.Key.Equals(_currentId));
            StopCoroutine(correspond.Value);
        };
    }
    protected virtual void AddCoroutine(Coroutine correspond) {
        WorkingCoroutines ??= new Dictionary<Guid, Coroutine>();
        WorkingCoroutines[_currentId] = correspond;
    }

    protected virtual void OnValidate() {
        _spriteRenderer = _spriteRenderer != null ? _spriteRenderer : GetComponent<SpriteRenderer>();
    }
}
