using UnityEngine;

public class ReactorManager : MonoBehaviour
{
    public static FrequencyBandReactor[] reactors;
    public static float _maxScale = 1000f;

    private void Start()
    {
        reactors = new FrequencyBandReactor[AudioVisualizer._samples.Length];
    }
}
