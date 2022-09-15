using System;
using UnityEngine;

[DisallowMultipleComponent]
public class FrequencyBandReactor : MonoBehaviour
{
    public int band;
    private Vector3 startPosition;
    private Vector3 startScale;
    public Vector3 maxScale = new Vector3(1,10,1);
    public bool useBuffer = false;

    private void OnEnable()
    {
        startScale = transform.localScale;
        startPosition = transform.position;
    }

    void Update()
    {
        if(useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (AudioVisualizer.bandBuffer[band] * maxScale.y) + startScale.y, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, (AudioVisualizer.bandBuffer[band] * maxScale.y)/2 + startPosition.y, transform.position.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, (AudioVisualizer.freqBands[band] * maxScale.y) + startScale.y, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, (AudioVisualizer.freqBands[band] * maxScale.y)/2 + startPosition.y, transform.position.z);
        }
    }
}
