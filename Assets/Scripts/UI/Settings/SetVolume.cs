﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private FloatRangeVariable variable;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private AudioMixer audioMixer = null;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private bool playSoundCheck = true;

    private void Start()
    {
        slider.value = variable.Value.Value;
    }
    
    public void ChangeVolume()
    {
        if(!gameObject.activeInHierarchy)
            return;
            
        variable.Value.Value = slider.value;

        SetVolumeLevel(slider.value);
        if (playSoundCheck)
        {
            playSoundCheck = false;
            sfxSource?.Play();
            StartCoroutine(Stopper());
        }
    }

    public void SetVolumeLevel(float value)
    {
        if (value == 0)
            audioMixer.SetFloat(variable.variableName, -80f);
        else
            audioMixer.SetFloat(variable.variableName, Mathf.Log10(value) * 20);
    }

    private IEnumerator Stopper()
    {
        yield return new WaitForSeconds(0.1f);
        playSoundCheck = true;
    }
}
