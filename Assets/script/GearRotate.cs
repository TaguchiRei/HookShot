using UnityEngine;

public class GearRotate : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] float rotateSpeed = 1.0f;
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * 2f);
    }
}
