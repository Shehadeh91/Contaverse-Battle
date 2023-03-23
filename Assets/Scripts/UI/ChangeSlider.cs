using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChangeSlider : MonoBehaviour
{
    [SerializeField] private bool useImage;
    [SerializeField] [ShowIf("useImage")] protected Image image = null;
    [SerializeField] [HideIf("useImage")] protected Slider slider = null;

    public void SetSliderProgress(FloatRangeValue newValue)
    {
        if (useImage)
            image.fillAmount = newValue.GetProgress();
        else
            slider.value = newValue.GetProgress();
    }
}