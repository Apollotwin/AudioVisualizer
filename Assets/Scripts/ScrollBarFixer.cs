using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarFixer : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;

    private void Update()
    {
        if (!scrollbar.gameObject.activeSelf) scrollbar.gameObject.SetActive(true);
    }
}
