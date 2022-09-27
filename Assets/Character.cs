using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private AudioVisualizer _audioVisualizer;
    [SerializeField] private Animator _animator;
    
    private void Update()
    {
        _animator.SetBool("IsDancing", AudioVisualizer._audioSource.isPlaying);
    }
}
