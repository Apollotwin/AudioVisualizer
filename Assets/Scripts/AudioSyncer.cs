using System;
using AudioImporter.Scripts;
using NAudio.Dsp;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    [SerializeField,Tooltip("Determines what sample value is going to trigger a beat")] 
    private float sampleTriggerValue;

    [SerializeField, Tooltip("The minimal time between beats")] 
    private float  timeStep;

    [SerializeField, Tooltip("How much time before the visualization completes")] 
    private float timeToBeat;

    [SerializeField, Tooltip("How fast the object goes to rest after a beat. Example: How fast a object go from a scale of 2 to a scale of 1 after a beat.")] 
    private float restSmoothTime;

    [SerializeField] private float sensitivity = 1.5f;
    

    private float previousSampleValue;
    private float currentSampleValue;
    private float timer;

    protected bool isBeat;

    [SerializeField] private OnSetDetection onSetDetection;
    
    private void Update()
    {
        //TODO https://medium.com/giant-scam/algorithmic-beat-mapping-in-unity-real-time-audio-analysis-using-the-unity-api-6e9595823ce4
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        previousSampleValue = currentSampleValue;
        currentSampleValue = AudioVisualizer.sampleValue;

        if (previousSampleValue > sampleTriggerValue &&
            currentSampleValue <= sampleTriggerValue)
        {
            if(timer > timeStep) OnBeat();
        }

        if (previousSampleValue <= sampleTriggerValue &&
            currentSampleValue > sampleTriggerValue)
        {
            if(timer > timeStep) OnBeat();
        }

        timer += Time.deltaTime;
    }

    public virtual void OnBeat()
    {
        timer = 0;
        isBeat = true;
    }
}
