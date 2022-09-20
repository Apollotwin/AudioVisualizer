using System;
using UnityEngine;

[DisallowMultipleComponent]
public class FrequencyBandReactor : MonoBehaviour
{
    public int band;

    [Header("Scale")]
    [SerializeField] private bool scaleObject = true;
    
    private Vector3 startPosition;
    private Vector3 startScale;
    public Vector3 maxScale = new Vector3(1,10,1);
    public bool useBuffer = false;
    
    [Header("Activate or Deactivate")]
    [SerializeField] private bool setObjectsActive;
    [SerializeField] private float threshold = 1f;
    public float avarage = 0;
    public float highestAvarage = 0;
    [SerializeField] private GameObject[] objects;
    

    private void OnEnable()
    {
        startScale = transform.localScale;
        startPosition = transform.position;
    }

    void Update()
    {
        if(scaleObject)
        {
            if (useBuffer)
            {
                transform.localScale = new Vector3(transform.localScale.x,
                    (AudioVisualizer.bandBuffer[band] * maxScale.y) + startScale.y, transform.localScale.z);
                transform.position = new Vector3(transform.position.x,
                    (AudioVisualizer.bandBuffer[band] * maxScale.y) / 2 + startPosition.y, transform.position.z);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x,
                    (AudioVisualizer.freqBands[band] * maxScale.y) + startScale.y, transform.localScale.z);
                transform.position = new Vector3(transform.position.x,
                    (AudioVisualizer.freqBands[band] * maxScale.y) / 2 + startPosition.y, transform.position.z);
            }
        }

        if (setObjectsActive)
        {
            for (int i = 0; i < AudioVisualizer.freqBands.Length; i++)
            {
                avarage += AudioVisualizer.freqBands[i];
            }

            avarage /= 8;

            if (avarage > highestAvarage)
            {
                highestAvarage = avarage;
            }
            
            if (avarage > threshold)
            {
                foreach (var go in objects)
                {
                    go.SetActive(true);
                }
            }
            else
            {
                foreach (var go in objects)
                {
                    go.SetActive(false);
                }
            }
        }
    }
}
