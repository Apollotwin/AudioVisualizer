using System;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementOscillator : MonoBehaviour
{
    [SerializeField] Vector3 MovementVector;
    [SerializeField] float Period = 2f;
    [Range(0, 1)] [SerializeField] float MovementFactor;
    Vector3 StartingPos;
    [HideInInspector]
    public Vector3 Movement;
    void Start()
    {
        StartingPos = transform.position;
    }
 
    void Update()
    {
        if (Period <= Mathf.Epsilon)
        {
            return;
        }
 
        float time = Time.time / Period;
 
        const float tau = Mathf.PI * 2f;
        float RawSinWave = Mathf.Sin(time * tau);
 
        MovementFactor = RawSinWave / 2f + 0.5f;
        Vector3 offset = MovementFactor * MovementVector;
        transform.position = StartingPos + offset;
    }
}
