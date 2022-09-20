using System;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class RotationOscillator : MonoBehaviour
{
    [Header("Random")]
    [SerializeField] private bool randomize;
    [SerializeField] private Vector3 minRotation;
    [SerializeField] private Vector3 maxRotation;
    [SerializeField] private float minfrequency;
    [SerializeField] private float maxfrequency;
    
    [Header("Constant Values")] 
    [SerializeField] private bool X;
    [SerializeField] private bool Y;
    [SerializeField] private bool Z;
    [Space]
    [SerializeField] Vector3 RotationVector;
    [SerializeField] float speed = 2f;
    [Range(0, 1)] [SerializeField] float amplitude;
    Vector3 StartingRot;
    const float tau = Mathf.PI * 2f;
    void Start()
    {
        StartingRot = transform.eulerAngles;

        if (!randomize) return;

        speed = Random.Range(minfrequency, maxfrequency);
        StartingRot.x = Random.Range(StartingRot.x + minRotation.x,StartingRot.x + maxRotation.x);
        StartingRot.y = Random.Range(StartingRot.y + minRotation.y,StartingRot.y + maxRotation.y);
        StartingRot.z = Random.Range(StartingRot.z + minRotation.z,StartingRot.z + maxRotation.z);
    }
 
    void Update()
    {
        if (speed <= Mathf.Epsilon)
        {
            return;
        }
 
        float time = Time.time / speed;
        float RawSinWave = Mathf.Sin(time * tau);
 
        amplitude = RawSinWave / 2f + 0.5f;

        Vector3 offset = new Vector3(0,0,0);
        
        if(X) offset.x = Mathf.Lerp(minRotation.x, maxRotation.x, amplitude);
        if(Y) offset.y = Mathf.Lerp(minRotation.y, maxRotation.y, amplitude);
        if(Z) offset.z = Mathf.Lerp(minRotation.z, maxRotation.z, amplitude);
        
        transform.eulerAngles = StartingRot + offset;
    }
}
