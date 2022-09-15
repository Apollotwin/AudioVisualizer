using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleReactor : MonoBehaviour
{
    public int sample;
    public Vector3 startPosition;
    private Vector3 startScale;
    public Vector3 maxScale;

    private void OnEnable()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        transform.localScale = new Vector3(startScale.x, AudioVisualizer._samples[sample] * maxScale.y + startScale.y, startScale.z);
        transform.position = new Vector3(startPosition.x, AudioVisualizer._samples[sample] * maxScale.y/2 + startPosition.y, startPosition.z);
    }
}
