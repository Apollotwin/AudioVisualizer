using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private GameObject target;
    private Oscillator oscillator;
    
    private void Start()
    {
        target = new GameObject("Actor");
        target.transform.position = Vector3.zero;
        TryGetComponent(out Oscillator _oscillator);
        oscillator = _oscillator;
    }

    void Update()
    {
        transform.LookAt(target.transform.position);
        transform.RotateAround(target.transform.position, Vector3.up, Time.deltaTime * moveSpeed);
        transform.position = new Vector3(transform.position.x,oscillator.Movement.y, oscillator.Movement.z);
    }
}
