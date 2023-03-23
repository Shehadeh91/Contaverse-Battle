using System;
using System.Collections;
using UnityEngine;

namespace Contaquest.UI
{
    public class RectTransformAnimator : MonoBehaviour
    {
        [SerializeField]
        private RectTransform rectTransform;
        [Header("Values")]
        [SerializeField]
        [Tooltip("Multiplies the speed")]
        private float speedMultiplier = 1f;

        private void Awake()
        {
            if (rectTransform == null)
                rectTransform = transform as RectTransform;
        }
        public void SetNewHeight(float height)
        {
            StopAllCoroutines();
            StartCoroutine(SetHeight(height, speedMultiplier));
        }

        private IEnumerator SetHeight(float height, float speedMultiplier)
        {
            float startTime = Time.time;
            float value = rectTransform.rect.height;

            while (true)
            {
                float progress = (Time.time - startTime) / speedMultiplier;

                if (progress > 1)
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                    progress = 1;
                    break;
                }
                value = Mathf.Lerp(value, height, progress);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
                yield return null;
            }
        }
    }
}
