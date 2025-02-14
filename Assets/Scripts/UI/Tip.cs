using UnityEngine;

public class Tip : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }

    public void Move(Vector3 mousePos)
    {
        transform.rotation = cam.transform.rotation;
        Vector3 posWithOffset = mousePos - offset;

        transform.localPosition = posWithOffset;
    }

    private void OnEnable()
    {
        offset = new Vector3(0, transform.localScale.y / 2 + 2, 0);
    }
}
