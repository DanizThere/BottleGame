using UnityEngine;

public abstract class Decision : MonoBehaviour
{
    [SerializeField] protected AnimationCurve curve;
    public abstract float Decide(Enemy enemy);
}
