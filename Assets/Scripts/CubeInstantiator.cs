using System;
using UnityEngine;

public class CubeInstantiator : MonoBehaviour
{
    public GameObject frequncy_prefab;
    public GameObject sample_prefab;
    private GameObject[] circleObjects;
    private GameObject[] lineObjects;
    public int amount = 10;
    public float radius = 25;
    public float pad = 1f;

    void Start()
    {
        FormCircle();
        FormLine();
        transform.position = Vector3.zero;
    }
  
    void DivideIntoFrequencyBands(int bandAmount, GameObject[] gameObjects)
    {
        for (var index = 0; index < bandAmount; index++) 
        {
            var gameObject = gameObjects[index];
            gameObject.TryGetComponent(out FrequencyBandReactor audioReactor);
            audioReactor.band = index;
            audioReactor.useBuffer = true;
        }
    }

    public void FormCircle()
    {
        circleObjects = new GameObject[AudioVisualizer._samples.Length];
        GameObject parent = new GameObject("Circle");
        parent.transform.position = transform.position;
        
        for (int i = 0; i < circleObjects.Length; i++)
        {
            GameObject gameObject = Instantiate(sample_prefab,parent.transform);
            gameObject.name = $"{sample_prefab.name}_{i + 1}";
            
            float angle = 360f / circleObjects.Length;
            gameObject.transform.eulerAngles = new Vector3(0, angle * i, 0);
            gameObject.transform.position = Vector3.forward * radius;

            gameObject.TryGetComponent(out SampleReactor sampleReactor);
            sampleReactor.startPosition = gameObject.transform.position;
            sampleReactor.sample = i;
            
            circleObjects[i] = gameObject;
        }
    }

    public void FormLine()
    {
        lineObjects = new GameObject[8];
        GameObject parent = new GameObject("Line");
        float cubeScale = frequncy_prefab.transform.localScale.x + pad;
        float startPos = cubeScale * 8 / -2;

        for(int i = 0; i < lineObjects.Length; i++)
        {
            GameObject gameObject = Instantiate(frequncy_prefab,parent.transform);
            gameObject.name = $"{frequncy_prefab.name}_{i}";
            gameObject.transform.position = new Vector3(startPos + cubeScale * (i + 1), 0f, 0f);
            lineObjects[i] = gameObject;
        }

        parent.transform.position = new Vector3(0, 0, 0);
        DivideIntoFrequencyBands(AudioVisualizer.freqBands.Length,lineObjects);

    }

}
