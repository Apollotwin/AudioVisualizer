using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualizer : MonoBehaviour
{
    public static AudioSource _audioSource;
    public static float[] _samples = new float[512];
    public static float[] freqBands = new float[8];
    public static float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];
    public float[] bands;
    [SerializeField, Range(1f, 2f)]private float smoothFactor = 1.2f;
    
    public static float sampleValue { get; private set; }
    public static int sampleRate { get; private set; }

    private float timeSample;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        bands = freqBands;

        if (_samples != null && _samples.Length > 0)
        {
            sampleValue = _samples[0] * 100;
        }

        if (sampleRate == null && _audioSource != null) sampleRate = _audioSource.clip.samples;

        timeSample = _audioSource.timeSamples;
    }
    void GetSpectrumAudioSource() => _audioSource.GetSpectrumData(_samples, 0, FFTWindow.BlackmanHarris);

    void MakeFrequencyBands()
    {
        //Which sample we are at
        int count = 0;

        //Loop through all freq bands
        for (int i = 0; i < 8; i++)
        {
            float averageAmp = 0;
            int sampleCount = (int)Math.Pow(2, i) * 2;

            if (i == 7) sampleCount += 2;

            for (int j = 0; j < sampleCount; j++)
            {
                averageAmp += _samples[count] * (count + 1);
                count++;
            }

            averageAmp /= count;

            freqBands[i] = averageAmp * 10;
        }
    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBands[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBands[i];
                bufferDecrease[i] = 0.005f;
            }
            else
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= smoothFactor;
            }
            
        }
    }
    
    // Track Position getter/setter
    public static int GetTrackPosition()
    {
        return _audioSource.timeSamples;
    }
    public static void SetTrackPosition(int position)
    {
        _audioSource.timeSamples = position;
    }
}
