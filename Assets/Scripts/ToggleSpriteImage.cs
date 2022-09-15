using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSpriteImage : MonoBehaviour
{
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    private Image image;
    private bool toggle = false;

    private void Start()
    {
        gameObject.TryGetComponent(out Image _image);
        image = _image;
        image.sprite = sprite1;
    }

    public void ToggleSprite()
    {
        toggle = !toggle;
        image.sprite = toggle ? sprite2 : sprite1;
    }
    
}
