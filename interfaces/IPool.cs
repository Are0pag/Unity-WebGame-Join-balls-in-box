using Character;
using UnityEngine;

public delegate void CallbackVoid();

public interface IPool {
    public GameObject Take(BallSize ballSize, Vector3 pos);
    public void Release(params GameObject[] objects);
    public GameObject Create(BallSize ballSize, Vector3 pos);
}