using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private GameObject target;
    private MovementOscillator _movementOscillator;
    
    private void Start()
    {
        target = new GameObject("Actor");
        target.transform.position = Vector3.zero;
        TryGetComponent(out MovementOscillator _oscillator);
        _movementOscillator = _oscillator;
    }

    void Update()
    {
        transform.LookAt(target.transform.position);
        transform.RotateAround(target.transform.position, Vector3.up, Time.deltaTime * moveSpeed);
        transform.position = new Vector3(transform.position.x,_movementOscillator.Movement.y, _movementOscillator.Movement.z);
    }
}
