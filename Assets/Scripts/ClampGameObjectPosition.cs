using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampGameObjectPosition : MonoBehaviour
{
    [SerializeField] private Vector2 clampX;
    [SerializeField] private Vector2 clampY;

    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (rect.position.y < clampY.x)
        {
            rect.position = new Vector3(0, clampX.x);
        }
    }
}
